
function createChartDamageTaken(id, sequenceLength, colors, previousChart) {
    /// <param name="id">DOM Id of the element to create the chart on</param>
    /// <param name="sequenceLength">The length of the sequence to preload</param>
    /// <param name="previousChart">The previous object, if being recreated for re-theming</param>

    let preloadSeries = [];
    if (previousChart && previousChart.series) {
        console.debug('Recreated a chart with existing series', previousChart.series);


        for (let key in previousChart.series) {
            if (previousChart.series.hasOwnProperty(key)) {
                const s = previousChart.series[key];
                const data = [];
                for (let dataKey in s.data) {
                    if (s.data.hasOwnProperty(dataKey)) {
                        data.push({
                            x: s.data[dataKey].x,
                            y: s.data[dataKey].y
                        });
                    }
                }

                let obj = {
                    name: s.name,
                    data: data,
                    color: s.color,
                    marker: s.marker || { enabled: false },
                    type: s.type
                };

                if (s.fillOpacity)
                    obj.fillOpacity = s.fillOpacity;

                if (s.tooltip)
                    obj.tooltip = s.tooltip;

                preloadSeries.push(obj);

            }
        }
        console.log('Preload:', preloadSeries);
    } else {
        /*
        let pdata = [];
        for (let idx = 0; idx < sequenceLength; idx++) {
            pdata.push({

            });
        }*/
        const turbo = 0;
        preloadSeries = [
            {
                name: 'Total',
                fillOpacity: 0.1,
                data: Array.from(Array(sequenceLength), () => null),
                color: colors.color('Total'),
                turboThreshold: turbo
            },
            {
                name: 'Raw Total',
                fillOpacity: 0.03,
                data: Array.from(Array(sequenceLength), () => null),
                color: colors.color('Raw Total'),
                tooltip: {
                    pointFormat: 'Received <b>{point.y:,.0f}</b> {series.name} damage'
                },
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Physical',
                color: colors.color('Physical'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Lightning',
                color: colors.color('Lightning'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Vitality',
                color: colors.color('Vitality'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Aether',
                color: colors.color('Aether'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                name: 'Bleeding',
                marker: { enabled: false },
                color: colors.color('Bleeding'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Poison',
                color: colors.color('Acid'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Pierce',
                color: colors.color('Pierce'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Chaos',
                color: colors.color('Chaos'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'PercentCurrentLife',
                color: colors.color('PercentCurrentLife'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'LifeLeech',
                color: colors.color('LifeLeech'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Cold',
                color: colors.color('Cold'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            },
            {
                type: 'spline',
                marker: { enabled: false },
                name: 'Fire',
                color: colors.color('Fire'),
                data: Array.from(Array(sequenceLength), () => null),
                turboThreshold: turbo
            }
        ];
    }


    return Highcharts.chart(id,
        {
            chart: {
                type: 'area'
            },
            title: {
                text: 'Damage taken'
            },
            xAxis: {
                allowDecimals: false,
                labels: {
                    formatter: function () {
                        return this.value; // clean, unformatted number
                    }
                },
                tickPixelInterval: 150
            },
            yAxis: {
                title: {
                    text: 'Damage per second'
                },
                events: {
                    afterSetExtremes: function() {
                        console.log('after extrmee!');
                        // var chart = $('#container').highcharts();
                    }
                },
                labels: {
                    formatter: function () {
                        if (this.value > 10000)
                            return Math.round(this.value / 1000) + 'k';
                        else
                            return Math.round(this.value / 1000, 1) + 'k';
                    }
                }
            },
            tooltip: {//
                //pointFormat: 'Received <b>{point.y:,.0f}</b> {series.name} damage <br> ({point.extrapolated:,.0f} before resists)'
                pointFormat: 'Received <b>{point.y:,.0f}</b> {series.name} damage'
            },
            plotOptions: {
                series: {
                    turboThreshold: 0
                },
                area: {
                    animation: false,
                    pointStart: 0,
                    marker: {

                        animation: false,
                        enabled: false,
                        symbol: 'circle',
                        radius: 2,
                        states: {
                            hover: {
                                enabled: true
                            }
                        }
                    }
                }
            },
            series: preloadSeries
        });
}

function createChartDamageDealt(id, sequenceLength, colors, previousChart) {
    let preloadSeries = [
        {
            name: 'Total',
            color: colors.color('Total'),
            fillOpacity: 0.3,
            data: Array.from(Array(sequenceLength), () => null)
        },
        {
            type: 'spline',
            marker: { enabled: false },
            name: 'Single Target',
            color: colors.color('Single Target'),
            data: Array.from(Array(sequenceLength), () => null)
        }
    ];


    if (previousChart && previousChart.series) {
        console.debug('Recreated a chart with existing series', previousChart.series);

        for (let key in previousChart.series) {
            if (previousChart.series.hasOwnProperty(key)) {
                const s = previousChart.series[key];
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
                    color: s.color || colors.color(s.name),
                    marker: s.marker || { enabled: false },
                    type: s.type
                });

            }
        }

        console.log('Preload:', preloadSeries);
    }

    return Highcharts.chart(id,
        {
            chart: {
                type: 'area'
            },
            title: {
                text: 'Damage dealt'
            },
            subtitle: {
                text: '(...)'
            },

            rangeSelector: {
                buttons: [{
                    count: 10,
                    text: '10'
                }, {
                    count: 5,
                    text: '5M'
                }, {
                    type: 'all',
                    text: 'All'
                }],
                inputEnabled: false,
                selected: 0
            },

            xAxis: {
                allowDecimals: false,
                labels: {
                    formatter: function () {
                        return this.value; // clean, unformatted number
                    }
                },
                tickPixelInterval: 150
            },
            yAxis: {
                title: {
                    text: 'Damage per second'
                },
                labels: {
                    formatter: function () {
                        if (this.value > 10000)
                            return Math.round(this.value / 1000) + 'k';
                        else
                            return Math.round(this.value / 1000, 1) + 'k';
                    }
                }
            },
            tooltip: {
                pointFormat: '{series.name} did <b>{point.y:,.0f}</b><br/>damage'
            },
            plotOptions: {
                series: {
                    turboThreshold: 0
                },
                area: {
                    animation: false,
                    pointStart: 0,
                    marker: {

                        animation: false,
                        enabled: false,
                        symbol: 'circle',
                        radius: 2,
                        states: {
                            hover: {
                                enabled: true
                            }
                        }
                    }
                }
            },
            series: preloadSeries
        });
}

    /* Keeping the code for now
    let damageTakenPie = Highcharts.chart('container-damage-taken-pie',
=======
function createPieChartDamageTaken(id) {
    return Highcharts.chart(id,
>>>>>>> master
        {
            chart: {
                plotBackgroundColor: null,
                plotBorderWidth: null,
                plotShadow: false,
                type: 'pie'
            },
            title: {
                text: 'Total damage taken'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    dataLabels: {
                        enabled: false
                    },
                    showInLegend: true
                }
            },
            series: [{
                name: 'Damage taken',
                colorByPoint: true,
                data: []
            }]
        });
<<<<<<< HEAD dataTable
        */
