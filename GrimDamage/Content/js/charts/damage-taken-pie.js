function createDamageTakenPieChart(id) {
    return Highcharts.chart(id, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        
        title: {
            text: null
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
            name: 'Brands',
            colorByPoint: true,
            data: [
                {
                    name: 'Chaos',
                    y: 56.33
                },
                {
                    name: 'Fire',
                    y: 24.03,
                    sliced: true,
                    selected: true
                },
                {
                    name: 'Aether',
                    y: 10.38
                },
                {
                    name: 'Cold',
                    y: 4.77
                },
                {
                    name: 'Physical',
                    y: 0.91
                },
                {
                    name: 'Piercing',
                    y: 0.2
                }
            ]
        }]
    });
}