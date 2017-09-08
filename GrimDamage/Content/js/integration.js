
// This function is called from C# when a stat update is received
function _itemsReceived() {
    itemsReceived(
        JSON.parse(data.playersJson),
        JSON.parse(data.damageDealtJson),
        JSON.parse(data.damageTakenJson),
        JSON.parse(data.damageDealtToSingleTargetJson)
    );
}

// This function is called from C# when a saved parse is loaded
function _saveReceived(data) {
    console.log(data);
}

// TODO: move this to main.jspreviousDamageTaken
var n = 0;
let previousDamageTaken = {};
function itemsReceived(players, damageDealt, damageTaken, damageDealtSingleTarget) {
    if (players.length == 0) {
        console.log("No player yet.. skipping graph..");
    } else {
        let id = players[0].id;
        addDamageDealt(id, damageDealt, damageDealtSingleTarget);

        for (var i = 0; i < damageTaken[id].length; i++) {
            var elem = damageTaken[id][i];
            let shouldRender = i === damageTaken[id].length - 1;
            addDamageTaken(elem, shouldRender);
        }
        
    }
}

function addDamageDealt(id, damageDealt, damageDealtSingleTarget) {
    let total = damageDealt[id].filter(s => s.damageType === 'Total')[0];
    let totalSingle = damageDealtSingleTarget[id].filter(s => s.damageType === 'Total')[0];
    if (total) {
        //console.log("Adding ", damageDealt[id][i].amount, ' to ', damageDealt[id][i].damageType);
        // TODO: Add series if it doesn't exist, that would resolve the issue with having damage types stored 2 places (js and c#)
        // TODO: This is critical, new damage types are being discovered
        // TODO: if it does not exist, it needs to be added!

        let dmg = total.amount;
        gDamageTakenDone.series.filter(s => s.name === 'Total')[0].addPoint(dmg, totalSingle !== undefined, true);
    }

    if (totalSingle) {
        let dmg = totalSingle.amount;
        gDamageTakenDone.series.filter(s => s.name === 'Single Target')[0].addPoint(dmg, true, true);
    }
}

function addDamageTaken(elem, shouldRender) {
    let dmg = elem.amount;
    let type = elem.damageType;

    var chart = gDamageTakenChart.series.filter(s => s.name === type)[0];
    if (dmg > 2) {
        chart.addPoint(dmg, shouldRender, true);
    }
    else {
        // If this is a consecutive 0, cut out the damage type sequence
        if (previousDamageTaken[type] !== undefined && previousDamageTaken[type] <= 0) {
            chart.addPoint(null, shouldRender, true);
        }
        // If this is the first zero, draw it, so the line goes back down to 0
        else {
            chart.addPoint(dmg, shouldRender, true);
        }
    }

    previousDamageTaken[type] = dmg;
}

// Request damage stats every second
var intervalID = window.setInterval(data.requestUpdate, 1000);