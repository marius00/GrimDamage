
function _itemsReceived() {
    itemsReceived(
        JSON.parse(data.playersJson),
        JSON.parse(data.damageDealtJson),
        JSON.parse(data.damageTakenJson));
}

function itemsReceived(players, damageDealt, damageTaken) {
    console.log(players, damageDealt, damageTaken);
}

var intervalID = window.setInterval(data.requestUpdate, 1000);