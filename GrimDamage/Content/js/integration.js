let tickCallbackMethod = undefined;
let requestCallbackMethod = undefined;
let saveReceivedCallbackMethod = undefined;
let intervalID = undefined;
let lastStateTimestamp = 0;

/// <field name='TYPE_STATES' type='Number'>The event type is states, such as death and pause game.</field>  
const TYPE_STATES = 1;
const TYPE_DETAILED_DAMAGE_TAKEN = 2;
const TYPE_DETAILED_DAMAGE_DEALT = 3;

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

function setCsharpRequestCallback(method) {
    requestCallbackMethod = method;
}

function _notifyStateChanges(dataset) {
    if (dataset.length > 0)
        lastStateTimestamp = dataset[0].timestamp;

    if (requestCallbackMethod) {
        requestCallbackMethod(TYPE_STATES, dataset);
    }
}
function requestUpdates() {
    // Sending timestamps as strings, as Chromium confuses them with doubles
    data.requestData(TYPE_STATES, lastStateTimestamp.toString(), -1, '_notifyStateChanges');
    data.requestUpdate();
}
function setCsharpTickInterval(interval) {
    if (intervalID !== undefined) {
        clearInterval(intervalID);
    }

    intervalID = window.setInterval(requestUpdates, interval);
}

function setCsharpLoadHistoryCallback(method) {
    saveReceivedCallbackMethod = method;
}

function sendCsharpNameSuggestion(suggestion) {
    if (suggestion)
        data.suggestLocationName(suggestion);
}

function api() {
    console.log('All data is available in the global data variable: \"data\"');
    console.log('The data variable contains the following keys:');
    for (let key in data) {
        if (key !== 'api') {
            console.log('\t', key, '=>', data[key]);
        }
    }

    console.log('\r\nThe following information is documented in C#:');
    console.log('\t', data.api);
}

console.log("The API can be requested by calling api()");
api();