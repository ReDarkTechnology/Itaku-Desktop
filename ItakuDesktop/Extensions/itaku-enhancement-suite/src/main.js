/* ------------------------------------------------------ */
/* --------------- CACHES/SETTINGS/CONSTANTS ------------ */
/* ------------------------------------------------------ */
/* The Itaku Extension Suite loads its background scripts using
 * persist: false. That means this script will regularly be discarded
 * and re-loaded; the data here is temporary and will often need to be
 * re-fetched from extension settings/front-end. */

const settings = {
  positive_regexes: [],
  negative_regexes: [],
  hide_follower_counts: true,
  expanded_notifications: true,
};

const user = { /* Stored in extension sessionStorage (this is pretty fragile, but probably fine) */
  id: null,
  username: null,
};

/* Stored purely locally, used to bootstrap sessionStorage for new tabs. */
const syncs = {
  contentCache: {},
}

/* Feed URLs that need to be checked for warnings. Add additional entry points here.
 * If the structure varries from typical response objects, add additional handling
 * code in "handleContentWarnings"
 *
 * TODO check for post previews when editing tags, they're likely not handled.
 * TODO check for animations, they might use a different key than "gallery_images" */
const CONTENT_FEEDS = {
  urls: [
    'https://itaku.ee/api/feed/?*', /* Both home feed and profile feed */
    'https://itaku.ee/api/posts/?*', /* Home "posts" tab */
    'https://itaku.ee/api/posts/*/', /* Posts with multiple images embedded within */
    'https://itaku.ee/api/galleries/images/?*', /* Home "images" tab and profile galleries */
    'https://itaku.ee/api/user_profiles/*/latest_content/', /* Recently starred/uploaded */
    'https://itaku.ee/api/galleries/images/user_starred_imgs/?*', /* starred images on profiles */

    /* These links are used more for direct fetch caching, rather than content warnings */
    'https://itaku.ee/api/*/comments/?*', /* Comment fetch */
    'https://itaku.ee/api/galleries/images/*/',
  ],
  types: ['xmlhttprequest']
};

/* Used to determine which user account we should be caching content for. Stored
 * only in sessionStorage to avoid any possible data leaks. */
const USER_CALLS = {
  urls: ['https://itaku.ee/api/auth/user/'],
  types: ['xmlhttprequest']
};

const NOTIFICATION_CALLS = {
  urls: ['https://itaku.ee/api/notifications/?*'],
  types: ['xmlhttprequest']
};

/* Persist settings and cache (TODO we should watch out for race conditions with "load" here) */
async function save () {
  browser.storage.sync.set(settings);
  sessionStorage.setItem('ItakuEnhancedUserMeta', JSON.stringify(user));
}

async function load () {
  const storageSettings = await browser.storage.sync.get();
  Object.assign(settings, storageSettings);

  settings.positive_regexes = settings.positive_regexes || [];
  settings.negative_regexes = settings.negative_regexes || [];
  const userObj = JSON.parse(sessionStorage.getItem('ItakuEnhancedUserMeta')) || {};
  user.id = userObj.id;
  user.username = userObj.username;
}

/* ------------------------------------------------------ */
/* --------------------- ENTRY LOGIC -------------------- */
/* ------------------------------------------------------ */
/* How the extension is wired up and initialized, request handlers attached, and
 * errors handled. */

/* Events need to be top-level to work with non-persistant background scriptsa.
 * This complicates logic somewhat, but what can you do? */
let onMessageHandler;
browser.runtime.onMessage.addListener((evt, sender, response) => {
  if (onMessageHandler) { return onMessageHandler(evt, sender, response); }
});

let contentRequestMiddleware = loadBlockingHandler;
browser.webRequest.onBeforeRequest.addListener((details) => {
  if (contentRequestMiddleware) { return contentRequestMiddleware(details); }
  return {};
}, CONTENT_FEEDS, ['blocking']);

