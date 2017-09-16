// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class DeathTrackerViewModel {
    constructor(chartDamageTaken) {
        /// <summary>Responsible for rendering the player deaths view</summary>  
        var self = this;
        this.deaths = ko.observableArray([]);


        this.damageTakenChart = chartDamageTaken;


        this._detailedDamagePoints = ko.observableArray([]);
        this.detailedDamagePoints = ko.pureComputed({
            read: function () {
                return self._detailedDamagePoints;
            },
            write: function(value) {
                self._detailedDamagePoints(value);
                console.log('Update the graph with the following data please:', value);

                if (value.length > 0) {
                    console.log('Pie graph data points:', this.getPieChartDataPoints(value));


                    // TS transform is: Math.ceil((ts - min)/1000)
                    const lineChartPoints = this.getLineChartDataPoints(value);
                    console.log('Line graph points:', lineChartPoints, 'Length:', lineChartPoints.length);
                    let points = {};
                    for (let x in lineChartPoints) {
                        for (let damageType in lineChartPoints[x]) {
                            if (!points[damageType])
                                points[damageType] = [];

                            points[damageType].push({ x: parseInt(x), y: lineChartPoints[x][damageType] });
                        }
                    }

                    for (let damageType in points) {
                        const series = self.damageTakenChart.series.filter(s => s.name === damageType)[0];
                        if (series) {
                            series.setData(points[damageType], true, false, false);
                            console.log('Settings points', points[damageType], 'for series', damageType);
                        } else {
                            console.log(`Could not find series ${damageType}`);
                        }
                    }

                } else {
                    //self.resetGraph(10);
                }

            },
            owner: this
        });
        

        this.showDeath = function(death) {
            const period = 10 * 1000;
            self.resetGraph(10); // TODO: This causes things to fail later on..
            self.detailedDamagePoints([]);
            console.log(death);

            // This is really unfortunate, but i have no better solution yet.
            const hardcodedClassVarName = 'deathTrackerViewModel';
            data.requestData(TYPE_DETAILED_DAMAGE_DEALT, // TODO: Change this to taken, not dealt. Taken is just useful for testing.
                (death.timestamp - period).toString(),
                death.timestamp.toString(),
                death.entityId,
                `${hardcodedClassVarName}.detailedDamagePoints`);
        }
    }

    resetGraph(numElements) {
        console.log('Resetting graph');
        for (let idx = 0; idx < this.damageTakenChart.series.length; idx++) {
            this.damageTakenChart.series[idx].setData(Array.from(Array(numElements), () => null));
        }
        this.damageTakenChart.redraw();
    }

    getPieChartDataPoints(value) {
        /// <returns type="Dictionary">Generates a list of datapoints for a pie graph</returns>
        // Group by damage type and sum, this gets the totals usable for pie charts
        let dataPoints = [];
        dataPoints = value.reduce(function (res, value) {
                if (!res[value.damageType]) {
                    res[value.damageType] = 0;
                    dataPoints.push(res[value.damageType]);
                }
                res[value.damageType] += value.amount;
                return res;
            },
            {}
        );

        return dataPoints;
    }

    getLineChartDataPoints(value) {
        /// <summary>Transforms the timestamp value into a series X-axis point and groups by X-axis and damage type</summary>
        /// <returns type="Array">Generates a list of datapoints for a line graph</returns>
        
        let minTimestamp = Math.min.apply(Math, value.map(function (o) { return o.timestamp; }));
        let dataPoints = [];
        dataPoints = value.reduce(function (res, value) {
                const series = Math.ceil((value.timestamp - minTimestamp) / 1000);
                console.log('detected series', series);
                if (!res[series]) {
                    res[series] = {};
                    dataPoints.push(res[value]);
                }
                if (!res[series][value.damageType]) {
                    res[series][value.damageType] = 0;
                }

                res[series][value.damageType] += value.amount;
                return res;
            },
            {}
        );

        return dataPoints;
    }

    add(death) {
        /// <summary>Add a death</summary>  
        var label = `Died at ${new Date(death.timestamp).toLocaleTimeString()}`;
        this.deaths.push({
            label: label,
            timestamp: death.timestamp,
            entityId: death.entityId
        });
    }
}