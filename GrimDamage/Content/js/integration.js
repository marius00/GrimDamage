
function _itemsReceived() {
    itemsReceived(
        JSON.parse(data.playersJson),
        JSON.parse(data.damageDealtJson),
        JSON.parse(data.damageTakenJson),
        JSON.parse(data.damageDealtToSingleTargetJson)
    );
}

// TODO: move this to main.jspreviousDamageTaken
var n = 0;
let previousDamageTaken = {};
function itemsReceived(players, damageDealt, damageTaken, damageDealtSingleTarget) {
    if (players.length == 0) {
        console.log("No player yet.. skipping graph..");
    } else {
        var id = players[0].id;
        //console.log("Adding ", damageDealt, damageTaken, players);

        var total = damageDealt[id].filter(s => s.damageType === 'Total')[0];
        var totalSingle = damageDealtSingleTarget[id].filter(s => s.damageType === 'Total')[0];
        if (total) {
            //console.log("Adding ", damageDealt[id][i].amount, ' to ', damageDealt[id][i].damageType);
            // TODO: Add series if it doesn't exist, that would resolve the issue with having damage types stored 2 places (js and c#)

            let dmg = total.amount;
            gDamageTakenDone.series.filter(s => s.name === 'Total')[0].addPoint(dmg, totalSingle !== undefined, true);
        }

        if (totalSingle) {
            let dmg = totalSingle.amount;
            gDamageTakenDone.series.filter(s => s.name === 'Single Target')[0].addPoint(dmg, true, true);
        }
        

        for (var i = 0; i < damageTaken[id].length; i++) {
            var elem = damageTaken[id][i];
            let dmg = elem.amount;
            let shouldRender = i === damageTaken[id].length - 1;
            let type = elem.damageType;

            var chart = gDamageTakenChart.series.filter(s => s.name === type)[0];
            if (dmg > 2) {
                chart.addPoint(dmg, shouldRender, true);
            } 
            else {
                // If this is a consecutive 0, cut out the damage type sequence
                if (previousDamageTaken[type] !== undefined && previousDamageTaken[type] <= 0) {
                    chart.addPoint(null, shouldRender, true);
                    console.log('=>null dmg is ', dmg, 'and last done', previousDamageTaken[type]);
                }
                // If this is the first zero, draw it, so the line goes back down to 0
                else {
                    chart.addPoint(dmg, shouldRender, true);
                    console.log(previousDamageTaken[type], previousDamageTaken[type] <= 0);
                    console.log('=>0 dmg is ', dmg, 'and last done', previousDamageTaken[type]);
                }
            }
            previousDamageTaken[type] = dmg;
        }
        
    }

    
}

var intervalID = window.setInterval(data.requestUpdate, 1000);