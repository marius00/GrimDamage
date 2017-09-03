using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons;
using EvilsoftCommons.DllInjector;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Forms;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using log4net;

namespace GrimDamage {
    public partial class Form1 : Form {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Form1));

        private Action<RegisterWindow.DataAndType> _registerWindowDelegate;
        private RegisterWindow _window;
        private InjectionHelper _injector;
        private ProgressChangedEventHandler _injectorCallbackDelegate;
        private bool _isFirstMessage = true;
        private readonly DamageParsingService _damageParsingService = new DamageParsingService();
        private readonly CefBrowserHandler _browser;

        private readonly List<string> _events = new List<string>();

        public Form1(CefBrowserHandler browser) {
            InitializeComponent();
            this._browser = browser;
        }

        private void Form1_Load(object sender, EventArgs e) {
            Logger.Debug("Starting..");
            _registerWindowDelegate = CustomWndProc;
            _window = new RegisterWindow("GDDamageWindowClass", _registerWindowDelegate);

            _injectorCallbackDelegate = InjectorCallback;
            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "Hook.dll");
            this.Closing += Form1_Closing;

            var webView = new WebView(_browser);
            webView.TopLevel = false;
            this.panel1.Controls.Add(webView);
            webView.Show();
        }

        private void Save() {
            string filename = Path.Combine(GlobalSettings.LogPath, DateTime.UtcNow.ToFileTime().ToString() + ".txt");
            File.WriteAllLines(filename, _events);
        }

        private void Form1_Closing(object sender, CancelEventArgs e) {
            _injector?.Dispose();
            _injector = null;

            Save();
        }

        private void AddEvent(string s) {
            //Logger.Debug(s);
            _events.Add(s);
        }

        private void CustomWndProc(RegisterWindow.DataAndType bt) {
            if (bt.Type == 45454) {
                string message = IOHelper.GetNullString(bt.Data, 0).Replace("\r", "").Replace("\n", "");
                //Logger.Debug("Stringy: " + message);
                AddEvent(message);
            }
            else if (bt.Type == 45001) {
                var dmg = IOHelper.GetDouble(bt.Data, 0);
                int victim = IOHelper.GetInt(bt.Data, 8);
                string dmgType = IOHelper.GetNullString(bt.Data, 12);
                AddEvent($"^y    Damage {dmg} to Defender 0x{victim} ({dmgType})");
                _damageParsingService.ApplyDamage(dmg, victim, dmgType);
            }
            else if (bt.Type == 45002) {
                var chance = IOHelper.GetDouble(bt.Data, 0);
                var healed = IOHelper.GetDouble(bt.Data, 4);
                AddEvent($"    ^b{chance}% Life Leech return {healed} Life");
                _damageParsingService.ApplyLifeLeech(chance, healed);
                
            }
            else if (bt.Type == 45003) {
                var amount = IOHelper.GetDouble(bt.Data, 0);
                var dot = IOHelper.GetDouble(bt.Data, 4);
                AddEvent($"    Total Damage:  Absolute ({amount}), Over Time ({dot})");
                _damageParsingService.ApplyTotalDamage(amount, dot);
            }
            else if (bt.Type == 45004) {
                string name = IOHelper.GetNullString(bt.Data, 0);
                AddEvent($"    attackerName = {name}");
                _damageParsingService.SetAttackerName(name);
            }
            else if (bt.Type == 45005) {
                string name = IOHelper.GetNullString(bt.Data, 0);
                AddEvent($"    defenderName = {name}");
                _damageParsingService.SetDefenderName(name);
            }
            else if (bt.Type == 45006) {
                int id = IOHelper.GetInt(bt.Data, 0);
                AddEvent($"    attackerID = {id}");
                _damageParsingService.SetAttackerId(id);
            }
            else if (bt.Type == 45007) {
                int id = IOHelper.GetInt(bt.Data, 0);
                AddEvent($"    defenderID = {id}");
                _damageParsingService.SetDefenderId(id);
            }
            else if (bt.Type == 45008) { // this is DEFLECT
                var chance = IOHelper.GetDouble(bt.Data, 0);
                AddEvent($"    ^yDeflect Projectile Chance ({chance}) caused prefix deflection");
                _damageParsingService.ApplyDeflect(chance);
            }
            else if (bt.Type == 45009) { // this is absorb
                var amount = IOHelper.GetDouble(bt.Data, 0);
                AddEvent($"    protectionAbsorption = {amount}");
                _damageParsingService.SetAbsorb(amount);
            }
            else if (bt.Type == 45010) {
                var amount = IOHelper.GetDouble(bt.Data, 0);
                AddEvent($"    ^str{amount}% Damage Reflected");
                _damageParsingService.ApplyReflect(amount);
            }
            else if (bt.Type == 45011) {
                var a = IOHelper.GetDouble(bt.Data, 0);
                var b = IOHelper.GetDouble(bt.Data, 4);
                var c = IOHelper.GetDouble(bt.Data, 8);
                AddEvent($"^bShield: Reduced ({a}) Damage by ({b}%) percent, remaining damage ({c})");
                _damageParsingService.ApplyBlock(a, b, c);
            }
            else if (bt.Type == 45000) {
                //Logger.Debug($"Unrecognized log: {IOHelper.GetNullString(bt.Data, 0)}");
            }

            else {
                Logger.Warn($"Got a message of type {bt.Type}");
            }

            if (_isFirstMessage) {
                Logger.Debug("Window message received");
                this.labelHookStatus.Text = "Hook activated";
                this.labelHookStatus.ForeColor = System.Drawing.Color.Green;
                _isFirstMessage = false;
            }
        }

        private void InjectorCallback(object sender, ProgressChangedEventArgs e) {
            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate { InjectorCallback(sender, e); });
            }
            else {
                //Logger.Debug("Injector callback");
            }
        }

        private void btnSave_Click(object sender, EventArgs e) {
            Save();
        }

        private void btnReadFile_Click(object sender, EventArgs e) {
            ParsingService parser = new ParsingService();
            string filename = Path.Combine(GlobalSettings.LogPath, "131482213376447842.txt");
            string[] dataset = File.ReadAllLines(filename);
            foreach (var entry in dataset) {
                parser.Parse(entry);
            }

        }
    }
}
