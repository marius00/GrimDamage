function createResistGraph(id, sequenceLength, colors, previousChart) {
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

                preloadSeries.push(obj);

            }
        }
        console.log('Preload:', preloadSeries);
    } else {
        const types = ['Physical', 'Fire', 'Cold', 'Lightning', 'Bleeding', 'Poison', 'Vitality', 'Aether', 'Chaos', 'Pierce'];
        for (let idx = 0; idx < types.length; idx++) {
            preloadSeries.push({
                type: 'spline',
                marker: { enabled: false },
                name: types[idx],
                color: colors.color(types[idx]),
                data: Array.from(Array(sequenceLength), () => 0)
            });
        }
    }


    return Highcharts.chart(id,
        {
            chart: {
                type: 'spline'
            },
            title: {
                text: 'Resists'
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
                    text: 'Resists'
                },
                labels: {
                    formatter: function () {
                        return Math.round(this.value);
                    }
                }
            },
            tooltip: {//
                pointFormat: 'You had <b>{point.y:,.0f}</b> {series.name} resist'
            },
            plotOptions: {
                spline: {
                    animation: false,
                    pointStart: 0
                }
            },
            series: preloadSeries
        });
}
