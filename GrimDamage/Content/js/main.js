function triggerSuggestNameDialog() {
    $.prompt({
        state0: {
            title: "What is this zone called?",
            persistent: false,
            position: { container: '#button-suggest-name', x: -80, y: 0, width: 350, arrow: 'rm' },
            html:
                '<input id="suggestion" type="text" size="35" title="The suggested name for this zone" maxlength="55"/> ',
            submit: function(e, v, m, f) {
                if (v === true) {
                    sendCsharpNameSuggestion($('#suggestion').val());
                }
            },
            buttons: { "Suggest name": true, "Close": false }
        }
    });
}