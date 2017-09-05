
function _itemsReceived() {
    itemsReceived(
        JSON.parse(data.playersJson),
        JSON.parse(data.damageDealtJson),
        JSON.parse(data.damageTakenJson));
}

// TODO: move this to main.js
var n = 0;
function itemsReceived(players, damageDealt, damageTaken) {
    if (players.length == 0) {
        console.log("No player yet.. skipping graph..");
    } else {
        var id = players[0].id;
        for (var i = 0; i < damageDealt[id].length; i++) {
            //console.log("Adding ", damageDealt[id][i].amount, ' to ', damageDealt[id][i].damageType);
            // TODO: Add series if it doesn't exist, that would resolve the issue with having damage types stored 2 places (js and c#)
            globalChart.series.filter(s => s.name === damageDealt[id][i].damageType)[0].addPoint(damageDealt[id][i].amount, i === damageDealt[id].length-1, true);
        }
        
    }

    
}

var intervalID = window.setInterval(data.requestUpdate, 1000);