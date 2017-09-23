
enableLogToCsharp();

// KO, move this?
/*
function _VM() {
    this.suggestName = triggerSuggestNameDialog;
    this.isZoneUnknown = ko.observable(false);
}
var VM = new _VM();
ko.applyBindings(VM, document.getElementById('name-suggest-ko'));
*/


var colors = new Colors();
var damageDoneStepChart = new StepChart('step-test', 'Damage Done');
const damageTakenAtDeathChart = new StepChart('container-died-damage-taken-zoomy', 'Damage Taken');


// Chart and parsing
let chartDamageTaken, chartDamageDealt;
[chartDamageTaken, chartDamageDealt] = loadCharts();
var p = new DamageParser(chartDamageTaken, chartDamageDealt, damageDoneStepChart);

setCsharpTickCallback((players, damageDealt, damageTaken, damageDealtSingleTarget, playerLocationName, detailedDamageDealt, detailedDamageTaken, entitiesList) => {
    p.tick(players, damageDealt, damageTaken, damageDealtSingleTarget, playerLocationName, detailedDamageDealt, detailedDamageTaken, entitiesList);
    //VM.isZoneUnknown(playerLocationName === 'Unknown');
});

setCsharpTickInterval(1000);
setCsharpLoadHistoryCallback((dataset) => { console.log("Load:", dataset); });


// Track callback events
const deathTrackerViewModel = new DeathTrackerViewModel(
    () => $('#deathModal').modal('show'),
    createChartDamageTaken('container-died-damage-taken', 10),
    damageTakenAtDeathChart
);
let deathTracker = new DeathTracker(p, deathTrackerViewModel); // damage parser is the current 'track player id' class, may be split later
ko.applyBindings(deathTrackerViewModel, document.getElementById('what-killed-me-listing'));

// State update callback
setCsharpRequestCallback((type, dataset) => {
    if (type === TYPE_STATES) {
        console.log(dataset);
        deathTracker.process(dataset);
    }
});


// ===================================================
// Damage taken view, pie graph creation
createDamageTakenPieChart('damage-taken-pie-graph');
// ===================================================


// ===================================================
// Light/Dark mode toggle
let isDarkModeEnabled = window.location.search.toString().toLowerCase().indexOf('darkmode=1') !== -1;
ko.applyBindings(new LightModeToggleViewModel(isDarkModeEnabled), document.getElementById('light-mode-view'));
// ===================================================





// ===================================================
// FAQ -- Consider moving to its own file
function FaqViewModel() {
    this.questions = ko.observableArray([{ issue: '', solution: '' }]);
}

const faqViewModel = new FaqViewModel();

ko.applyBindings(faqViewModel, document.getElementById('faq'));

let faqElements = [];
const questions = $('#questions').children();
for (var i = 0; i < questions.length; i++) {
    let question = $(questions[i]).children(':nth-child(1)').text();
    let answer = $(questions[i]).children(':nth-child(2)').html();
    faqElements.push({ issue: question, solution: answer });
}
faqViewModel.questions(faqElements);
// ===================================================
