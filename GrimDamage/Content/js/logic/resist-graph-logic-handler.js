// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes
class PlayerResistsGraphLogichandler {

    constructor(database, graph) {
        this.database = database;
        this.graph = graph;
        this.lastRunTime = new Date().getTime();
        this.knownSeries = [];

        let existingSeries = graph.series.map(x => x.name);
        for (let idx = 0; idx < existingSeries.length; idx++) {
            this.knownSeries.push(existingSeries[idx]);
        }
    }


    setGraph(graph) {
        /// <summary>Set the graph element, typically after having remade it with a new theme</summary>
        this.graph = graph;
    }

    process(resist, start, end, dataset) {
        /// <summary>Preprocessing of raw data, fills in padded entries to make the data fit the graph better</summary>
        let points = [
            {
                y: this.database.getResists(resist, start),
                x: start
            }
        ];

        let previousTimestamp = start;
        let previousAmount = points[0].y;
        for (let idx = 0; idx < dataset.length; idx++) {
            if (Math.abs(previousTimestamp - dataset[idx].timestamp) > 1) {
                // Keep things "smooth", insert a recent timestamp for the last damage, to prevent a graph looking like the resist slowly declined.
                points.push(
                    {
                        x: dataset[idx].timestamp - 500,
                        y: previousAmount
                    }
                );
                
            }

            points.push(
                {
                    x: dataset[idx].timestamp,
                    y: dataset[idx].amount
                }
            );
        }

        // Duplicate the last point to finish the graph -- Unsure if this is needed
        points.push([
            {
                y: points[points.length - 1].y,
                x: end + 1
            }
        ]);

        return points;
    }


    update(timestamp, interval) {
        /// <param name="timestamp">The last timestamp in the graph, epoch</param>
        /// <param name="interval">Interval in seconds</param>
        const startInterval = timestamp - interval * 1000;
        const resists = this.database.getAllResists(startInterval, timestamp);
        for (let idx = 0; idx < this.knownSeries.length; idx++) {
            let resistName = this.knownSeries[idx];
            let graphPoints = this.process(resistName, startInterval, resists.filter(r => r.type === resistName));
            let series = this.graph.series.filter(s => s.name === resistName)[0];
            console.log(graphPoints);
            series.setData(graphPoints, false, false, false);
        }

        this.graph.redraw();
    }

}