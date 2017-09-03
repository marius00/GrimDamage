using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Config;
using GrimDamage.Tracking.Model;
using log4net;
using log4net.Repository.Hierarchy;

namespace GrimDamage.Parser.Service {
    class DamageParsingService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DamageParsingService));
        private readonly int _nameCacheDuration = 30;
        private string _defenderName;
        private string _attackerName;
        private int _attackerId;
        private readonly Dictionary<int, Entity> _names;

        public DamageParsingService() {
            this._names = new Dictionary<int, Entity>();
        }

        public void Cleanup() {
            var expired = _names.Values.Where(m => (DateTime.UtcNow - m.LastSeen).Minutes > _nameCacheDuration)
                .Select(m => m.Id)
                .ToList();

            foreach (var key in expired) {
                _names.Remove(key);
            }
        }

        public void ApplyDamage(double amount, int victim, string damageType) {
            var dmg = new DamageEntry {
                Amount = amount,
                Target = victim,
                Type = convertDamage(damageType),
                Time = DateTime.UtcNow
            };


            if (_names.ContainsKey(victim)) {
                _names[victim].DamageTaken.Add(dmg);
            }
            else {
                Logger.Warn($"Got a damage entry for victim {victim}, but the entity has not been previously stored");
            }

            if (_names.ContainsKey(_attackerId)) {
                _names[_attackerId].DamageDealt.Add(dmg);
            }
            else {
                Logger.Warn($"Got a damage entry for attacker {_attackerId}, but the entity has not been previously stored");
            }
        }

        public void ApplyLifeLeech(double chance, double amount) {
            
        }

        public void ApplyTotalDamage(double amount, double overTime) {

        }

        public void SetAttackerName(string name) {
            _attackerName = name;
        }
        public void SetAttackerId(int id) {
            if (!_names.ContainsKey(id)) {
                _names[id] = new Entity {
                    Id = id,
                    Name = _attackerName
                };
            }

            _attackerId = id;
        }

        public void SetDefenderName(string name) {
            _defenderName = name;
        }

        public void SetDefenderId(int id) {
            if (!_names.ContainsKey(id)) {
                _names[id] = new Entity {
                    Id = id,
                    Name = _defenderName
                };
            }
        }

        public void ApplyDeflect(double chance) {
            
        }

        public void SetAbsorb(double amount) {
            
        }

        public void ApplyReflect(double amount) {
            
        }

        public void ApplyBlock(double total, double percentile, double remaining) {
            
        }


        private DamageType convertDamage(string type) {
            if (EventMapping.DamageTypes.ContainsKey(type)) {
                return EventMapping.DamageTypes[type];
            }
            else {
                Logger.Warn($"Unknown damage type \"{type}\"");
                return DamageType.Unknown;
            }
        }
    }
}
