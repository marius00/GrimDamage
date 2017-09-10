let callbackMethod = undefined;

// This function is called from C# when a stat update is received
function _itemsReceived() {
    if (callbackMethod) {
        callbackMethod(
            JSON.parse(data.playersJson),
            JSON.parse(data.damageDealtJson),
            JSON.parse(data.damageTakenJson),
            JSON.parse(data.damageDealtToSingleTargetJson),
            data.playerLocationName
        );
    }
}

function setCsharpTickCallback(method) {
    callbackMethod = method;
}

// This function is called from C# when a saved parse is loaded
function _saveReceived(data) {
    console.log(data); // TODO:
}


// Request damage stats every second
var intervalID = window.setInterval(data.requestUpdate, 1000);