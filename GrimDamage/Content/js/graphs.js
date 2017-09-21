
function createChartDamageTaken(id, sequenceLength) {
    
    return Highcharts.chart(id,
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
                    fillOpacity: 0.1,
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Physical',
                    color: '#000000',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Lightning',
                    color: '#4658f8',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Vitality',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Aether',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    name: 'Bleeding',
                    marker: { enabled: false },
                    color: '#ffaec9',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#00ff00',
                    name: 'Acid',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#ffffff',
                    name: 'Pierce',
                    data: Array.from(Array(sequenceLength), () => null)
                },

                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Chaos',
                    data: Array.from(Array(sequenceLength), () => null)
                },

                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'PercentCurrentLife',
                    data: Array.from(Array(sequenceLength), () => null)
                },

                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'LifeLeech',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#0000ff',
                    name: 'Cold',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    color: '#ff0000',
                    name: 'Fire',
                    data: Array.from(Array(sequenceLength), () => null)
                }
            ]
        });
}

function createChartDamageDealt(id, sequenceLength) {
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
                    data: Array.from(Array(sequenceLength), () => null)
                },
                {
                    type: 'spline',
                    marker: { enabled: false },
                    name: 'Single Target',
                    color: '#000000',
                    data: Array.from(Array(sequenceLength), () => null)
                },
                /*
                {
                    type: 'flags',
                    name: 'EventLine',
                    color: '#333333',
                    shape: 'circlepin',
                    y: 15,
                    gapUnit: 'value',
                    data: Array.from(Array(sequenceLength), () => null),
                    //onSeries: 'Single Target',
                    showInLegend: false
                }*/
            ]
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


function loadCharts() {
    let damageTakenChart = createChartDamageTaken('container-damage-taken', 100);
    let damageDealtChart = createChartDamageDealt('container-damage-done', 100);
    //let damageTakenPie = createPieChartDamageTaken('container-damage-taken-pie');

    return [damageTakenChart, damageDealtChart];
}