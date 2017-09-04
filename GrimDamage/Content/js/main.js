﻿var globalChart;
function loadChart() {
    globalChart = Highcharts.chart('container',
        {
            chart: {
                type: 'area'
            },
            title: {
                text: 'Damage dealt'
            },
            subtitle: {
                text: 'Lacking stash space? <a href="http://grimdawn.dreamcrash.org/ia">Try item assistant</a>'
            },
            xAxis: {
                allowDecimals: false,
                labels: {
                    formatter: function() {
                        return this.value; // clean, unformatted number for year
                    }
                }
            },
            yAxis: {
                title: {
                    text: 'Damage per second'
                },
                labels: {
                    formatter: function() {
                        return this.value / 1000 + 'k';
                    }
                }
            },
            tooltip: {
                pointFormat: '{series.name} produced <b>{point.y:,.0f}</b><br/>warheads in {point.x}'
            },
            plotOptions: {
                area: {
                    pointStart: 1940,
                    marker: {
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
            series: [
                {
                    name: 'Physical',
                    data: [100,200,1000,2000]
                },
                {
                    name: 'Lightning',
                    data: [0, 0]
                },
                {
                    name: 'Vitality', data: [0, 0]
                },
                {
                    name: 'Aether', data: [0,0]
                },
                {
                    name: 'Bleeding', data: [0, 0]
                },
                {
                    name: 'Acid', data: [0, 0]
                },
                {
                    name: 'Chaos', data: [0, 0]
                },
                {
                    name: 'Cold', data: [0, 0]
                },
                {
                    name: 'Fire', data: [0, 0]
                },
                {
                    name: 'Unknown', data: [0, 0]
                },
                {
                    name: 'Total', data: [0, 0]
                }
            ]
        });
}