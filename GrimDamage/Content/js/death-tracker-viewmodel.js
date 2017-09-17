// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class DeathTrackerViewModel {
    constructor(chartDamageTaken) {
        /// <summary>Responsible for rendering the player deaths view</summary>  
        var self = this;
        this.deaths = ko.observableArray([]);


        this.damageTakenChart = chartDamageTaken;

        this.deathtable = $('#killed-table').DataTable({
            "columns": [
                { "data": "timestamp"},
                { "data": "location" }
            ]
        });
        $('#killed-table tbody').on('click', 'tr', this.deathtableclicked);
        p.modals.add("death-modal", "Death", "Death chart here");



        this._health = ko.observableArray([]);
        this.health = ko.pureComputed({
            read: function () {
                return self._health;
            },
            write: function (value) {
                /// <summary>Responsible for rendering the player health line on the chart</summary>  
                self._health(value);

                if (value.length > 0) {
                    let minTimestamp = Math.min.apply(Math, value.map(function (o) { return o.timestamp; }));
                    let dataPoints = [];
                    dataPoints = value.reduce(function (res, val) {
                        const xAxis = Math.floor((val.timestamp - minTimestamp) / 1000);

                            if (!res[xAxis]) {
                                res[xAxis] = {};
                                dataPoints.push(res[val]);
                            }

                            res[xAxis] = { x: parseInt(xAxis), y: val.amount };
                            return res;
                        },
                        {}
                    );

                    let points = Object.keys(dataPoints).map(function (key) {
                        return dataPoints[key];
                    });

                    const series = self.damageTakenChart.series.filter(s => s.name === 'Hitpoints')[0];
                    if (!series) {
                        console.log('Hitpoints series not found, creating now..');
                        self.damageTakenChart.addSeries(
                            {
                                type: 'spline',
                                dashStyle: 'Dash',
                                name: 'Hitpoints',
                                color: '#ff0000',
                                data: points,
                                tooltip: {
                                    pointFormat: '<b>{point.y:,.0f}</b> life remaining'
                                }
                            });

                    } else {
                        series.setData(points, true, false, false);
                    }
                    console.log('Settings points', points, 'for series hitpoints');

                }
            },
            owner: this
        });


        this._detailedDamagePoints = ko.observableArray([]);
        this.detailedDamagePoints = ko.pureComputed({
            read: function () {
                return self._detailedDamagePoints;
            },
            write: function (value) {
                /// <summary>Responsible for rendering the damage types the player has taken</summary>  
                self._detailedDamagePoints(value);
                console.log('Update the graph with the following data please:', value);

                if (value.length > 0) {
                    console.log('Pie graph data points:', this.getPieChartDataPoints(value));


                    // TS transform is: Math.ceil((ts - min)/1000)
                    const lineChartPoints = this.getLineChartDataPoints(value);
                    console.log('Line graph points:', lineChartPoints, 'Length:', lineChartPoints.length);

                    // Convert the dictionary to single x/y paired elements
                    let points = {};


                    // Create the datapoint values
                    const minTimestamp = Math.min.apply(Math, value.map(function (o) { return o.timestamp; }));
                    const maxTimestamp = Math.max.apply(Math, value.map(function (o) { return o.timestamp; }));
                    const xRange = Math.ceil((maxTimestamp - minTimestamp) / 1000);

                    for (let damageType in lineChartPoints) {
                        if (!points[damageType])
                            points[damageType] = [];

                        for (let x = 0; x < xRange; x++) {
                            if (lineChartPoints[damageType][x]) {
                                points[damageType].push({ x: x, y: lineChartPoints[damageType][x] });
                                //console.log('Inserting', damageType, 'x=', x, 'y=', lineChartPoints[damageType][x]);
                            } else {
                                points[damageType].push({ x: x, y: 0 });
                                //console.log('Inserting', damageType, 'x=', x, 'y=', 0);
                            }
                        }
                    }

                    // Set the datapoints on the graph
                    for (let damageType in points) {
                        const series = self.damageTakenChart.series.filter(s => s.name === damageType)[0];
                        if (series) {
                            series.setData(points[damageType], true, false, false);
                            console.log('Settings points', points[damageType], 'for series', damageType);
                        } else {
                            console.log(`Could not find series ${damageType}`);
                        }
                    }

                }

            },
            owner: this
        });
        

        this.showDeath = function(death) {
            const period = 10 * 1000; // TODO: The number '10' is used many places, consider making a constant
            self.resetGraph(10);
            self.detailedDamagePoints([]);
            console.log(death);

            // This is really unfortunate, but i have no better solution yet.
            const hardcodedClassVarName = 'deathTrackerViewModel';
            data.requestData(TYPE_DETAILED_DAMAGE_TAKEN,
                (death.timestamp - period).toString(),
                death.timestamp.toString(),
                death.entityId,
                `${hardcodedClassVarName}.detailedDamagePoints`);


            data.requestData(TYPE_HEALTH_CHECK,
                (death.timestamp - period).toString(),
                death.timestamp.toString(),
                death.entityId,
                `${hardcodedClassVarName}.health`);
        }
    }


    deathtableclicked() {
        let deathid = this.id;
        console.log('Clicked the row with id ' + deathid);
        p.modals.show('death-modal');
    }

    resetGraph(numElements) {
        console.log('Resetting graph');
        for (let idx = 0; idx < this.damageTakenChart.series.length; idx++) {
            this.damageTakenChart.series[idx].setData(Array.from(Array(numElements), () => null));
        }
        this.damageTakenChart.redraw();
    }

    getPieChartDataPoints(dataset) {
        /// <returns type="Dictionary">Generates a list of datapoints for a pie graph</returns>
        // Group by damage type and sum, this gets the totals usable for pie charts
        let dataPoints = [];
        dataPoints = dataset.reduce(function (res, value) {
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

    getLineChartDataPoints(dataset) {
        /// <summary>Transforms the timestamp value into a series X-axis point and groups by X-axis and damage type</summary>
        /// <returns type="Array">Generates a list of datapoints for a line graph</returns>
        
        const minTimestamp = Math.min.apply(Math, dataset.map(function (o) { return o.timestamp; }));
        let dataPoints = [];
        dataPoints = dataset.reduce(function (res, value) {
                const series = Math.ceil((value.timestamp - minTimestamp) / 1000);
                console.log('detected series', series);
                if (!res[value.damageType]) {
                    res[value.damageType] = {};
                    dataPoints.push(res[value]);
                }
                if (!res[value.damageType][series]) {
                    res[value.damageType][series] = 0;
                }

                res[value.damageType][series] += value.amount;
                return res;
            },
            {}
        );

        return dataPoints;
    }

    add(death) {
        /// <summary>Add a death</summary>  
        const label = `Died at ${new Date(death.timestamp).toLocaleTimeString()}`;
        this.deaths.push({
            label: label,
            timestamp: death.timestamp,
            entityId: death.entityId
        });
        let now = moment(new Date());
        this.deathtable.row.add({ "DT_RowId": death.entityId, "timestamp": now.format('HH:mm:ss'), "location": label }).draw(false);
    }
}