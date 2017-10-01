// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageTakenGraphLogichandler {

    constructor(database, damageTakenGraph) {
        this.database = database;
        this.damageTakenGraph = damageTakenGraph;
        this.lastRunTime = new Date().getTime();
    }


    setGraph(graph) {
        /// <summary>Set the graph element, typically after having remade it with a new theme</summary>
        this.damageTakenGraph = graph;
    }


    extrapolateForResists(damage, resists) {
        /// <summary>Extrapolate how much damage was really taken, before resists</summary>  
        /// <param name="damage">How much damage was taken</param>
        /// <param name="resists">The player resist for this damage type</param>
        return damage / ((100 - resists) / 100);
    }


    addDamageTaken(elem, ts) {
        const dmg = elem.amount;
        const type = elem.damageType;

        const chart = this.damageTakenGraph.series.filter(s => s.name === type)[0];
        if (dmg > 2) {
            const resist = this.database.getResists(type, ts);
            const extrapolated = this.extrapolateForResists(dmg, resist);

            chart.addPoint({ y: dmg, extrapolated: extrapolated }, false, true);
        } else {
            chart.addPoint({ y: 0, extrapolated: 0 }, false, true);
        }
    }


    update() {
        const playerId = this.database.getMainPlayerEntityId();
        if (playerId) {
            const damageTaken = this.database.getDamageTaken(this.lastRunTime, 888888888888888888888);
            const aggregated = Enumerable.From(damageTaken)
                .GroupBy(
                    (m) => m.damageType,
                    null,
                    (e, g) => { return { damageType: e, amount: Enumerable.From(g.source).Sum(p => p.amount) }; }
                ).ToArray();

            console.debug('Updating damage TAKEN graph with',
                aggregated,
                'from',
                damageTaken,
                'using time values',
                this.lastRunTime,
                888888888888888888888);


            for (let i = 0; i < aggregated.length; i++) {
                const elem = aggregated[i];
                this.addDamageTaken(elem, 888888888888888888888);
            }

            // Redraw
            this.damageTakenGraph.redraw();

            if (damageTaken.length > 0) {
                this.lastRunTime = Enumerable.From(damageTaken).Max(x => x.timestamp);
            }
        }
    }

}