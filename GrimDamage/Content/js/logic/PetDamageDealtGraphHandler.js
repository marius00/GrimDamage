

class PetDamageDealtGraphHandler {

    constructor(database, graph) {
        this.database = database;
        this.graph = graph;
        this.lastRunTime = new Date().getTime();
        this.knownDamageTypes = {
            'Total': true
        };

        let existingSeries = graph.series.map(x => x.name);
        for (let idx = 0; idx < existingSeries.length; idx++) {
            this.knownDamageTypes[existingSeries[idx]] = true;
        }
    }


    setGraph(graph) {
        /// <summary>Set the graph element, typically after having remade it with a new theme</summary>
        this.graph = graph;
    }

    addPoint(type, amount) {
        const chart = this.graph.series.filter(s => s.name === type)[0];
        chart.addPoint(amount, false, true);
    }


    update() {
        const damage = this.database.getPetDamage(this.lastRunTime, 888888888888888888888);
        const aggregated = Enumerable.From(damage)
            .GroupBy(
                (m) => m.damageType,
                null,
                (e, g) => { return { damageType: e, amount: Enumerable.From(g.source).Sum(p => p.amount) }; }
            ).ToArray();


        if (damage.length > 0) {
            this.lastRunTime = Enumerable.From(damage).Max(x => x.timestamp);
        }

        let total = 0;
        const damageTypesThisTurn = {
            'Total': true
        };
        for (let i = 0; i < aggregated.length; i++) {
            const elem = aggregated[i];
            this.addPoint(elem.damageType, elem.amount);

            total += elem.amount;

            damageTypesThisTurn[elem.damageType] = true;
            this.knownDamageTypes[elem.damageType] = true;
        }

        // Fill in zeroes for missing types
        for (let type in this.knownDamageTypes) {
            if (!damageTypesThisTurn.hasOwnProperty(type) && this.knownDamageTypes.hasOwnProperty(type)) {
                this.addPoint(type, 0, 0);
            }
        }


        this.addPoint('Total', total);

        // Redraw
        this.graph.redraw();
    }
    

}