﻿function loadCharts() {
    let damageTakenChart = Highcharts.chart('container-damage-taken',
        {
            chart: {
                type: 'area'
            },
            title: {
                text: 'Damage taken'
            },
            subtitle: {
                text: '(...)'
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
                pointFormat: 'Received {series.name} <b>{point.y:,.0f}</b><br/>damage'
            },
            plotOptions: {
                area: {
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
            series: [
                {
                    name: 'Total',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Physical',
                    color: '#000000',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Lightning',
                    color: '#4658f8',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Vitality',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Aether',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    name: 'Bleeding',
                    marker: { enabled: false },
                    color: '#ffaec9',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#00ff00',
                    name: 'Acid',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#ffffff',
                    name: 'Pierce',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },

                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Chaos',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },

                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'PercentCurrentLife',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },

                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'LifeLeech',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#0000ff',
                    name: 'Cold',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#ff0000',
                    name: 'Fire',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                }
            ]
        });


    let damageDealtChart = Highcharts.chart('container-damage-done',
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
                area: {
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
            series: [
                {
                    name: 'Total',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Single Target',
                    color: '#000000',
                    data: [null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null]
                },
                {
                    type: 'flags',
                    name: 'EventLine',
                    color: '#333333',
                    shape: 'circlepin',
                    y: 15,
                    data: [
                        { x: 8, text: 'Player detected', title: 'Parser started' },
                    ],
                    onSeries: 'Single Target',
                    showInLegend: false
                }
            ]
        });

    let damageTakenPie = Highcharts.chart('container-damage-taken-pie',
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

    return [damageTakenChart, damageDealtChart, damageTakenPie];
}