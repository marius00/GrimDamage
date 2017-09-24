// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Classes


class DeathTrackerViewModel {
    constructor(showModal, chartDamageTaken, stepChartDamageTaken) {
        /// <summary>Responsible for rendering the player deaths view</summary>  
        var self = this;
        this.deaths = ko.observableArray([]);
        this.currentlyDisplayedDeath = 0;
        this.showModal = showModal;


        this.damageTakenChart = chartDamageTaken;
        this.stepChartDamageTaken = stepChartDamageTaken;


        this.calculateGraphPosition = function(timestamp) {
            const startingPoint = this.currentlyDisplayedDeath - 10 * 1000;
            const series = Math.floor((timestamp - startingPoint) / 1000);
            return series;
        }

        this._health = ko.observableArray([]);
        this.health = ko.pureComputed({
            read: function () {
                return self._health;
            },
            write: function (value) {
                /// <summary>Responsible for rendering the player health line on the chart</summary>  
                self._health(value);

                for (let idx = 0; idx < value.length; idx++) {
                    self.stepChartDamageTaken.addPoint('hitpoints', value[idx].timestamp, value[idx].amount);
                }
                console.debug('Received life data for death:', value);

                self.stepChartDamageTaken.redraw();

                if (value.length > 0) {
                    let dataPoints = [];
                    dataPoints = value.reduce(function (res, val) {
                        const xAxis = self.calculateGraphPosition(val.timestamp);

                        if (!res[xAxis]) {
                            res[xAxis] = {};
                            dataPoints.push(res[val]);
                        }

                        res[xAxis] = { x: parseInt(xAxis), y: val.amount };
                        return res;
                    },{});

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
                    self.addOrUpdatePie(value);
                    console.log('Pizza pie:', self.getPieChartDataPoints(value));


                    for (let idx = 0; idx < value.length; idx++) {
                        console.log('self.stepChartDamageTaken.addPoint',
                            value[idx].damageType,
                            value[idx].timestamp,
                            value[idx].amount);
                        self.stepChartDamageTaken.addPoint(value[idx].damageType, value[idx].timestamp, value[idx].amount);
                    }
                    console.debug('Received damage data for death:', value);

                    self.stepChartDamageTaken.redraw();

                    //console.log('Pie graph data points:', this.getPieChartDataPoints(value));

                    const lineChartPoints = this.getLineChartDataPoints(value);
                    console.log('Line graph points:', lineChartPoints, 'Length:', lineChartPoints.length);

                    // Convert the dictionary to single x/y paired elements
                    let points = {};
                    const xRange = 10;

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


        this.addOrUpdatePie = function(value) {
            /* Mini Pie */
            const dataset = self.getPieChartDataPoints(value);
            let total = 0;
            for (let key in dataset) {
                total += dataset[key];
            }

            let pies = [];
            for (let key in dataset) {
                pies.push({
                    name: key,
                    y: 100 * (dataset[key] / total),
                    // TODO: I'd like to be able to show the total..
                    label: dataset[key]
                    //color: 
                });
            }


            const pieSeries = this.damageTakenChart.series.filter(s => s.type === 'pie')[0];
            if (!pieSeries) {
                this.damageTakenChart.addSeries({
                        type: 'pie',
                        name: 'Pie!',
                        data: pies,
                        center: [30, 5],
                        size: 100,
                        showInLegend: false,
                        dataLabels: {
                            enabled: false
                        },
                        tooltip: {
                            pointFormat: '<b>{point.label:,.0f}</b> damage taken ({point.y:,.0f}% of total)'
                        }
                    }
                );
            } else {
                pieSeries.setData(pies);
            }
        }
        

        this.showDeath = function(death) {
            const period = 10 * 1000; // TODO: The number '10' is used many places, consider making a constant
            self.resetGraph(10);
            self.stepChartDamageTaken.reset();
            self.currentlyDisplayedDeath = death.timestamp;
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

            self.showModal();
        }
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
        
        let dataPoints = [];
        const self = this;
        dataPoints = dataset.reduce(function (res, value) {
            const series = self.calculateGraphPosition(value.timestamp);
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
        {});

        return dataPoints;
    }

    add(death) {
        /// <summary>Add a death</summary>  
        const label = 'You died a horrible death';
        const labelTimestamp = new Date(death.timestamp).toLocaleTimeString();

        const d = {
            label: label,
            labelTimestamp: labelTimestamp,
            timestamp: death.timestamp,
            entityId: death.entityId
        };

        this.deaths.push(d);
        this.showDeath(d);
    }
}