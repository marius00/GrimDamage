using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons;
using GrimDamage.GD.Dto;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using log4net;

namespace GrimDamage.GD.Processors {
    class GdLogMessageProcessor : IMessageProcessor {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IMessageProcessor));
        private readonly AppSettings _appSettings;
        private readonly DamageParsingService _damageParsingService;
        
        public GdLogMessageProcessor(AppSettings appSettings, DamageParsingService damageParsingService) {
            _appSettings = appSettings;
            _damageParsingService = damageParsingService;
        }

        private void AddEvent(string s) {
            if (_appSettings.LogAllEvents) {
                Logger.Debug(s);
            }
        }

        public bool Process(MessageType type, byte[] data) {
            if (type == MessageType.DamageToVictim) {
                var dmg = IOHelper.GetDouble(data, 0);
                int victim = IOHelper.GetInt(data, 8);
                string dmgType = IOHelper.GetNullString(data, 12);
                AddEvent($"^y    Damage {dmg} to Defender 0x{victim} ({dmgType})");
                _damageParsingService.ApplyDamage(dmg, victim, dmgType);
            }
            else if (type == MessageType.LifeLeech) {
                var chance = IOHelper.GetDouble(data, 0);
                var healed = IOHelper.GetDouble(data, 4);
                AddEvent($"    ^b{chance}% Life Leech return {healed} Life");
                _damageParsingService.ApplyLifeLeech(chance, healed);

            }
            else if (type == MessageType.TotalDamage) {
                var amount = IOHelper.GetDouble(data, 0);
                var dot = IOHelper.GetDouble(data, 4);
                AddEvent($"    Total Damage:  Absolute ({amount}), Over Time ({dot})");
                _damageParsingService.ApplyTotalDamage(amount, dot);
            }
            else if (type == MessageType.SetAttackerName) {
                string name = IOHelper.GetNullString(data, 0);
                AddEvent($"    attackerName = {name}");
                _damageParsingService.SetAttackerName(name);
            }
            else if (type == MessageType.SetDefenderName) {
                string name = IOHelper.GetNullString(data, 0);
                AddEvent($"    defenderName = {name}");
                _damageParsingService.SetDefenderName(name);
            }
            else if (type == MessageType.SetAttackerId) {
                int id = IOHelper.GetInt(data, 0);
                AddEvent($"    attackerID = {id}");
                _damageParsingService.SetAttackerId(id);
            }
            else if (type == MessageType.SetDefenderId) {
                int id = IOHelper.GetInt(data, 0);
                AddEvent($"    defenderID = {id}");
                _damageParsingService.SetDefenderId(id);
            }
            else if (type == MessageType.Deflect) { // this is DEFLECT
                var chance = IOHelper.GetDouble(data, 0);
                AddEvent($"    ^yDeflect Projectile Chance ({chance}) caused prefix deflection");
                _damageParsingService.ApplyDeflect(chance);
            }
            else if (type == MessageType.Absorb) { // this is absorb
                var amount = IOHelper.GetDouble(data, 0);
                AddEvent($"    protectionAbsorption = {amount}");
                _damageParsingService.SetAbsorb(amount);
            }
            else if (type == MessageType.Reflect) {
                var amount = IOHelper.GetDouble(data, 0);
                AddEvent($"    ^str{amount}% Damage Reflected");
                _damageParsingService.ApplyReflect(amount);
            }
            else if (type == MessageType.Block) {
                var a = IOHelper.GetDouble(data, 0);
                var b = IOHelper.GetDouble(data, 4);
                var c = IOHelper.GetDouble(data, 8);
                AddEvent($"^bShield: Reduced ({a}) Damage by ({b}%) percent, remaining damage ({c})");
                _damageParsingService.ApplyBlock(a, b, c);
            }
            else if (type == MessageType.EndCombat) {
                _damageParsingService.RemoveAttackerId();
            }
            else if (type == MessageType.LogUnrecognized) {
                if (_appSettings.LogUnknownEvents) {
                    Logger.Debug($"Unrecognized log: {IOHelper.GetNullString(data, 0)}");
                }
            }
            else {
                return false;
            }

            return true;
        }
    }
}
