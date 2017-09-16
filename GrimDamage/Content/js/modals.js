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

    }
}