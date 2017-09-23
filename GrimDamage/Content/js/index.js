/// <reference path="logic/pause-tracker.js" />
/// <reference path="logic/damage-parser.js" />
/// <reference path="logic/death-tracker.js" />
/// <reference path="integration.js" />
/// <reference path="charts/modals.js" />
/// <reference path="charts/graphs.js" />
/// <reference path="charts/colors.js" />
/// <reference path="charts/step-chart.js" />
/// <reference path="vendor/jquery-3.2.1.min.js" />
/// <reference path="vendor/knockout-3.4.0.js" />
/// <reference path="light-mode-toggle.js" />

enableLogToCsharp();

let pauseTracker = new PauseTracker();
var colors = new Colors();
var damageDoneStepChart = new StepChart('step-test', 'Damage Done');
const damageTakenAtDeathChart = new StepChart('container-died-damage-taken-zoomy', 'Damage Taken');


// Chart and parsing
let chartDamageTaken, chartDamageDealt;
[chartDamageTaken, chartDamageDealt] = loadCharts();
var p = new DamageParser(chartDamageTaken, chartDamageDealt, damageDoneStepChart);

setCsharpTickCallback((players, damageDealt, damageTaken, damageDealtSingleTarget, playerLocationName, detailedDamageDealt, detailedDamageTaken, entitiesList) => {
    if (pauseTracker.isActive) {
        p.tick(players,
            damageDealt,
            damageTaken,
            damageDealtSingleTarget,
            playerLocationName,
            detailedDamageDealt,
            detailedDamageTaken,
            entitiesList);
    }
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




// ===================================================
// State update callback
setCsharpRequestCallback((type, dataset) => {
    if (type === TYPE_STATES) {
        pauseTracker.process(dataset);
        deathTracker.process(dataset);
    }
});
// ===================================================




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


