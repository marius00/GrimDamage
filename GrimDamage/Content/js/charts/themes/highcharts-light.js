
function EnableHighchartsLightmode() {

    Highcharts.theme = {
        colors: ['#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970',
            '#f28f43', '#77a1e5', '#c42525', '#a6c96a'],
        chart: {
            backgroundColor: '#fff',
            borderWidth: 0,
            plotBackgroundColor: '#fff',
            plotShadow: false,
            plotBorderWidth: 0
        },
        title: {
            style: {
                color: '#274b6d',//#3E576F',
                fontSize: '16px'
            }
        },
        subtitle: {
            style: {
                color: '#4d759e'
            }
        },
        xAxis: {
            gridLineWidth: 0,
            lineColor: '#C0D0E0',
            tickColor: '#C0D0E0',
            labels: {
                style: {
                    color: '#666',
                    cursor: 'default',
                    fontSize: '11px',
                    lineHeight: '14px'
                }
            },
            title: {
                style: {
                    color: '#4d759e',
                    fontWeight: 'bold'
                }
            }
        },
        yAxis: {
            minorTickInterval: null,
            lineColor: '#C0D0E0',
            lineWidth: 1,
            tickWidth: 1,
            tickColor: '#C0D0E0',
            labels: {
                style: {
                    color: '#666',
                    cursor: 'default',
                    fontSize: '11px',
                    lineHeight: '14px'
                }
            },
            title: {
                style: {
                    color: '#4d759e',
                    fontWeight: 'bold'
                }
            }
        },
        legend: {
            itemStyle: {
                color: '#274b6d',
                fontSize: '12px'
            },
            itemHoverStyle: {
                color: '#000'
            },
            itemHiddenStyle: {
                color: '#CCC'
            }
        },
        labels: {
            style: {
                color: '#3E576F'
            }
        },

        navigation: {
            buttonOptions: {
                theme: {
                    stroke: '#CCCCCC'
                }
            }
        }
    };

    // Apply the theme
    Highcharts.setOptions(Highcharts.theme);
}