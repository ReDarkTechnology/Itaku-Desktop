(function () {
  /* TODO I'm not a fan of using `innerHTML` for this. I know it's safe in this case, but in general I don't want to use innerHTML at all. I should set up templating support. */
  const content_warnings_str = `<section _ngcontent-serverapp-c232="" class="p-16 ng-star-inserted ItakuEnhanced__CWRegex"><div _ngcontent-serverapp-c232="" class="m-top-20"><h2 _ngcontent-serverapp-c232="" class="mat-h2">Content Warning Keywords</h2><p _ngcontent-serverapp-c232="">Check regex filters against content warnings to determine whether they should be ignored. If none match, content warnings will be displayed as normal.</p><div _ngcontent-serverapp-c232=""><div _ngcontent-serverapp-c232="" class="inline-block"><h4 _ngcontent-serverapp-c232="" class="mat-h4 wrapper-title mat-hint">Positive filters</h4></div></div><section _ngcontent-serverapp-c232=""><div _ngcontent-serverapp-c232="" class="keywords"><div _ngcontent-serverapp-c232="" data-cy="app-tag-suggestions-btns" class="card tags-card material-radius theme-border theme-border" style="position: relative;"><textarea class="ItakuEnhanced__PositiveRegex" style=" " spellcheck="false"></textarea><div _ngcontent-serverapp-c232="" class="center-text m-top-10 ng-star-inserted" style="position: relative;"><span _ngcontent-serverapp-c232="" class="mat-small outline-text">Ignore content warnings if any of these regex match (one per line)</span></div><!----><!----></div></div></section><div _ngcontent-serverapp-c232=""><div _ngcontent-serverapp-c232="" class="inline-block"><h4 _ngcontent-serverapp-c232="" class="mat-h4 wrapper-title mat-hint">Negative filters</h4></div></div><section _ngcontent-serverapp-c232=""><div _ngcontent-serverapp-c232="" class="keywords"><div _ngcontent-serverapp-c232="" data-cy="app-tag-suggestions-btns" class="card tags-card material-radius theme-border theme-border" style="position: relative;"><textarea class="ItakuEnhanced__NegativeRegex" style=" " spellcheck="false"></textarea><div _ngcontent-serverapp-c232="" class="center-text m-top-10 ng-star-inserted" style="position: relative;"><span _ngcontent-serverapp-c232="" class="mat-small outline-text">Re-show content warnings if any of these regex match (one per line)</span></div><!----><!----></div></div></section></div></section>`
  const content_warnings_container = document.createElement('div');
  content_warnings_container.innerHTML = content_warnings_str;
  const content_warnings = content_warnings_container.firstChild;

  /* Wire up filling in the fields, fetching the fields, etc...
   * If the extension gets more complicated, eventually we'll want
   * data binding support. But don't overcomplicate things early. */
  let settings = {
    positive_regexes: [],
    negative_regexes: [],
  };

  content_warnings
    .querySelector('.ItakuEnhanced__PositiveRegex')
    .addEventListener('input', (e) => {
      browser.runtime.sendMessage({
        type: 'set_positive_regexes',
        content: e.target.value.split('\n'),
      });
    }, false);
  content_warnings
    .querySelector('.ItakuEnhanced__NegativeRegex')
    .addEventListener('input', (e) => {
      browser.runtime.sendMessage({
        type: 'set_negative_regexes',
        content: e.target.value.split('\n'),
      });
    }, false);

  async function fill () {
    const response = await browser.runtime.sendMessage({
      type: 'get_settings'
    });

    settings = response.content;
    content_warnings
      .querySelector('.ItakuEnhanced__PositiveRegex')
      .value = settings.positive_regexes.join('\n');

    content_warnings
      .querySelector('.ItakuEnhanced__NegativeRegex')
      .value = settings.negative_regexes.join('\n');
  };

  /* Standard observer setup. Whenever we move to the content settings
   * page, check to see if our stuff is attached. If it's not attached,
   * then attach it. */

  /* We want to determine if we are A) in the settings pages, and B) in the correct section.
   * For efficiency's sake we use a single MutationObserver for both. Finally, if we are in
   * the correct location, we check to see if we need to insert the HTML. Because we keep
   * the same reference to the elements, we don't need to re-wire event handlers. */
  let page, observedSettings, observedBlockSettingsTab;
  const parentObserver = new MutationObserver(attachSettingsAndObservers);
  function attachSettingsAndObservers () {
    const settings = page.querySelector('.settings-wrapper');
    const blockSettingsTab = page.querySelector('app-blacklist-settings');

    /* If we're not in the settings area, there's nothing really for us to do.
     * unobserve/return. */
    if (!settings) {
      parentObserver.disconnect();
      parentObserver.observe(page, { childList: true });
      observedBlockSettingsTab = null;
      observedSettings = null;
      return;
    }

    /* Otherwise, make sure we're properly wired up */
    if (observedSettings !== settings) {
      observedSettings = settings;
      parentObserver.observe(observedSettings, { childList: true });
    }

    /* Same deal for the blocklist child area (yes, it's annoying there is no "unobserve") */
    if (!blockSettingsTab) {
      parentObserver.disconnect();
      parentObserver.observe(page, { childList: true });
      parentObserver.observe(observedSettings, { childList: true });
      observedBlockSettingsTab = null;
      return;
    }

    if (observedBlockSettingsTab !== blockSettingsTab) {
      observedBlockSettingsTab = blockSettingsTab;
      parentObserver.observe(observedBlockSettingsTab, { childList: true });
    }

    /* And check to see if we need to care about inserting the HTML. */
    const injectedSettings = document.querySelector('.ItakuEnhanced__CWRegex');
    if (!injectedSettings) {
      blockSettingsTab.querySelector('.slider-settings-wrapper').appendChild(content_warnings);
      fill();
    }
  }

  /* Wait for the entire app to load before we start attaching
   * scripts to things. */
  const appObserver = new MutationObserver(() => {
    page = document.querySelector('mat-sidenav-content');
    if (!page) { return; }

    parentObserver.observe(page, { childList: true });
    appObserver.disconnect();

    /* Try to wire up the rest of the page, if able */
    attachSettingsAndObservers();
  });
  appObserver.observe(document.body, { childList: true });
})();
