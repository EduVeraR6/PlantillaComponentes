window.getBoundingClientRect = function (element) {
    if (element) {
        const rect = element.getBoundingClientRect();
        return {
            x: rect.x,
            y: rect.y,
            width: rect.width,
            height: rect.height,
            top: rect.top + window.scrollY, // Include scroll position
            right: rect.right,
            bottom: rect.bottom + window.scrollY, // Include scroll position
            left: rect.left
        };
    }
    return null;
};

// Event emitter for table refresh
let eventEmitters = {};

window.registerTableRefreshEvent = function (dotNetRef, eventName) {
    if (!eventEmitters[eventName]) {
        eventEmitters[eventName] = [];
    }

    // Add DotNet reference to the event
    eventEmitters[eventName].push(dotNetRef);

    console.log(`Registered event handler for '${eventName}'`);
};

window.emitTableEvent = function (eventName) {
    if (eventEmitters[eventName]) {
        eventEmitters[eventName].forEach(dotNetRef => {
            dotNetRef.invokeMethodAsync('RefreshTable')
                .catch(error => console.error(`Error invoking RefreshTable: ${error}`));
        });
        console.log(`Emitted event '${eventName}'`);
        return true;
    }
    console.log(`No handlers registered for event '${eventName}'`);
    return false;
};