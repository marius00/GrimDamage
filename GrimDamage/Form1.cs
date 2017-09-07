using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons;
using EvilsoftCommons.DllInjector;
using GrimDamage.GD.Logger;
using GrimDamage.GD.Processors;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Browser.dto;
using GrimDamage.GUI.Forms;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using GrimDamage.Statistics.dto;
using GrimDamage.Statistics.Service;
using log4net;

namespace GrimDamage {
    public partial class Form1 : Form {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Form1));

        private readonly StatisticsService _statisticsService;
        private readonly DamageParsingService _damageParsingService = new DamageParsingService();
        private readonly CefBrowserHandler _browser;
        private readonly MessageProcessorCore _messageProcessorCore;
        private readonly PositionTrackerService _positionTrackerService = new PositionTrackerService();
        private readonly CombatFileLogger _combatFileLogger = new CombatFileLogger();
        private readonly GeneralStateService _generalStateService = new GeneralStateService();
        


        public Form1(CefBrowserHandler browser) {
            InitializeComponent();
            this._browser = browser;
            _messageProcessorCore = new MessageProcessorCore(_damageParsingService, _combatFileLogger, _positionTrackerService, _generalStateService);
            _statisticsService = new StatisticsService(_damageParsingService);
            _browser.JsPojo.OnRequestUpdate += btnUpdateData_Click;
            _browser.JsPojo.OnSuggestLocationName += JsPojoOnOnSuggestLocationName;
        }

        private void JsPojoOnOnSuggestLocationName(object sender, EventArgs eventArgs) {
            SuggestLocationNameArgument args = eventArgs as SuggestLocationNameArgument;
            MessageBox.Show($"Upload this: user suggest name for current zone: {args.Suggestion}");
        }

        private void Form1_Load(object sender, EventArgs e) {
            Logger.Debug("Starting..");

            this.Closing += Form1_Closing;

            {
                var webView = new WebView(_browser);
                webView.TopLevel = false;
                this.panel1.Controls.Add(webView);
                webView.Show();
            }
            {
                var debugView = new DebugSettings(_combatFileLogger, _damageParsingService);
                debugView.TopLevel = false;
                this.panelDebugView.Controls.Add(debugView);
                debugView.Show();
            }

            _messageProcessorCore.OnHookActivation += (_, __) => {
                this.labelHookStatus.Text = "Hook activated";
                this.labelHookStatus.ForeColor = System.Drawing.Color.Green;
            };
        }


        private void Form1_Closing(object sender, CancelEventArgs e) {
            _messageProcessorCore?.Dispose();
        }



        private void btnShowDevtools_Click(object sender, EventArgs e) {
            _browser.ShowDevTools();
        }

        private void btnUpdateData_Click(object sender, EventArgs e) {
            var players = _statisticsService.GetPlayers();
            var pets = _statisticsService.GetPets();

            Dictionary<int, List<DamageEntryJson>> damageDealt = new Dictionary<int, List<DamageEntryJson>>();
            Dictionary<int, List<DamageEntryJson>> damageDealtToSingleTarget = new Dictionary<int, List<DamageEntryJson>>();
            Dictionary<int, List<DamageEntryJson>> damageTaken = new Dictionary<int, List<DamageEntryJson>>();
            foreach (var player in players) {
                damageDealt[player.Id] = _statisticsService.GetLatestDamageDealt(player.Id);
                damageTaken[player.Id] = _statisticsService.GetLatestDamageTaken(player.Id);
                damageDealtToSingleTarget[player.Id] = _statisticsService.GetLatestDamageDealtToSingleTarget(player.Id);
            }
            foreach (var pet in pets) {
                damageDealt[pet.Id] = _statisticsService.GetLatestDamageDealt(pet.Id);
                damageTaken[pet.Id] = _statisticsService.GetLatestDamageTaken(pet.Id);
                damageDealtToSingleTarget[pet.Id] = _statisticsService.GetLatestDamageDealtToSingleTarget(pet.Id);
            }

            _browser.JsInteractor.SetPets(pets);
            _browser.JsInteractor.SetPlayers(players);
            _browser.JsInteractor.SetDamageDealt(damageDealt);
            _browser.JsInteractor.SetDamageDealtToSingleTarget(damageDealtToSingleTarget);
            _browser.JsInteractor.SetDamageTaken(damageTaken);
            _browser.JsInteractor.SetPlayerLocation(_positionTrackerService.GetPlayerLocation());
            _browser.JsInteractor.SetStateChanges(_generalStateService.GetAndClearStates());

            _browser.NotifyUpdate();
        }

        private void linkDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://discord.gg/PJ87Ewa");
        }
    }
}