async function init () {
  try {
    await load();

    /* Receive communication from the front-end (mostly used for setting/fetching settings) */
    onMessageHandler = (evt, sender, response) => {
      switch (evt.type) {
      case 'set_positive_regexes':
        settings.positive_regexes = evt.content;
        save();
        break;
      case 'set_negative_regexes':
        settings.negative_regexes = evt.content;
        save();
        break;
      case 'get_settings':
        response({ type: 'response', content: settings });
        break;
      case 'get_user':
        response({ type: 'response', content: user });
        break;
      case 'sync':

        /* Note that this still doesn't guarantee that session data will be
         * shared between every tab. But it should reduce the number of
         * occurances at least. */
        const missing =
          Object.keys(syncs.contentCache)
          .reduce((result, key) => {
            if (evt.content[key]) { return result; }
            result.push(syncs.contentCache[key]);
            return result;
          }, []);

        syncs.contentCache = { ...syncs.contentCache, ...evt.content };
        response({ type: 'response', content: missing });
        break;
      }
    };

  } catch (err) { /* Abandon setup */
    console.log('HEY!!! Report this to foxyoreos on Itaku or on https://codeberg.org/foxyoreos/itaku-enhancement-suite >:3');
    console.log('If (and only if) you feel comfortable sharing your CW regexes, they may be helpful to include.');
    console.log('Load failed with error: ', err);
    return Promise.reject(err);
  }
}

/* We want to wait for initialization to finish before responding to any
 * requests. However, we also want to fail gracefully if initialization blows up
 * and the extension can't load. The solution is a blocking handler that removes
 * itself regardless of whether or not initialization happened correctly and
 * attaches the correct handler/call if everything went OK. */
const loading = init();
let attached = false;
function loadBlockingHandler (details) {
  console.log('Blocking handler started');

  return loading
    .then(() => {

      /* In the rare case that two requests are fired off before init is done. */
      if (attached) { return handleContentObject(details); }
      contentRequestMiddleware = handleContentObject;
      attached = true;

      console.log('Attached middleware and removed blocking handler.');
      return handleContentObject(details);

    }).then(
      (result) => result, /* Everything went fine */
      () => { /* Oops, somewhere along the line we errored out, sowwy ;w; */

        console.log('Initial load failed, detaching backend scripts.');
        console.log('You may notice errors since I haven\'t added error handling to the UI-side changes yet.');
        contentRequestMiddleware = null;
        return {};
      });
}

/* Messaging with notification component */
let ports = [];
browser.runtime.onConnect.addListener((port) => {
  ports.push(port);
  port.onDisconnect.addListener(()=>{
    const index = ports.indexOf(port);
    ports.splice(index, 1);
  });
});

/* And finally, the last of the handler setup for user fetch. Nothing fancy needs to happen here. */
browser.webRequest.onBeforeRequest.addListener((details) => {
  /* Little bit frustrating that we can't just wait for the request to finish
   * and look at the body. This method should likely be simplified a bit. */
  let filter = browser.webRequest.filterResponseData(details.requestId);
  let decoder = new TextDecoder('utf-8');
  let str = '';
  filter.ondata = (e) => {
    str += decoder.decode(e.data, { stream: true });
    filter.write(e.data);
  }

  filter.onstop = (e) => {
    const json = JSON.parse(str);
    (() => { /* TODO isn't there like a ?. operator now or something? */
      if (!json.profile || !json.profile.owner) { return null; }
      user.username = json.profile.owner_username;
      user.id = json.profile.owner;
    })();

    sessionStorage.setItem('ItakuEnhancedUserMeta', JSON.stringify(user));
    console.log('Caching for user: ', user);
    filter.close();
  }
}, USER_CALLS, ['blocking']);

/* Quick bug fix and prep to get rid of the user account fetch possibly... if a
 * notification request comes in, it should have the user attached to it. */
browser.webRequest.onBeforeRequest.addListener((details) => {
  let filter = browser.webRequest.filterResponseData(details.requestId);
  let decoder = new TextDecoder('utf-8');
  let str = '';
  filter.ondata = (e) => {
    str += decoder.decode(e.data, { stream: true });
    filter.write(e.data);
  }

  filter.onstop = (e) => {
    try {
      const json = JSON.parse(str);
      (() => { /* TODO isn't there like a ?. operator now or something? */
        if (!json || !json.results || !json.results.length) { return; }
        user.id = json.results[0].owner;
        sessionStorage.setItem('ItakuEnhancedUserMeta', JSON.stringify(user));
        console.log('Re-set user ID: ', user.id);
      })();
    } catch (err) { console.log(err); /* We always want to return the request no matter what */
    } finally { filter.close(); }
  }
}, NOTIFICATION_CALLS, ['blocking']);

