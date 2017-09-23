function createDamageTakenPieChart(id) {
    return Highcharts.chart(id, {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        tooltip: {
            pointFormat: '<b>{point.label:,.0f}</b> damage taken ({point.y:,.0f}% of total)'
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
            colorByPoint: true,
            data: [
                {
                    name: 'Imaginary',
                    y: 100
                }
            ]
        }]
    });
}