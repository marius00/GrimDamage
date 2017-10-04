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
/// <reference path="logic/graph-damage-dealt-single-aoe.js" />
/// <reference path="logic/graph-damage-taken-graph-handler.js" />

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
var p = new DamageParser(damageDoneStepChart);


let chartDamageTaken = createChartDamageTaken('container-damage-taken', 100, colors); // TODO set this the rightplace!
let damageTakenGraphLogichandler = new DamageTakenGraphLogichandler(database, chartDamageTaken);

let chartDamageDealt = createChartDamageDealt('container-damage-done', 100, colors);
let damageDealtGraphHandler = new DamageDealtGraphLogicHandler(database, chartDamageDealt);



setCsharpTickInterval(1000);
setCsharpLoadHistoryCallback((dataset) => { console.log('Load:', dataset); });


//Modal class
var Modal = new Modals();

// Track callback events
let damageTakenChartDiedPopup = createChartDamageTaken('container-died-damage-taken', 10, colors);
const deathTrackerViewModel = new DeathTrackerViewModel(
    database,
    () => Modal.show('#deathModal'),
    damageTakenChartDiedPopup,
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
// Resists graph
//let resistGraph = createResistGraph('resistance-graph', 100, colors);
//let playerResistsGraphLogichandler = new PlayerResistsGraphLogichandler(database, resistGraph);
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
function recreateGraphs(mode) {
    if (mode === 'light') {
        $('#bootstrap-theme').attr('href', 'css/vendor/bootstrap.min.css');
    } else {
        EnableHighchartsDarkmode(); // Apply graph theme
        $('#bootstrap-theme').attr('href', 'css/vendor/bootstrap-dark.min.css');
    }

    damageDoneStepChart = new StepChart('step-test', 'Damage Done', damageDoneStepChart);
    p.damageDoneStepChart = damageDoneStepChart;

    chartDamageTaken = createChartDamageTaken('container-damage-taken', 100, colors, chartDamageTaken);
    damageTakenGraphLogichandler.setGraph(chartDamageTaken);

    chartDamageDealt = createChartDamageDealt('container-damage-done', 100, colors, chartDamageDealt);
    damageDealtGraphHandler.setGraph(chartDamageDealt);
    


    damageTakenChartDiedPopup = createChartDamageTaken('container-died-damage-taken', 10, colors, damageTakenChartDiedPopup);
    damageTakenAtDeathChart = new StepChart('container-died-damage-taken-zoomy', 'Damage Taken', damageTakenAtDeathChart);

    deathTrackerViewModel.damageTakenChart = damageTakenChartDiedPopup;
    deathTrackerViewModel.stepChartDamageTaken = damageTakenAtDeathChart;
}

// Light/Dark mode toggle
let isDarkModeEnabled = window.location.search.toString().toLowerCase().indexOf('darkmode=1') !== -1;
ko.applyBindings(new LightModeToggleViewModel(isDarkModeEnabled, recreateGraphs), document.getElementById('light-mode-view'));
// ===================================================




// ===================================================
// Tick handler - This is up for refactoring
let lastPlayerId = undefined;
setCsharpTickCallback((players, damageDealt, playerLocationName, detailedDamageDealt, detailedDamageTaken, entitiesList) => {
    if (pauseTracker.isActive) {
        p.tick(players,
            damageDealt,
            detailedDamageDealt,
            detailedDamageTaken,
            entitiesList
        );

        const playerId = database.getMainPlayerEntityId();
        if (playerId && detailedDamageTaken[playerId]) {
            damageTakenPieHandler.addDamageTaken(detailedDamageTaken[playerId]);

            if (playerId !== lastPlayerId) {
                database.reset();
            }

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
        //database.setEntities(entitiesList); // TODO: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        // Request new detailed damage dealt
        if (playerId) {
            data.requestData(TYPE_DETAILED_DAMAGE_TAKEN,
                database.getHighestDamageTakenTimestamp().toString(),
                TimestampEverything,
                playerId,
                'database.addDetailedDamageTaken');

            data.requestData(TYPE_DETAILED_DAMAGE_DEALT,
                database.getHighestDamageDealtTimestamp().toString(),
                TimestampEverything,
                playerId,
                'database.addDetailedDamageDealt');


            data.requestData(TYPE_FETCH_RESISTS_CHECK,
                database.getHighestResistTimestamp().toString(),
                TimestampEverything,
                playerId,
                'database.addResists');
        }


        data.requestData(TYPE_FETCH_ENTITIES,
            '0',
            '0',
            0,
            'database.setEntities');


        // Tick/update for damage dealt graph
        damageDealtGraphHandler.update();
        damageTakenGraphLogichandler.update();
    }
    //VM.isZoneUnknown(playerLocationName === 'Unknown');
});
// ===================================================


