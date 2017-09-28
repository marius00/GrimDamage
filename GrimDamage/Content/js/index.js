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
/// <reference path="logic/pie-chart-damage-taken-processor.js" />
/// <reference path="data/Database.js" />
/// <reference path="ViewModels/death-tracker-viewmodel.js" />

enableLogToCsharp();

// ===================================================
// Practically globals
let pauseTracker = new PauseTracker();
var colors = new Colors();
// ===================================================



// ===================================================
// Data lookup class
const database = new Database();
// ===================================================

// var damageDoneStepChart = new StepChart('step-test', 'Damage Done', damageDoneStepChart);
let damageDoneStepChart = new StepChart('step-test', 'Damage Done');
let damageTakenAtDeathChart = new StepChart('container-died-damage-taken-zoomy', 'Damage Taken');



// Chart and parsing
let chartDamageTaken, chartDamageDealt;
[chartDamageTaken, chartDamageDealt] = loadCharts();
var p = new DamageParser(chartDamageTaken, chartDamageDealt, damageDoneStepChart);


setCsharpTickInterval(1000);
setCsharpLoadHistoryCallback((dataset) => { console.log("Load:", dataset); });


//Modal class
var Modal = new Modals();

// Track callback events
const deathTrackerViewModel = new DeathTrackerViewModel(
    database,
    () => Modal.show('#deathModal'),
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
let damageTakenPieChart = createDamageTakenPieChart('damage-taken-pie-graph');
let damageTakenPieHandler = new DamageTakenPieHandler(damageTakenPieChart, 60 * 5);
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









// ===================================================
// Text view - each damage hit taken
const detailedDamageTakenTextVm = new DetailedDamageTakenTextViewModel(database);
ko.applyBindings(detailedDamageTakenTextVm, document.getElementById('damage-taken-detailed-textview'));
// ===================================================






// ===================================================
// Graph reconstruction - Changing light/dark mode
function recreateGraphs() {
    damageDoneStepChart = new StepChart('step-test', 'Damage Done', damageDoneStepChart);
    damageTakenAtDeathChart = new StepChart('container-died-damage-taken-zoomy', 'Damage Taken', damageTakenAtDeathChart);

    p.damageDoneStepChart = damageDoneStepChart;
    deathTrackerViewModel.stepChartDamageTaken = damageTakenAtDeathChart;
}
// ===================================================



// ===================================================
// Tick handler - This is up for refactoring
let lastPlayerId = undefined;
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

        const playerId = p.mainPlayerId;
        if (playerId && detailedDamageTaken[playerId]) {
            damageTakenPieHandler.addDamageTaken(detailedDamageTaken[playerId]);

            if (playerId !== lastPlayerId) {
                database.reset();
            }

            database.addDetailedDamageTaken(detailedDamageTaken[playerId]);
            lastPlayerId = playerId;

            detailedDamageTakenTextVm.update();
        }

        // For test
        /*
        if (playerId && detailedDamageDealt[playerId]) {
            database.addDetailedDamageTaken(detailedDamageDealt[playerId]);
            detailedDamageTakenTextVm.update();
        }*/

        database.setPlayerLocation(playerLocationName);
        database.setEntities(entitiesList);
    }
    //VM.isZoneUnknown(playerLocationName === 'Unknown');
});
// ===================================================