/* ------------------------------------------------------ */
/* ------------------ EXTENSION LOGIC ------------------- */
/* ------------------------------------------------------ */
/* The actual code that's doing stuff. */

/* Filter code for content warnings. No matches should leave the warning
 * untouched. A positive match should hide the content warning. A negative match
 * should re-show the content warning, overriding the positive match. */
function checkContentObjectWarnings (result) {
 if (!result.show_content_warning) { return; }
      const show = settings.positive_regexes.reduce((show, regex) => {
        if (regex == '') { return show; }

        try { /* Catch invalid regex */
          return show || !!result.content_warning.match(regex);
        } catch (err) {
          return show;
        }
      }, false);

      const rehide = settings.negative_regexes.reduce((hide, regex) => {
        if (regex == '') { return hide; }

        try {
          return hide || !!result.content_warning.match(regex);
        } catch (err) {
          return hide;
        }
      }, false);

  if (show && !rehide) { result.show_content_warning = false; }
}

/* Cache updates. We only cache content that belongs to the user since that's
 * the only content that will be referenced in the notifications drawer. */
function cacheContentObject(result) {
  if (!user.id) { return; }
  if (user.id !== result.owner) { return; }

  /* Handle both comments and images/posts */
  const description = result.description || result.content || '';
  const truncatedDescription = description.slice(0, 50) +
        (description.length > 50 ? '...' : '');

  /* We don't check to see if a title/description exists
   * because even if they don't, we still want to mark that
   * content as fetched so the front-end doesn't try to re-fetch
   * it a second time. */
  syncs.contentCache[result.id] = {
    loading: false,
    id: result.id,
    title: result.title,
    description: truncatedDescription,
  };

  ports.forEach((port) => {
    port.postMessage({
      type: 'cache',
      content: [syncs.contentCache[result.id]],
    });
  });
}

/* Different object types on Itaku are embedded within each other. It's often
 * necessary to repeatedly recurse into objects to get at everything that can
 * have a content warning attached. */
function recurseContentObject (result) {
  checkContentObjectWarnings(result);
  cacheContentObject(result);

  if (result.gallery_images) {
    result.gallery_images.forEach(
      (image) => {
        checkContentObjectWarnings(image);
        cacheContentObject(image);
      });
  }

  /* Handle comment requests (note that we don't need to check these for content
   * warnings though, only to cache) */
  if (result.children) {
    result.children.forEach(
      (child) => {
        cacheContentObject(child);
      });
  }

  /* Handle reposts, which are treated as their own objects */
  if (result.content_object) {
    recurseContentObject(result.content_object);
  }
}

/* Entry point for most "backend" logic:
 * - check content warnings and modify requests to deal with them.
 * - save content name/IDs in cache for use with notifications.
 * - moar stuff in the future too :3
 *
 * This is all mostly handled by calling into other functions (see above) */
function handleContentObject (details) {
  let filter = browser.webRequest.filterResponseData(details.requestId);
  let decoder = new TextDecoder('utf-8');
  let encoder = new TextEncoder();

  let str = '';
  filter.ondata = (e) => {
    str += decoder.decode(e.data, { stream: true });
  };

  filter.onstop = (e) => {
    try { /* TODO should also try to check the status here? */
      const json = JSON.parse(str);

      /* Most feeds are structured around "results", but we also check recently
       * starred/uploaded lists, which use different keys. For right now, we ignore
       * "latest_active_commissions" because it's not clear content warnings will be
       * applicable to them. */
      const results = json.results || [];
      const latest_gallery_images = json.latest_gallery_images || [];
      const recently_liked_images = json.recently_liked_images || [];
      const embedded_images = json.gallery_images || [];
      const pinned_item = json.pinned_item || null;

      /* TODO there could be better checks here to see which are applicable. */
      recurseContentObject(json); /* Check itself (for direct posts) */
      if (pinned_item) { recurseContentObject(pinned_item); }
      results.forEach((obj) => recurseContentObject(obj));
      latest_gallery_images.forEach((obj) => recurseContentObject(obj));
      recently_liked_images.forEach((obj) => recurseContentObject(obj));
      embedded_images.forEach((obj) => recurseContentObject(obj));

      filter.write(encoder.encode(JSON.stringify(json)));

    } catch (err) {
      filter.write(encoder.encode(str));
    }

    filter.close();
  }

  return {};
}
