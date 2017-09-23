class Modals {
    constructor() {

    }

    show(modalId) {
        $(`#${modalId}`).modal('show');
    }

    add(modalId, modalTitle, modalContent) {
        const div1 = document.createElement('div');
        div1.id = modalId;
        div1.className = 'modal fade bd-example-modal-lg';
        div1.setAttribute('role', 'dialog');

        const innerDiv1M = document.createElement('div');
        innerDiv1M.className = 'modal-dialog modal-lg';
        div1.appendChild(innerDiv1M);

        const innerDiv2M = document.createElement('div');
        innerDiv2M.className = 'modal-content';
        innerDiv1M.appendChild(innerDiv2M);

        const innerDiv3 = document.createElement('div');
        innerDiv3.className = 'modal-header';
        innerDiv2M.appendChild(innerDiv3);

        const buttonM = document.createElement('button');
        buttonM.className = 'close';
        buttonM.setAttribute('data-dismiss', 'modal');
        buttonM.setAttribute('aria-hidden', 'true');
        buttonM.setAttribute('value', 'Close');
        innerDiv3.appendChild(buttonM);

        const headerM = document.createElement('h4');
        headerM.className = 'modal-title';
        const headerText = document.createTextNode(modalTitle);
        headerM.appendChild(headerText);
        innerDiv3.appendChild(headerM);

        const innerDiv31 = document.createElement('div');
        innerDiv31.className = 'modal-body';
        innerDiv31.innerHTML = modalContent;
        innerDiv2M.appendChild(innerDiv31);

        //var para = document.createElement('p');
        //innerDiv31.appendChild(para);
        //para.innerHTML = "paragraph";

        const innerDiv32 = document.createElement('div');
        innerDiv32.className = 'modal-footer';
        innerDiv2M.appendChild(innerDiv32);

        const closeButton = document.createElement('input');
        closeButton.className = 'btn btn-default';
        closeButton.setAttribute('data-dismiss', 'modal');
        closeButton.setAttribute('type', 'button');
        closeButton.setAttribute('value', 'Close');
        innerDiv32.appendChild(closeButton);

        document.getElementById('modals').appendChild(div1);
    }

    addBossModal() {
        /* Create the modal */
        this.add('bossmodal', '', '');

        /* Create the bosschart */
        const bosschartDiv = $('<div/>', {
            id: 'bosschart'
        });

        //Not the best way, but mixing javascript and jquery is meh.. so fuck it, refactor later
        bosschartDiv.appendTo($('#bossmodal .modal-body'));

        /* Init the chart */
        const bosschart = Highcharts.chart('bosschart',
            {
                chart: {
                    renderTo: 'container',
                    type: 'pie'
                },
                title: {
                    text: 'Boss chart'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: false,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false,
                            format: '<b>{point.name}</b>: {point.y} ({point.percentage:.1f}%)',
                            style: {
                                color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                            }
                        },
                        size: '75%',
                        showInLegend: true
                    }
                },
                tooltip: {
                    pointFormat: '{point.y} ({point.percentage:.1f}%)'
                },
                series: [{
                    size: 200,
                    center: ['20%', '20%'],
                    title: {
                        align: 'center',
                        format: '{name}<br>{total:.0f}',
                        verticalAlign: 'middle'
                    },
                    name: 'Dealt',
                    innerSize: '70%',
                    data: []
                }, {
                    size: 200,
                    center: ['80%', '20%'],
                    title: {
                        align: 'center',
                        format: '{name}<br>{total:.0f}',
                        verticalAlign: 'middle'
                    },
                    name: 'Taken',
                    innerSize: '70%',

                    linkedTo: ':previous',
                    data: []
                }]
            }
        );
        return bosschart;
    }
}