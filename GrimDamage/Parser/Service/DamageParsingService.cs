using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Config;
using GrimDamage.Tracking.Model;
using log4net;
using log4net.Repository.Hierarchy;

namespace GrimDamage.Parser.Service {
    public class DamageParsingService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DamageParsingService));
        private readonly EntityNamingService _entityNamingService;

        private const string PlayerPattern = "/pc/";
        private const string PetPattern0 = @"/playerclass";
        private const string PetPattern1 = @"/pets/";
        private readonly int _nameCacheDuration = 30;
        private string _defenderName;
        private string _attackerName;
        private int _attackerId;
        private int _defenderId;
        private int _primaryId;
        private readonly ConcurrentDictionary<int, Entity> _entities;

        public DamageParsingService() {
            _entityNamingService = new EntityNamingService();
            this._entities = new ConcurrentDictionary<int, Entity>();
        }

        public ICollection<Entity> Values => _entities.Values;

        public Entity GetEntity(int id) {
            if (_entities.ContainsKey(id))
                return _entities[id];
            else
                return null;
        }

        public void SetHealth(int id, float amount) {
            if (_entities.ContainsKey(id)) {
                _entities[id].Health = amount;
            }
        }

        // TODO: Call regularly, every minute or so.
        public void Cleanup() {
            var expired = _entities.Values.Where(m => (DateTime.UtcNow - m.LastSeen).Minutes > _nameCacheDuration)
                .Select(m => m.Id)
                .ToList();

            foreach (var key in expired) {
                Entity o;
                _entities.TryRemove(key, out o);
            }
        }

        public void SetPlayerInfo(int id, bool isPrimary) {
            if (isPrimary) {
                _primaryId = id;
            }

            if (_entities.ContainsKey(id))
                _entities[id].IsPrimary = isPrimary;
        }

        public void ApplyDamage(double amount, int victim, string damageType) {
            var dmg = new DamageDealtEntry {
                Amount = amount,
                Target = victim,
                Type = convertDamage(damageType),
                Time = DateTime.UtcNow
            };


            if (_entities.ContainsKey(victim)) {
                var taken = new DamageTakenEntry {
                    Amount = amount,
                    Attacker = _attackerId,
                    Type = convertDamage(damageType),
                    Time = DateTime.UtcNow
                };

                _entities[victim].DamageTaken.Add(taken);
            }
            else {
                Logger.Warn($"Got a damage entry for victim {victim}, but the entity has not been previously stored");
            }

            if (_entities.ContainsKey(_attackerId)) {
                _entities[_attackerId].DamageDealt.Add(dmg);
            }
            else {
                Logger.Warn($"Got a damage entry for attacker {_attackerId}, but the entity has not been previously stored");
            }
        }

        public void ApplyLifeLeech(double chance, double amount) {
            // Life leech is being removed soon, wasted effort to implement
        }

        public void ApplyTotalDamage(double amount, double overTime) {
            // Need to research more on what this data truly means
        }

        public void SetAttackerName(string name) {
            _attackerName = name;
        }
        public void SetAttackerId(int id) {
            if (!_entities.ContainsKey(id) && _attackerName != null) {
                _entities[id] = new Entity {
                    Id = id,
                    Name = _entityNamingService.GetName(_attackerName),
                    Type = Classify(_attackerName),
                    IsPrimary = id == _primaryId
                };
            }

            _attackerId = id;
        }

        public void SetDefenderName(string name) {
            _defenderName = name;
        }

        public void SetDefenderId(int id) {
            if (!_entities.ContainsKey(id) && _defenderName != null) {
                _entities[id] = new Entity {
                    Id = id,
                    Name = _entityNamingService.GetName(_defenderName),
                    Type = Classify(_defenderName),
                    IsPrimary = id == _primaryId
                };
            }

            _defenderId = id;
        }

        public void ApplyDeflect(double chance) {
            // Deflect is a "dodge ranged attack", the amount of damage that would have been done is not available
        }

        public void SetAbsorb(double amount) {
            
        }

        public void ApplyReflect(double amount) {
            
        }

        public void ApplyBlock(double total, double percentile, double remaining) {


            if (_entities.ContainsKey(_defenderId)) {
                var blocked = new DamageBlockedEntry {
                    Amount = total,
                    Attacker = _attackerId,
                    Time = DateTime.UtcNow
                };

                _entities[_defenderId].DamageBlocked.Add(blocked);
            }
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

        private EntityType Classify(string record) {
            if (record.Contains(PlayerPattern)) {
                return EntityType.Player;
            }
            else if (record.Contains(PetPattern0) && record.Contains(PetPattern1)) {
                return EntityType.Pet;
            }
            else if (record.Contains("/nemesis/")) {
                return EntityType.Nemsis;
            }
            else if (record.Contains("/hero/")) {
                return EntityType.Hero;
            }
            else if (record.Contains("/boss&quest/")) {
                return EntityType.Boss;
            }
            else if (record.Contains("/bounties/")) {
                return EntityType.Bounty;
            }
            else { 
                return EntityType.Monster;
            }
        }
    }
}
