(function () {

  let menu, user;
  function fixProfileLink () {
    /* User missing? */
    if (!user || !user.username) {
      return;
    }

    /* Already attached? Attaching too early*/
    if(!menu || menu.querySelector('a.ItakuEnhanced__profileLink')) {
      return;
    }

    const img = menu.querySelector('img.avatar');
    const link = document.createElement('a');
    link.classList.toggle('ItakuEnhanced__profileLink', true);
    link.setAttribute('href', `/profile/${user.username}`);
    link.setAttribute('onClick', 'return false;');

    img.remove();
    link.appendChild(img);
    menu.prepend(link);
  }

  async function getUser () {
    const response = await browser.runtime.sendMessage({
      type: 'get_user'
    });

    if (response) {
      user = response.content;
    }

    /* If we're too early, wait a bit and try again. */
    if (!user || !user.id || !user.username) {
      setTimeout(getUser, 250);
      return;
    }

    fixProfileLink(); /* And run the fix if this returns after we've already attached */
  };


  /* TODO: Handle signed out behavior */
  const menuObserver = new MutationObserver((list, observer) => { fixProfileLink(); });

  /* Wait for the entire app to load before we start attaching
   * scripts to things. */
  const appObserver = new MutationObserver(() => {
    menu = document.querySelector('div[data-cy="app-header-user-menu"]');
    if (!menu) { return; }

    menuObserver.observe(menu, { childList: true });
    appObserver.disconnect();

    /* In practice, these elements get attached at the same time,
     * so we should try to run a replacement immediately. */
    fixProfileLink();
  });

  getUser();
  appObserver.observe(document.body, { childList: true });
})();
