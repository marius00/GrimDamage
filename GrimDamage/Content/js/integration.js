let saveReceivedCallbackMethod = undefined;
let intervalID = undefined;

const TimestampEverything = '2527282800000';

/// <field name='TYPE_STATES' type='Number'>The event type is states, such as death and pause game.</field>  
const TYPE_STATES = 1;
const TYPE_DETAILED_DAMAGE_TAKEN = 2;
const TYPE_DETAILED_DAMAGE_DEALT = 3;
const TYPE_SIMPLE_DAMAGE_TAKEN = 4;
const TYPE_SIMPLE_DAMAGE_DEALT = 5;
const TYPE_HEALTH_CHECK = 6;
const TYPE_FETCH_RESISTS_CHECK = 7;
const TYPE_FETCH_ENTITIES = 8;
const TYPE_FETCH_LOCATIONS = 9;
const TYPE_FETCH_SIMPLE_PET_DAMAGE = 10;



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


function requestUpdates() {
    // Sending timestamps as strings, as Chromium confuses them with doubles
    
}

function setCsharpLoadHistoryCallback(method) {
    saveReceivedCallbackMethod = method;
}
/*
function sendCsharpNameSuggestion(suggestion) {
    if (suggestion)
        data.suggestLocationName(suggestion);
}
*/
function setCsharpLightMode(mode) {
    data.setLightMode(mode);
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