class StepChart {
    constructor(id, title, previousChart) {
        /// <param name="id">DOM Id of the element to create the chart on</param>
        /// <param name="title">Title of the chart</param>
        /// <param name="previousChart">The previous object, if being recreated for re-theming</param>
        this.series = {};

        const preloadMinutes = 3;
        let preloadSeries = [];
        if (previousChart && previousChart.series) {
            console.debug('Recreated a chart with existing series', previousChart.series, 'from', previousChart.chart.series);

            for (let key in previousChart.series) {
                if (previousChart.series.hasOwnProperty(key)) {
                    const idx = previousChart.series[key];
                    const s = previousChart.chart.series[idx];
                    const data = [];
                    for (let dataKey in s.data) {
                        if (s.data.hasOwnProperty(dataKey)) {
                            data.push({
                                x: s.data[dataKey].x,
                                y: s.data[dataKey].y
                            });
                        }
                    }

                    preloadSeries.push({
                        name: s.name,
                        data: data,
                        color: s.color,
                        step: s.step,
                        tooltip: s.tooltip
                    });

                }
            }

            console.debug('Received preload data', preloadSeries);
            this.series = previousChart.series; // Redundant?
            previousChart.chart.destroy();
        }
        else {
            preloadSeries.push({
                name: 'Total',
                data: (function() {
                    // generate an array of random data
                    const data = [];
                    const time = (new Date()).getTime();
                    let i;

                    for (i = -(preloadMinutes * 60 + 1); i <= 0; i += 1) {
                        data.push({
                            x: time + i * 1000,
                            y: 0
                        });
                    }
                    return data;
                }())
            });
        }

        this.chart = Highcharts.stockChart(id, {
            
            xAxis: {
                events: {
                    afterSetExtremes: function (event) {
                        //console.debug('Selection', event); // TODO: handle this
                    }
                }
            },
            chart: {
                animation: false
            },
            series: preloadSeries,
            rangeSelector: {
                buttons: [{
                    count: 500,
                    type: 'millisecond',
                    text: '0.5sec'
                }, {
                    count: 10,
                    type: 'second',
                    text: '10sec'
                }, {
                    count: 30,
                    type: 'second',
                    text: '30sec'
                }, {
                    count: 1,
                    type: 'minute',
                    text: '1M'
                }, {
                    count: 3,
                    type: 'minute',
                    text: '3M'
                }, {
                    count: 5,
                    type: 'minute',
                    text: '5M'
                }, {
                    type: 'all',
                    text: 'All'
                }],
                inputEnabled: false,
                selected: 4
            },
            title: {
                text: title
            }
        });

        console.debug(`There are ${preloadSeries.length} series in this graph. Storing locally`);
        for (let idx = 0; idx < preloadSeries.length; idx++) {
            const s = preloadSeries[idx];
            console.log(`Storing series "${s.name}" as index ${idx}`);
            this.series[s.name] = idx;
        }
        //this.series['Total'] = 0;
    }

    addPoint(type, timestamp, value) {
        /// <param name="type">The damage type / name of the series</param>
        /// <param name="value">Y axis, usually damage taken</param>
        if (!this.series.hasOwnProperty(type)) {
            console.debug(`Creating new series for ${type}`);
            let newSeries = this.chart.addSeries({
                name: type,
                color: colors.color(type),
                step: false,
                tooltip: {
                    valueDecimals: 0
                }
            });
            this.series[type] = newSeries.index;
        }
        //console.log('Adding damage for', type, 'at', timestamp, 'with', value, 'on', this.chart.series[this.series[type]]);
        this.chart.series[this.series[type]].addPoint([timestamp, Math.round(value)], false);
    }

    reset() {
        for (let idx = 0; idx < this.chart.series.length; idx++) {
            this.chart.series[idx].setData([], true, false, false);
        }
    }

    redraw() {
        this.chart.redraw();
    }
}
