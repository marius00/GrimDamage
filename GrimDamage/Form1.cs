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

        private List<string> events = new List<string>();

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            Logger.Debug("Starting..");
            _registerWindowDelegate = CustomWndProc;
            _window = new RegisterWindow("GDDamageWindowClass", _registerWindowDelegate);

            _injectorCallbackDelegate = InjectorCallback;
            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "Hook.dll");
            this.Closing += Form1_Closing;
        }

        private void Save() {
            string filename = Path.Combine(GlobalSettings.LogPath, DateTime.UtcNow.ToFileTime().ToString() + ".txt");
            File.WriteAllLines(filename, events);
        }

        private void Form1_Closing(object sender, CancelEventArgs e) {
            _injector?.Dispose();
            _injector = null;

            Save();
        }

        private void AddEvent(string s) {
            Logger.Debug(s);
            events.Add(s);
        }

        private void CustomWndProc(RegisterWindow.DataAndType bt) {
            if (bt.Type == 45454) {
                string message = IOHelper.GetNullString(bt.Data, 0).Replace("\r", "").Replace("\n", "");
                //Logger.Debug("Stringy: " + message);
                AddEvent(message);
            }
            else if (bt.Type == 45001) {
                float dmg = IOHelper.GetFloat(bt.Data, 0);
                int victim = IOHelper.GetInt(bt.Data, 4);
                string dmgType = IOHelper.GetNullString(bt.Data, 8);
                AddEvent($"^y    Damage {dmg} to Defender 0x{victim} ({dmgType})");
            }
            else if (bt.Type == 45002) {
                float chance = IOHelper.GetFloat(bt.Data, 0);
                float healed = IOHelper.GetFloat(bt.Data, 4);
                AddEvent($"    ^b{chance}% Life Leech return {healed} Life");
            }
            else if (bt.Type == 45003) {
                float chance = IOHelper.GetFloat(bt.Data, 0);
                float healed = IOHelper.GetFloat(bt.Data, 4);
                AddEvent($"    Total Damage:  Absolute ({chance}), Over Time ({healed})");
            }
            else if (bt.Type == 45004) {
                string name = IOHelper.GetNullString(bt.Data, 0);
                AddEvent($"    attackerName = {name}");
            }
            else if (bt.Type == 45005) {
                string name = IOHelper.GetNullString(bt.Data, 0);
                AddEvent($"    defenderName = {name}");
            }
            else if (bt.Type == 45006) {
                int id = IOHelper.GetInt(bt.Data, 0);
                AddEvent($"    attackerID = {id}");
            }
            else if (bt.Type == 45007) {
                int id = IOHelper.GetInt(bt.Data, 0);
                AddEvent($"    defenderID = {id}");
            }
            else if (bt.Type == 45008) { // this is DEFLECT
                float id = IOHelper.GetFloat(bt.Data, 0);
                AddEvent($"    ^yDeflect Projectile Chance ({id}) caused prefix deflection");
            }
            else if (bt.Type == 45009) { // this is absorb
                var id = IOHelper.GetLong(bt.Data, 0);
                var id2 = IOHelper.GetDouble(bt.Data, 0);
                var id3 = IOHelper.GetFloat(bt.Data, 0);
                //var id4 = IOHelper.GetFloat(bt.Data, 4);
                //var id5 = IOHelper.GetDouble(bt.Data, 4);
                //var id6 = IOHelper.GetDouble(bt.Data, 8);
                var id7 = IOHelper.GetNullString(bt.Data, 0);
                //AddEvent($"    protectionAbsorption = {id7} {id} {id2} {id3} {id4} {id5} {id6}");
                AddEvent($"    protectionAbsorption = {id7} {id} {id2} {id3}");
            }
            else if (bt.Type == 45010) {
                var id = IOHelper.GetNullString(bt.Data, 0);
                AddEvent($"    ^str{id}% Damage Reflected");
            }
            else if (bt.Type == 45011) {
                float a = IOHelper.GetFloat(bt.Data, 0);
                float b = IOHelper.GetFloat(bt.Data, 4);
                float c = IOHelper.GetFloat(bt.Data, 8);
                AddEvent($"^bShield: Reduced ({a}) Damage by ({b}%) percent, remaining damage ({c})");
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
