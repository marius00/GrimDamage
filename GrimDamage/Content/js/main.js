// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageParser {
    constructor(damageTakenGraph, damageDealtGraph) {
        this.lastPlayerLocation = '';
        this.currentXAxis = 100;
        this.previousDamageTaken = {};
        this.damageDealtGraph = damageDealtGraph;
        this.damageTakenGraph = damageTakenGraph;
    }

    tick(players, damageDealt, damageTaken, damageDealtSingleTarget, playerLocationName) {
        console.log('tick');

        this.currentXAxis++;

        // IsPrimary
        this.itemsReceived(players, damageDealt, damageTaken, damageDealtSingleTarget);

        if (this.lastPlayerLocation !== playerLocationName && playerLocationName !== undefined && playerLocationName !== 'Unknown') {
            this.updatePlayerLocation(playerLocationName);
        }
    }


    addDamageDealt(id, damageDealt, damageDealtSingleTarget) {
        const total = damageDealt[id].filter(s => s.damageType === 'Total')[0];
        const totalSingle = damageDealtSingleTarget[id].filter(s => s.damageType === 'Total')[0];
        if (total) {
            //console.log("Adding ", damageDealt[id][i].amount, ' to ', damageDealt[id][i].damageType);
            // TODO: Add series if it doesn't exist, that would resolve the issue with having damage types stored 2 places (js and c#)
            // TODO: This is critical, new damage types are being discovered
            // TODO: if it does not exist, it needs to be added!

            const dmg = total.amount;
            this.damageDealtGraph.series.filter(s => s.name === 'Total')[0].addPoint(dmg, totalSingle === undefined, true);
        }

        if (totalSingle) {
            const dmg = totalSingle.amount;
            this.damageDealtGraph.series.filter(s => s.name === 'Single Target')[0].addPoint(dmg, true, true);
        }
    }

    addDamageTaken(elem, shouldRender) {
        const dmg = elem.amount;
        const type = elem.damageType;

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


    itemsReceived(players, damageDealt, damageTaken, damageDealtSingleTarget) {
        if (players.length === 0) {
            console.log('No player yet.. skipping graph..');
        } else {
            const id = players[0].id;
            this.addDamageDealt(id, damageDealt, damageDealtSingleTarget);

            for (let i = 0; i < damageTaken[id].length; i++) {
                const elem = damageTaken[id][i];
                const shouldRender = i === damageTaken[id].length - 1;
                this.addDamageTaken(elem, shouldRender);
            }

        }
    }


    updatePlayerLocation(playerLocationName) {
        console.log('updating player position to ', playerLocationName);
        this.lastPlayerLocation = playerLocationName;
        this.damageDealtGraph.series.filter(s => s.name === 'EventLine')[0].addPoint({
            text: this.lastPlayerLocation,
            title: this.lastPlayerLocation,
            x: currentXAxis
        });
    }
}