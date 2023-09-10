// Made by Bunzhida for Itaku Desktop, also works as a user script!
/** @type {string} */
const overlaySelector = "div[class='cdk-overlay-pane image-dialog']";

/** @type {Element} */
var cardButton = null;

/**
 * Wait for a specific element in a parent
 * @param {Element} parent 
 * @param {string} selector
 * @param {function} callback
 * @param {boolean} runOnce
 */
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

/**
 * Makes the mat card bigger to view the image better
 * @param {Element} element - The first argument to this function
 */
function modifyAndResizeMatCard(element)
{
    element.style.width = "100%";
    element.style.height = "100%";
    element.style.maxWidth = "100%";
    element.style.maxHeight = "100%";

    const container = element.children[1];
    container.style.width = "100%";
    container.style.height = "100%";
    container.style.maxWidth = "100%";
    container.style.maxHeight = "100%";

    const frameContainer = container.children[0].children[1];
    frameContainer.setAttribute("class", null);

    const centerContainer = frameContainer.children[0].children[0];
    centerContainer.setAttribute("class", "center");

    cardButton = centerContainer.children[0].children[1].children[0].children[3].children[0];

    const imageContainer = centerContainer.children[0].children[0].children[0];
    // Make it so that the cursor changes to hand when hovering the image
    imageContainer.parentNode.style.cursor = "pointer";
    imageContainer.parentNode.addEventListener("click", onHrefClick);
    imageContainer.setAttribute("class", "mat-card-image ng-star-inserted");
    imageContainer.setAttribute("style", "margin-top:0.5vh; max-width: 100%; max-height: 105vh; height: auto;");
}

function onHrefClick()
{
    open(cardButton.getAttribute("href"), "_blank");
}

// Let's start waiting -w-
waitForElement(document, overlaySelector, modifyAndResizeMatCard, false);