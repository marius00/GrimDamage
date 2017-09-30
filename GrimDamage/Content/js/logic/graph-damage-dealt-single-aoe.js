// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageDealtGraphLogicHandler {
    constructor(database, damageDealtGraph) {
        /// <summary>Class responsible for updating the "damage dealt" simple-graph</summary>
        this.damageDealtGraph = damageDealtGraph;
        this.lastRunTime = new Date().getTime();
        this.database = database;
    }

    setDamageDealtGraph(graph) {
        /// <summary>Set the graph element, typically after having remade it with a new theme</summary>
        this.damageDealtGraph = graph;
    }

    getSingleTargetDamage(detailedDamageEntries) {
        let candidates = {};
        for (let idx = 0; idx < detailedDamageEntries.length; idx++) {
            const entry = detailedDamageEntries[idx];
            if (!candidates[entry.victimId]) {
                candidates[entry.victimId] = 0;
            }

            candidates[entry.victimId] += entry.amount;
        }

        let best = 0;
        for (let key in candidates) {
            if (candidates.hasOwnProperty(key)) {
                if (candidates[key] > best) {
                    best = candidates[key];
                }
            }
        }

        return Math.round(best);
    }

    getTotalDamage(detailedDamageEntries) {
        let total = 0;
        for (let idx = 0; idx < detailedDamageEntries.length; idx++) {
            const entry = detailedDamageEntries[idx];
            total += entry.amount;
        }
        
        return Math.round(total);
    }

    getMaxTimestamp(detailedDamageEntries) {
        let m = this.lastRunTime;
        for (let idx = 0; idx < detailedDamageEntries.length; idx++) {
            const entry = detailedDamageEntries[idx];
            if (entry.timestamp > m)
                m = entry.timestamp;
        }

        return m;
        
    }

    update() {
        const playerId = this.database.getMainPlayerEntityId();
        if (playerId) {
            const damageDealt = this.database.getDamageDealt(this.lastRunTime, 888888888888888888888);
            const damageDealtSingleTarget = this.getSingleTargetDamage(damageDealt);
            const damageDealtTotal = this.getTotalDamage(damageDealt);
            console.debug('Updating damage dealt graph with',
                damageDealtSingleTarget,
                damageDealtTotal,
                'from',
                damageDealt, 'using time values', this.lastRunTime, 888888888888888888888);

            this.addDamageDealt(damageDealtTotal, damageDealtSingleTarget);
            this.lastRunTime = Math.max(this.lastRunTime, this.getMaxTimestamp(damageDealt));
        }

    }


    addDamageDealt(damageDealt, damageDealtSingleTarget) {
        //console.log("Adding ", damageDealt[id][i].amount, ' to ', damageDealt[id][i].damageType);
        // TODO: Add series if it doesn't exist, that would resolve the issue with having damage types stored 2 places (js and c#)
        // TODO: This is critical, new damage types are being discovered
        // TODO: if it does not exist, it needs to be added!

        this.damageDealtGraph.series.filter(s => s.name === 'Total')[0].addPoint(damageDealt, false, true);
        this.damageDealtGraph.series.filter(s => s.name === 'Single Target')[0].addPoint(damageDealtSingleTarget, true, true);
    }

}