// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageParser {
    constructor(damageTakenGraph, damageDealtGraph, damageTakenPie) {
        this.lastPlayerLocation = '';
        this.currentXAxis = 100;
        this.previousDamageTaken = {};
        this.damageDealtGraph = damageDealtGraph;
        this.damageTakenGraph = damageTakenGraph;
        this.damageTakenPie = damageTakenPie;
        this.players = [];
        this.totalDamageTaken = [];
    }

    tick(players, damageDealt, damageTaken, damageDealtSingleTarget, playerLocationName) {
        this.players = players; // Just a convinience so we don't need to call getMainPlayerId(players)

        this.currentXAxis++;
        this.dataReceived(players, damageDealt, damageTaken, damageDealtSingleTarget);

        if (this.lastPlayerLocation !== playerLocationName && playerLocationName !== undefined && playerLocationName !== 'Unknown') {
            this.updatePlayerLocation(playerLocationName);
        }
    }

    get mainPlayerId() {
        return this.players.filter(p => p.isPrimary).map(p => p.id)[0] || this.players.map(p => p.id)[0];
    }


    addDamageDealt(id, damageDealt, damageDealtSingleTarget) {
        if (damageDealt[id]) {
            const total = damageDealt[id].filter(s => s.damageType === 'Total')[0];
            const totalSingle = damageDealtSingleTarget[id].filter(s => s.damageType === 'Total')[0];
            if (total) {
                //console.log("Adding ", damageDealt[id][i].amount, ' to ', damageDealt[id][i].damageType);
                // TODO: Add series if it doesn't exist, that would resolve the issue with having damage types stored 2 places (js and c#)
                // TODO: This is critical, new damage types are being discovered
                // TODO: if it does not exist, it needs to be added!

                const dmg = total.amount;
                this.damageDealtGraph.series.filter(s => s.name === 'Total')[0].addPoint(dmg,
                    totalSingle === undefined,
                    true);
            }

            if (totalSingle) {
                const dmg = totalSingle.amount;
                this.damageDealtGraph.series.filter(s => s.name === 'Single Target')[0].addPoint(dmg, true, true);
            }
        }
    }

    addDamageTaken(elem, shouldRender) {
        const dmg = elem.amount;
        const type = elem.damageType;

        /* Pie chart of total damage recieved */
        if (!isNaN(dmg) && type != "Total" && dmg > 0) {
            if (!this.totalDamageTaken.hasOwnProperty(type)) {
                this.totalDamageTaken[type] = 0;
            }
            this.totalDamageTaken[type] += Math.round(dmg);
            var temp = this.totalDamageTaken; //this.totalDamageTaken[data] wouldn't work two lines down
            var out = Object.keys(this.totalDamageTaken).map(function (data) {
                return [data, temp[data]];
            });
            this.damageTakenPie.series[0].setData(out);
        }
        /* Pie chart end */

        const chart = this.damageTakenGraph.series.filter(s => s.name === type)[0];
        if (dmg > 2) {
            chart.addPoint(dmg, shouldRender, true);
        }
        else {
            // If this is a consecutive 0, cut out the damage type sequence
            if (this.previousDamageTaken[type] !== undefined && this.previousDamageTaken[type] <= 0) {
                chart.addPoint(null, shouldRender, true);
            }
            // If this is the first zero, draw it, so the line goes back down to 0
            else {
                chart.addPoint(dmg, shouldRender, true);
            }
        }

        this.previousDamageTaken[type] = dmg;
    }


    dataReceived(players, damageDealt, damageTaken, damageDealtSingleTarget) {
        const playerId = this.mainPlayerId;
        if (playerId && damageTaken[playerId]) {
            this.addDamageDealt(playerId, damageDealt, damageDealtSingleTarget);

            for (let i = 0; i < damageTaken[playerId].length; i++) {
                const elem = damageTaken[playerId][i];
                const shouldRender = i === damageTaken[playerId].length - 1;
                this.addDamageTaken(elem, shouldRender);
            }
        }
        else {
            console.log('No player yet.. skipping graph..');
        }
    }


    updatePlayerLocation(playerLocationName) {
        console.log('updating player position to ', playerLocationName);
        this.lastPlayerLocation = playerLocationName;
        this.damageDealtGraph.series.filter(s => s.name === 'EventLine')[0].addPoint({
            text: this.lastPlayerLocation,
            title: this.lastPlayerLocation,
            x: this.currentXAxis
        });
    }
}