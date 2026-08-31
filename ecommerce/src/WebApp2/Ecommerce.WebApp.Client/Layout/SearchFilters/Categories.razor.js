export function offsetWidth(el) {
    return el.offsetWidth;
}

export function computeVisible(containerRef, viewAllRef, measureRef) {
    var containerXOffset = containerRef.getBoundingClientRect().x;
    var containerWidth = containerRef.offsetWidth;
    var viewAllWidth = viewAllRef.offsetWidth;
    console.log(viewAllWidth);
    var availableWidth = containerWidth - viewAllWidth;
    var maxX = containerXOffset + availableWidth;

    const items = Array.from(measureRef.children).slice(0, -1);
    let visibleCount = 0;
    console.log(`Available width: ${availableWidth}`);

    for (const item of items) {
        if (item.getBoundingClientRect().x + item.offsetWidth <= maxX) {
            visibleCount++;
        } else {
            break;
        }
    }
    return visibleCount;
}

let observer;

export function startObserving(containerRef, viewAllRef, measureRef, dotNetRef) {
    if (observer) {
        observer.disconnect();
        observer = null;
    }
    observer = new ResizeObserver(() => {
        const visibleCount = computeVisible(containerRef, viewAllRef, measureRef);
        dotNetRef.invokeMethodAsync("SetVisibleCount", visibleCount);
    });
    observer.observe(containerRef);
}

export function stopObserving() {
    if (observer) {
        observer.disconnect();
        observer = null;
    }
}