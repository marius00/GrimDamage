class StepChart {
    constructor(id, title) {
        this.series = [];
        this.chart = Highcharts.stockChart(id, {
            rangeSelector: {
                buttons: [{
                    count: 0.5,
                    type: 'minute',
                    text: '30sec'
                }, {
                    count: 1,
                    type: 'minute',
                    text: '1M'
                }, {
                    count: 2,
                    type: 'minute',
                    text: '2M'
                }, {
                    count: 5,
                    type: 'minute',
                    text: '5M'
                }, {
                    type: 'all',
                    text: 'All'
                }],
                inputEnabled: false,
                selected: 0
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
    }

    addPoint(type, timestamp, value) {
        if (!this.series.hasOwnProperty(type)) {
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
        this.chart.series[this.series[type]].addPoint([timestamp, value]);
    }
}
