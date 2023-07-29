(function () {

  /* ---------------------------------------- */
  /* ------------- SETUP/CACHES ------------- */
  /* ---------------------------------------- */

  let contentCache = {};
  let sessionAge = 0;
  let lastFetch = 0;
  function loadContentCache () {
    /* Throttle to one reload every half a second, just in case there are performance implications. */
    const date = new Date().valueOf();
    if (date - lastFetch < 500) { return; }

    lastFetch = date;
    sessionAge = Number.parseInt(
      sessionStorage.getItem('ItakuEnhancedContentCacheAge')) || 0;
    contentCache = JSON.parse(
      sessionStorage.getItem('ItakuEnhancedContentCache')) || {};
  }

  function saveContentCache () {
    sessionAge = new Date().valueOf(); /* TODO: this is mostly useless */
    sessionStorage.setItem('ItakuEnhancedContentCacheAge', sessionAge);
    sessionStorage.setItem('ItakuEnhancedContentCache', JSON.stringify(contentCache));
  }

  /* Session storage isn't shared between new tabs in all situations.
   * As part of fetch, we should also query the content script to see
   * if we can pull down data from another tab's cache.
   *
   * TODO: check to see if this will break multiple accounts.
   * TODO: this is still imperfect, but I'm not sure of a better
   * way to handle session state between tabs. */
  async function syncContent () {
    const response = await browser.runtime.sendMessage({
      content: contentCache,
      type: 'sync',
    });

    /* Don't trigger a sessionStorage write if we don't need to (new content or newer date) */
    if (response && response.content.length) {
      response.content.forEach((item) => {
        contentCache[item.id] = item;
      });

      saveContentCache();
    }
  }

  /* Refresh the content cache (and connection to the content script) whenever the tab is loaded. This prevents
   * us from running into problems where you load notifications and then swap to a different tab. It also
   * prevents problems where the scripts wait so long or sleep that their ports and listeners get removed. */
  loadContentCache();
  syncContent();
  document.addEventListener('visibilitychange', () => {
    if (document.visibilityState === 'visible') {
      loadContentCache();
      connectPort();
      appObserver.disconnect();
      appObserver.observe(document.body, { childList: true });
      syncContent();
    }
  });


  function connectPort () {
    const tabId = new Date().valueOf();
    const port = browser.runtime.connect({
      name: `notifications-port-${tabId}`
    });

    let listener = (evt, sender, response) => {
      switch (evt.type) {
      case 'cache':
        loadContentCache(); /* Always reload the cache before trying to save or add new items. */
        evt.content.forEach((item) => {
          contentCache[item.id] = item;
        });

        saveContentCache();

        /* Update UI (if possible). Try to be Reactive about things. */
        extendNotifications(notificationsSubContainer);
        break;
      }
    }

    port.onMessage.addListener(listener);
    port.onDisconnect.addListener((evt, sender, response) => {
      port.onMessage.removeListener(listener);
    });
  }

  /* Send our local cache and see if we're missing anything. */

  /* ------------------------------------------- */
  /* ---------- INTERFACE HELPERS -------------- */
  /* ------------------------------------------- */
  function icon (type) {
    const element = document.createElement('mat-icon');
    element.classList = 'mat-icon material-icons mat-icon-inline mat-icon-no-color';
    element.setAttribute('inline', 'True');
    element.setAttribute('aria-hidden', 'true');
    element.setAttribute('data-mat-icon-type', 'font');
    element.innerText = type;

    return element;
  }


  /* ------------------------------------------- */
  /* ------------ COMPONENT LOGIC -------------- */
  /* ------------------------------------------- */

  let fetches = {};
  let fetchQueue = [];
  let currentFetches = 0;
  const MAX_FETCHES = 1;
  /* We don't want to overwhelm the backend with fetches. To help
   * avoid that problem, we only allow a limited number of fetches at a time. The rest
   * are queued up in the background and wait for their turn. Itaku's API for this
   * kind of thing is pretty fast, so I've purposefully set the limit to be very
   * low (1) because I don't think it'll slow down the extension much, and
   * that allows us to have even more breathing room. We also enforce a
   * delay on new requests because... why not? It just slows things
   * down a little bit more and give a little more breathing room. */
  function runFetchQueue () {
    if (fetchQueue.length === 0) {
      return;
    }

    if (currentFetches > MAX_FETCHES) {
      console.log('Deferring fetch, queue full...');
      return;
    }

    currentFetches++;
    const next = fetchQueue.shift();
    next().then(() => {
      setTimeout(() => {
        --currentFetches;
        runFetchQueue();
      }, 200);

    }, () => {
        setTimeout(() => {
          --currentFetches;
          next.tries = next.tries || 0;
          ++next;

          /* Avoid accidentally DDOSing the site. You get 3 tries max. */
          if (next.tries < 3) {
            fetchQueue.push(next);
          }

          runFetchQueue();
        }, 200);
    });

  }

  function fetchResources (type, id, commentId) {
    const cacheId = commentId || id;

    /* Where do we need to fetch? */
    const url = (() => {
      switch (type) {
      case 'images':
        return commentId ?
          `/api/galleries/images/${id}/comments/?&page=1&page_size=30&child_page_size=100` :
          `/api/galleries/images/${id}/`;

      case 'posts':
        return commentId ?
          `/api/posts/${id}/comments/?&page=1&page_size=30&child_page_size=100` :
          `/api/posts/${id}/`;
      }
    })();


    /* Are we already fetching this? If so, we're not going to duplicate the
     * request. Note that fetching multiple comments under the same post should
     * only trigger one fetch, because we get all of the comments for that post
     * as a batch operation. */
    if (fetches[url]) {
      console.log(`Skipping ${url} (fetch already in progress)`);

      /* Sometimes we can get stuck with pending fetches though. Because
       * the fetch queue handles deduplicating just fine on its own, we
       * might as well re-trigger it. */
      runFetchQueue();
      return;
    }

    /* Create the fetch request, set it, and queue it up. */
    fetches[url] = (async () => {

      /* I'm a little bit amazed that I don't need to do more here. Fetching
       * the url will route through the backend, which will automatically update
       * the cache once it's finished. The UI will get notified of cache updates
       * automatically and we refresh the UI when that happens. So just making
       * the request is enough. */
      console.log('fetching info for: ', url);
      try {
        const response = await fetch(`https://itaku.ee${url}`);
        console.log(url, 'response recieved');
      } catch (err) {
        console.log(err); /* This will get automatically retried. */
      }
    });

    /* Start request. */
    fetchQueue.push(fetches[url]);
    runFetchQueue();
  }

  /* DOM manipulation. */
  function extendNotifications (notifications) {
    if (!notifications) { return; } /* We haven't been attached yet. */

    const targets = notifications.querySelectorAll('a.notif-wrapper');
    targets.forEach((target) => {
      const urlInfo = (() => { /* Actual id + the id we would need to fetch */
        const url = new URL(target.href);
        const segments = (url.pathname + url.search).split('/').slice(1); /* drop preceding "/" */
        const finalSegment = segments[segments.length - 1];
        const ids = finalSegment.split('=');

        if (ids.length > 1) { ids[0] = ids[0].split('?')[0]; }
        return [segments[0], ...ids];
      })();

      /* The actual content id */
      const type = urlInfo[0];
      const id = urlInfo[urlInfo.length - 1];

      /* Follows don't need to be changed. */
      if (type === 'profile') { return; }

      /* If it's not already cached, queue a fetch. We also do a
       * mildly fragile check to see if this is for a comment, since
       * those are separate API requests and they can be attached
       * to images/posts that the user doesn't own. */
      let cache = contentCache[id];
      if (!cache || cache.loading) {
        fetchResources(type, urlInfo[1], urlInfo[2]);
        cache = { id: id, loading: true, };
      }

      /* If it's cached, but there is no description/title, then nothing we can do. */
      if (!cache.title && !cache.description) {
        return;
      }

      /* Get the container */
      /* TODO: this still seems fragile... */
      const labelContainer = target.querySelector('div > span');
      labelContainer.setAttribute('data-itakuenhanced-processed', 'true');

      /* TODO: this is unused, but there's stuff I want to do with it UwU */
      // const name = labelContainer.querySelector('a.accent-link');
      // const base = labelContainer?.querySelector('span')?.innerText;
      // if (!base) { return; }

      // const matches = base.match(/(.*) your ([a-z]+)[!]*/);
      // const prefix = matches[1];
      // const notificationType = matches[2];
      // if (!prefix || !notificationType) { return; }

      // const indicatorType = (() => {
      //   switch (notificationType) {
      //   case 'comment': return 'comment';
      //   case 'image': return 'image';
      //   case 'post': return 'edit';
      //   default: return null;
      //   }
      // })();

      // if (!indicatorType) { return; }
      // const indicator = icon(indicatorType);

      if (labelContainer.querySelector('.ItakuEnhanced--notificationDescription')) {
        return;
      }

      const description = document.createElement('div');
      description.setAttribute('data-itakuenhanced-processed', 'true');
      description.classList = 'ItakuEnhanced--notificationDescription';
      description.innerText = cache.title || cache.description;
      labelContainer.appendChild(description);
    });
  }

  /* Actual notifications observation logic. */
  let notificationsContainer;
  let notificationsSubContainer;
  let debouncing = null;
  const notificationsObserver = new MutationObserver((list, observer) => {
    /* Throttle to avoid unnecessary changes */
    const ignore = list.reduce((result, mutation) => {
      if (mutation.type === 'attributes' &&
          mutation.attributeName === 'data-itakuenhanced-processed' &&
          mutation.target.attributes['data-itakuenhanced-processed'].value === "true") {

        return result && true;
      }

      if (mutation.type === 'childList' &&
          mutation.target !== notificationsSubContainer &&
          mutation.target !== notificationsContainer) {
        return result && true;
      }

      return false;
    }, true);

    if (ignore) { return; }

    /* There are a lot of things that call this method like its candy,
     * so we want to basically just make sure its not over-eager to fill
     * up the call stack. */
    if (!debouncing) {
      debouncing = true;
      setTimeout(() => {

        /* handle mutations, refresh references. Yes, this is annoying but we have to do it
         * because of how we're handling observers... Maybe there's a better way,  I don't know.
         * I'll think about it for future releases. */
        const currentNotificationsSubContainer = notificationsContainer.querySelector('.notification-menu');
        if (currentNotificationsSubContainer !== notificationsSubContainer) {
          notificationsObserver.disconnect();
          notificationsSubContainer = currentNotificationsSubContainer;
          if (notificationsContainer) {
            notificationsObserver.observe(notificationsContainer, { childList: true });
          }

          if (notificationsSubContainer) {
            notificationsObserver.observe(notificationsSubContainer, {
              attributeFilter: ["data-itakuenhanced-processed"],
              // characterData: true,
              attributes: true,
              childList: true,
              subtree: true,
            });
          }
        }

        // console.log('triggered notifications hooks (debounced)');
        extendNotifications(notificationsSubContainer);
        debouncing = false;
      }, 100);
    }
  });

  /* Parent logic to avoid slowing down the page. */
  let popup;
  const parentObserver = new MutationObserver((list, observer) => {

    /* If anything has changed, disconnect the existing observer */
    const currentNotificationsContainer = popup.querySelector('app-notifications');
    const currentNotificationsSubContainer = popup.querySelector('app-notifications .notification-menu');

    if (currentNotificationsContainer !== notificationsContainer || /* Hidden */
        currentNotificationsSubContainer !== notificationsSubContainer) {

      notificationsObserver.disconnect();

      /* And attempt to reconnect */
      notificationsContainer = currentNotificationsContainer;
      notificationsSubContainer = currentNotificationsSubContainer;
      if (notificationsContainer) {
        notificationsObserver.observe(notificationsContainer, { childList: true });
      }

      if (notificationsSubContainer) {
        notificationsObserver.observe(notificationsSubContainer, {
          attributeFilter: ["data-itakuenhanced-processed"],
          // characterData: true,
          attributes: true,
          childList: true,
          subtree: true,
        });
      }

      /* Try to run a replacement, see what happens. */
      extendNotifications(notificationsSubContainer);
    }

  });

  /* Wait for the entire app to load before we start attaching
   * scripts to things. */
  const appObserver = new MutationObserver(() => {
    popup = document.querySelector('.cdk-overlay-container');
    if (!popup) { return; }

    parentObserver.observe(popup, { childList: true });
    appObserver.disconnect();
  });

  connectPort();
  appObserver.observe(document.body, { childList: true });
})();
