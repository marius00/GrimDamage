let tickCallbackMethod = undefined;
let saveReceivedCallbackMethod = undefined;
let intervalID = undefined;

// This function is called from C# when a stat update is received
function _itemsReceived() {
    if (tickCallbackMethod) {
        tickCallbackMethod(
            JSON.parse(data.playersJson),
            JSON.parse(data.damageDealtJson),
            JSON.parse(data.damageTakenJson),
            JSON.parse(data.damageDealtToSingleTargetJson),
            data.playerLocationName
        );
    }
}

// This function is called from C# when a saved parse is loaded
function _saveReceived(data) {
    if (saveReceivedCallbackMethod)
        saveReceivedCallbackMethod(JSON.parse(data));
}



function setCsharpTickCallback(method) {
    tickCallbackMethod = method;
}

function setCsharpTickInterval(interval) {
    if (intervalID !== undefined) {
        clearInterval(intervalID);
    }
    intervalID = window.setInterval(data.requestUpdate, interval);
}

function setCsharpLoadHistoryCallback(method) {
    saveReceivedCallbackMethod = method;
}