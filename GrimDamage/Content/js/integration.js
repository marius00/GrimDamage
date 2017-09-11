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

// This function ensures that JS errors are logged in C#, and reported as an error via the exception reporter
// So exceptions are reported when users without debug console encounters them
function enableLogToCsharp() {
    window.onerror = function (errorMsg, url, lineNumber, column, errorObj) {
        data.log(JSON.stringify([errorMsg, url + ':' + lineNumber + ' at character ' + column, errorObj]));
    }
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

function sendCsharpNameSuggestion(suggestion) {
    if (suggestion)
        data.suggestLocationName(suggestion);
}