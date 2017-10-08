function createDamageTakenPieChart(id, previousChart) {

    let preloadSeries = [];
    if (previousChart && previousChart.series) {
        console.debug('Recreated a chart with existing series', previousChart.series);

        for (let key in previousChart.series) {
            if (previousChart.series.hasOwnProperty(key)) {
                let sData = previousChart.series[key].data;
                for (let idx = 0; idx < sData.length; idx++) {
                    preloadSeries.push({
                        y: sData[idx].y,
                        label: sData[idx].label,
                        name: sData[idx].name
                    });
                }
            }
        }
    }
    if (preloadSeries.length === 0) {
        preloadSeries = [
            {
                name: 'Imaginary',
                y: 100
            }
        ];
    }

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
            data: preloadSeries
        }]
    });
}