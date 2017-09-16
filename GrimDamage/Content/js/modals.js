class Modals {
    constructor() {

    }

    show(modalId) {
        $('#'+modalId).modal('show');
    }

    add(modalId, modalTitle, modalContent) {
        var div1 = document.createElement('div');
        div1.id = modalId;
        div1.className = 'modal fade bd-example-modal-lg';
        div1.setAttribute("role", "dialog");

        var innerDiv1m = document.createElement('div');
        innerDiv1m.className = 'modal-dialog modal-lg';
        div1.appendChild(innerDiv1m);

        var innerDiv2m = document.createElement('div');
        innerDiv2m.className = 'modal-content';
        innerDiv1m.appendChild(innerDiv2m);

        var innerDiv3 = document.createElement('div');
        innerDiv3.className = 'modal-header';
        innerDiv2m.appendChild(innerDiv3);

        var buttonM = document.createElement("button");
        buttonM.className = 'close';
        buttonM.setAttribute("data-dismiss", "modal");
        buttonM.setAttribute("aria-hidden", "true");
        buttonM.setAttribute("value", "Close");
        innerDiv3.appendChild(buttonM);

        var headerM = document.createElement("h4");
        headerM.className = 'modal-title';
        var headerText = document.createTextNode(modalTitle);
        headerM.appendChild(headerText);
        innerDiv3.appendChild(headerM);

        var innerDiv31 = document.createElement('div');
        innerDiv31.className = 'modal-body';
        innerDiv31.innerHTML = modalContent;
        innerDiv2m.appendChild(innerDiv31);

        //var para = document.createElement('p');
        //innerDiv31.appendChild(para);
        //para.innerHTML = "paragraph";

        var innerDiv32 = document.createElement('div');
        innerDiv32.className = 'modal-footer';
        innerDiv2m.appendChild(innerDiv32);

        var closeButton = document.createElement("input");
        closeButton.className = 'btn btn-default';
        closeButton.setAttribute("data-dismiss", "modal");
        closeButton.setAttribute("type", "button");
        closeButton.setAttribute("value", "Close");
        innerDiv32.appendChild(closeButton);

        document.getElementById('modals').appendChild(div1);
    }

    addBossModal() {
        /* Create the modal */
        this.add("bossmodal", "", "");

        /* Create the bosschart */
        let bosschartDiv = $('<div/>', {
            id: "bosschart"
        });

        //Not the best way, but mixing javascript and jquery is meh.. so fuck it, refactor later
        bosschartDiv.appendTo($('#bossmodal .modal-body'));

        /* Init the chart */
        let bosschart = Highcharts.chart('bosschart',
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
                        dataLabels: {
                            enabled: false
                        },
                        size: '75%',
                        showInLegend: true
                    }
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

                    linkedTo:':previous',
                    data: []
                }]
            }
        );
        return bosschart;
    }
}