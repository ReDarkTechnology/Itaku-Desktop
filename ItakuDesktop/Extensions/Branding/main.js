function waitForElement(parent, selector, callback, runOnce = true) {
    const elementNow = parent.querySelector(selector);
    if (elementNow) {
        callback(elementNow);
        if (runOnce) {
            return;
        }
    }
    const observer = new MutationObserver((records) => {
        records.forEach((record) => {
            record.addedNodes.forEach((parentElement) => {
                if (parentElement instanceof Element) {
                    parentElement.querySelectorAll(selector).forEach((element) => {
                        if (runOnce) {
                            observer.disconnect();
                        }
                        callback(element);
                    });
                }
            });
        });
    });
    observer.observe(parent, {
        childList: true,
        subtree: true,
    });
}

waitForElement(document, "mat-icon[svgicon='itaku']", (element) => {
    var elem = document.createElement("img");
    elem.setAttribute("src", "https://cdn.discordapp.com/attachments/1147690247736541194/1147783421079335062/itaku-desktop.png");
    elem.setAttribute("height", "65px");
    elem.setAttribute("width", "130px");
    elem.setAttribute("alt", "ItakuLogo");
    element.parentElement.appendChild(elem);
    element.remove();
}, false);

// This one is to remove the patreon button
/*waitForElement(document, "a[class='mat-focus-indicator header-remove-ads-btn invent-color-text mat-stroked-button mat-button-base ng-star-inserted']", (element) => {
    element.remove();
}, false);*/
