using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using EvilsoftCommons.Exceptions;
using GrimDamage.Crowdsourced.Web;
using GrimDamage.GD.Processors;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Browser.dto;
using GrimDamage.GUI.Forms;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using GrimDamage.Statistics.dto;
using GrimDamage.Statistics.Service;
using GrimDamage.Utilities;
using log4net;
using Newtonsoft.Json;

namespace GrimDamage {
    public partial class Form1 : Form {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Form1));

        private readonly StatisticsService _statisticsService;
        private readonly DamageParsingService _damageParsingService = new DamageParsingService();
        private readonly CefBrowserHandler _browser;
        private readonly MessageProcessorCore _messageProcessorCore;
        private readonly PositionTrackerService _positionTrackerService = new PositionTrackerService();
        private readonly GeneralStateService _generalStateService;
        private readonly AutoUpdateUtility _autoUpdateUtility = new AutoUpdateUtility();
        private readonly NameSuggestionService _nameSuggestionService;
        private readonly AppSettings _appSettings;
        private readonly CSharpJsStateMapper _cSharpJsStateMapper;



        public Form1(CefBrowserHandler browser, AppSettings appSettings) {
            InitializeComponent();
            _browser = browser;
            _appSettings = appSettings;
            _generalStateService = new GeneralStateService(_appSettings);

            _messageProcessorCore = new MessageProcessorCore(_damageParsingService, _positionTrackerService, _generalStateService, _appSettings);
            _statisticsService = new StatisticsService(_damageParsingService);
            _browser.JsPojo.OnRequestUpdate += TransferStatsToJson;
            _browser.JsPojo.OnSuggestLocationName += JsPojoOnOnSuggestLocationName;
            _browser.JsPojo.OnSave += JsPojoOnOnSave;
            _browser.JsPojo.OnLog += (sender, args) => {
                string data = (args as SaveParseArgument)?.Data;
                Logger.Warn(data);
                ExceptionReporter.ReportIssue(data);
            };

            _nameSuggestionService = new NameSuggestionService(GlobalSettings.BineroHost);
            _cSharpJsStateMapper = new CSharpJsStateMapper(_browser, _statisticsService, _generalStateService);
            _browser.JsPojo.OnRequestData += (sender, _args) => {
                RequestDataArgument args = _args as RequestDataArgument;
                long start;
                if (long.TryParse(args.TimestampStart, out start)) {
                    long end;
                    if (long.TryParse(args.TimestampEnd, out end)) {
                        _cSharpJsStateMapper.RequestData(args.Type, start, end, args.EntityId, args.Callback);
                    }
                    else {
                        Logger.Warn($"Could not parse timestamp {args.TimestampEnd} received for {args.Type}");
                    }
                }
                else {
                    Logger.Warn($"Could not parse timestamp {args.TimestampStart} received for {args.Type}");   
                }
            };



#if !DEBUG
            webViewPanel.Parent.Controls.Remove(webViewPanel);
            Controls.Clear();
            Controls.Add(webViewPanel);

            bool itemAssistantInstalled = Directory.Exists(GlobalSettings.ItemAssistantFolder);
            if (itemAssistantInstalled) {
                webViewPanel.Location = new Point { X = 0, Y = 0 };
                webViewPanel.Width = this.ClientSize.Width;
                webViewPanel.Height = this.ClientSize.Height;
            }
            else {
                const int margin = 5;
                webViewPanel.Location = new Point { X = 0, Y = linkItemAssistant.Height + margin*2 };
                webViewPanel.Width = this.ClientSize.Width;
                webViewPanel.Height = this.ClientSize.Height - linkItemAssistant.Height - margin * 2;
                linkItemAssistant.Location = new Point { X = this.ClientSize.Width - linkItemAssistant.Width - 5, Y = margin };
                Controls.Add(linkItemAssistant);

                var labels = new[] {
                    "Is your stash full? Try Item Assistant!",
                    "Need a larger stash? Try Item Assistant!",
                    "Having trouble finding space for all your loot? Try Item Assistant!",
                    "Having trouble finding space for all your items? Try Item Assistant!",
                    "Need extra item storage? Try Item Assistant!",
                };
            }
#else
            linkItemAssistant.Hide();
#endif
        }

        private void JsPojoOnOnSave(object sender, EventArgs eventArgs) {

            if (InvokeRequired) {
                Invoke((MethodInvoker) delegate { JsPojoOnOnSave(sender, eventArgs); });
            }
            else {
                SaveParseArgument args = eventArgs as SaveParseArgument;
                
                var ofd = new SaveFileDialog {
                    InitialDirectory = GlobalSettings.SavedParsePath,
                    Filter = "Damage logs (*.dmg)|*.dmg|All files (*.*)|*.*"
                };

                if (ofd.ShowDialog() == DialogResult.OK) {
                    File.WriteAllText(ofd.FileName, args?.Data);
                }
            }
        }

        private void JsPojoOnOnSuggestLocationName(object sender, EventArgs eventArgs) {
            SuggestLocationNameArgument args = eventArgs as SuggestLocationNameArgument;
            if (!_nameSuggestionService.SuggestName(_positionTrackerService.PlayerPosition, args?.Suggestion)) {
                MessageBox.Show("Suggestion could not be sent due to an unknown error");
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            Logger.Debug("Starting..");
            if (Thread.CurrentThread.Name == null) {
                Thread.CurrentThread.Name = "UI";
                ExceptionReporter.EnableLogUnhandledOnThread();
            }

            this.Closing += Form1_Closing;

            {
                var webView = new WebView(_browser);
                webView.TopLevel = false;
                this.webViewPanel.Controls.Add(webView);
                webView.Show();
            }
            {
                var debugView = new DebugSettings(_appSettings);
                debugView.TopLevel = false;
                this.panelDebugView.Controls.Add(debugView);
                debugView.Show();
            }

            _messageProcessorCore.OnHookActivation += (_, __) => {
                this.labelHookStatus.Text = "Hook activated";
                this.labelHookStatus.ForeColor = System.Drawing.Color.Green;
            };

            _autoUpdateUtility.StartReportUsageTimer();

            this.FormClosing += OnFormClosing;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs) {
            _autoUpdateUtility.Dispose();
        }


        private void Form1_Closing(object sender, CancelEventArgs e) {
            _messageProcessorCore?.Dispose();
        }



        private void btnShowDevtools_Click(object sender, EventArgs e) {
            _browser.ShowDevTools();
        }


        

        private void TransferStatsToJson(object sender, EventArgs e) {
            var players = _statisticsService.GetPlayers();
            var pets = _statisticsService.GetPets();

            var damageDealt = new Dictionary<int, List<SimpleDamageEntryJson>>();
            var damageDealtToSingleTarget = new Dictionary<int, List<SimpleDamageEntryJson>>();
            var damageTaken = new Dictionary<int, List<SimpleDamageEntryJson>>();
            var detailedDamageTaken = new Dictionary<int, List<DetailedDamageTakenJson>>();
            var detailedDamageDealt = new Dictionary<int, List<DetailedDamageDealtJson>>();
            var damageBlocked = new Dictionary<int, List<DamageBlockedJson>>();
            foreach (var player in players) {
                damageDealt[player.Id] = _statisticsService.GetLatestDamageDealt(player.Id);
                damageTaken[player.Id] = _statisticsService.GetLatestDamageTaken(player.Id);
                damageDealtToSingleTarget[player.Id] = _statisticsService.GetLatestDamageDealtToSingleTarget(player.Id);
                detailedDamageTaken[player.Id] = _statisticsService.GetDetailedLatestDamageTaken(player.Id);
                detailedDamageDealt[player.Id] = _statisticsService.GetDetailedLatestDamageDealt(player.Id);
                damageBlocked[player.Id] = _statisticsService.GetDamageBlocked(player.Id);
            }
            foreach (var pet in pets) {
                damageDealt[pet.Id] = _statisticsService.GetLatestDamageDealt(pet.Id);
                damageTaken[pet.Id] = _statisticsService.GetLatestDamageTaken(pet.Id);
                damageDealtToSingleTarget[pet.Id] = _statisticsService.GetLatestDamageDealtToSingleTarget(pet.Id);
            }

            _browser.JsInteractor.SetEntities(_statisticsService.GetEntities());
            _browser.JsInteractor.SetPets(pets);
            _browser.JsInteractor.SetPlayers(players);
            _browser.JsInteractor.SetDamageBlocked(damageBlocked);
            _browser.JsInteractor.SetDamageDealt(damageDealt);
            _browser.JsInteractor.SetDamageDealtToSingleTarget(damageDealtToSingleTarget);
            _browser.JsInteractor.SetDamageTaken(damageTaken);
            _browser.JsInteractor.SetPlayerLocation(_positionTrackerService.GetPlayerLocation());
            _browser.JsInteractor.SetDetailedDamageTaken(detailedDamageTaken);
            _browser.JsInteractor.SetDetailedDamageDealt(detailedDamageDealt);
            
            _browser.NotifyUpdate();
        }

        

        private void linkDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://discord.gg/PJ87Ewa");
        }

        private void btnLoadSave_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog {
                InitialDirectory = GlobalSettings.SavedParsePath,
                Filter = "Damage logs (*.dmg)|*.dmg|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == DialogResult.OK) {
                if (File.Exists(ofd.FileName)) {
                    string data = File.ReadAllText(ofd.FileName);
                    _browser.TransferSave(data);
                }
                else {
                    MessageBox.Show(
                        "Could not find the file you requested",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
        }

        private void linkDonate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=29J3HM8G3CQSA");
        }

        private void linkItemAssistant_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://grimdawn.dreamcrash.org/ia/");
        }
    }
}
