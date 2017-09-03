using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons;
using EvilsoftCommons.DllInjector;
using GrimDamage.Parser.Service;
using log4net;
using log4net.Repository.Hierarchy;

namespace GrimDamage.GD.Processors {
    class MessageProcessorCore : IDisposable {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MessageProcessorCore));
        private RegisterWindow _window;
        private InjectionHelper _injector;
        private ProgressChangedEventHandler _injectorCallbackDelegate;
        private bool _isFirstMessage = true;
        private readonly Action<RegisterWindow.DataAndType> _registerWindowDelegate;
        private readonly DamageParsingService _damageParsingService;

        public MessageProcessorCore(DamageParsingService damageParsingService) {
            _damageParsingService = damageParsingService;

            _registerWindowDelegate = CustomWndProc;
            _injectorCallbackDelegate = InjectorCallback;
            _window = new RegisterWindow("GDDamageWindowClass", _registerWindowDelegate);
            _injector = new InjectionHelper(new BackgroundWorker(), _injectorCallbackDelegate, false, "Grim Dawn", string.Empty, "Hook.dll");
        }


        private void AddEvent(string s) {
            //Logger.Debug(s);
            //_events.Add(s);
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
                //this.labelHookStatus.Text = "Hook activated";
                //this.labelHookStatus.ForeColor = System.Drawing.Color.Green;
                // TODO: Add feedback to the user
                _isFirstMessage = false;
            }
        }

        private void InjectorCallback(object sender, ProgressChangedEventArgs e) {
            //Logger.Debug("Injector callback");
        }

        public void Dispose() {
            _injector?.Dispose();
            _injector = null;
        }
    }
}
