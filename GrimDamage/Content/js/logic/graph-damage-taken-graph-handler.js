// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class DamageTakenGraphLogichandler {

    constructor(database, damageTakenGraph) {
        this.database = database;
        this.damageTakenGraph = damageTakenGraph;
        this.lastRunTime = new Date().getTime();
        this.knownDamageTypes = {
            'Total': true,
            'Raw Total': true
        };

        let existingSeries = damageTakenGraph.series.map(x => x.name);
        for (let idx = 0; idx < existingSeries.length; idx++) {
            this.knownDamageTypes[existingSeries[idx]] = true;
        }
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


    addDamageTaken(type, amount, extrapolated) {
        //console.debug(`AddPoint(${type}, ${amount}, ${extrapolated})`);
        const chart = this.damageTakenGraph.series.filter(s => s.name === type)[0];
        if (amount > 2) {
            chart.addPoint({ y: amount, extrapolated: extrapolated }, false, true);
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


            if (damageTaken.length > 0) {
                this.lastRunTime = Enumerable.From(damageTaken).Max(x => x.timestamp);
            }

            let total = 0;
            let totalExtrapolated = 0;
            let damageTypesThisTurn = {
                'Total': true,
                'Raw Total': true
            };
            for (let i = 0; i < aggregated.length; i++) {
                const elem = aggregated[i];
                let extrapolated = elem.amount;

                // Perform extrapolation if the damage is sufficient to warrant a search
                if (elem.amount > 10) {
                    const resist = Math.min(80, this.database.getResists(elem.damageType, 888888888888888888888));
                    extrapolated = this.extrapolateForResists(elem.amount, resist);
                }

                this.addDamageTaken(elem.damageType, elem.amount, extrapolated);

                total += elem.amount;
                totalExtrapolated += extrapolated;

                damageTypesThisTurn[elem.damageType] = true;
                this.knownDamageTypes[elem.damageType] = true;
            }

            // Fill in zeroes for missing types
            for (let type in this.knownDamageTypes) {
                if (!damageTypesThisTurn.hasOwnProperty(type) && this.knownDamageTypes.hasOwnProperty(type)) {
                    this.addDamageTaken(type, 0, 0);
                }
            }


            this.addDamageTaken('Total', total, totalExtrapolated);
            //this.addDamageTaken('Raw Total', 0, totalExtrapolated);

            // Redraw
            this.damageTakenGraph.redraw();
        }
    }

}