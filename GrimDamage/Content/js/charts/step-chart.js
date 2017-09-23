class StepChart {
    constructor(id, title) {
        /// <param name="id">DOM Id of the element to create the chart on</param>
        /// <param name="title">Title of the chart</param>
        this.series = [];

        let preloadMinutes = 3;
        this.chart = Highcharts.stockChart(id, {
            
            xAxis: {
                events: {
                    afterSetExtremes: function (event) {
                        console.debug('Selection', event); // TODO: handle this
                    }
                },
            },
            chart: {
                animation: false
            },
            series: [{
                name: 'Total',
                data: (function () {
                    // generate an array of random data
                    let data = [];
                    const time = (new Date()).getTime();
                    let i;

                    for (i = -(preloadMinutes*60+1) ; i <= 0; i += 1) {
                        data.push({
                            x: time + i * 1000,
                            y: 0
                        });
                    }
                    return data;
                }())
            }],
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
            },
        });
        /*
        this.addPoint('Fire', 0, 10);
        this.addPoint('Fire', 1, 5);
        this.addPoint('Lightning', 0, 1);
        this.addPoint('Lightning', 1, 15);
        this.addPoint('Aether', 0, 45);
        this.addPoint('Aether', 1, 55);
        */


        this.series['Total'] = 0;
    }

    addPoint(type, timestamp, value) {
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
        this.chart.series[this.series[type]].addPoint([timestamp, value], false);
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
