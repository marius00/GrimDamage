using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Config;
using GrimDamage.Tracking.Model;
using GrimDamage.Utility;
using log4net;
using log4net.Repository.Hierarchy;

namespace GrimDamage.Parser.Service {
    public class DamageParsingService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DamageParsingService));
        private readonly EntityNamingService _entityNamingService;


        static class Pattern {
            public const string Nemesis = "/nemesis/";
            public const string Hero = "/hero/";
            public const string Boss = "/boss&quest/";
            public const string Bounties = "/bounties/";
            public const string Player = "/pc/";
            public const string Pet0 = @"/playerclass";
            public const string Pet1 = @"/pets/";
        }

        private const int NameCacheDuration = 30;
        private string _defenderName;
        private string _attackerName;
        private int _attackerId;
        private int _defenderId;
        private int _primaryId;
        private readonly ConcurrentDictionary<int, Entity> _entities;

        public DamageParsingService() {
            _entityNamingService = new EntityNamingService();
            this._entities = new ConcurrentDictionary<int, Entity>();

            _entities[0] = new Entity {
                Id = 0,
                Name = "Environmental",
                Type = EntityType.Environmental
            };
        }

        public ICollection<Entity> Values => _entities.Values;

        public Entity GetEntity(int id) {
            if (_entities.ContainsKey(id))
                return _entities[id];
            else
                return null;
        }

        public void SetHealth(int id, float amount) {
            var entity = GetOrCreate(id);
            entity.Health.Add(new EntityHealthEntry {
                Health = amount,
                Timestamp = Timestamp.UTCMillisecondsNow
            });
        }

        // TODO: Call regularly, every minute or so.
        public void Cleanup() {
            var expired = _entities.Values.Where(m => (DateTime.UtcNow - m.LastSeen).Minutes > NameCacheDuration)
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

            // TODO: Unset any other primary?
            foreach (var key in _entities.Keys) {
                _entities[key].IsPrimary = key == id;
            }

            var entity = GetOrCreate(id);
            entity.IsPrimary = true;
        }

        private Entity GetOrCreate(int entityId) {
            if (!_entities.ContainsKey(entityId)) {
                _entities[entityId] = new Entity {
                    Id = entityId,
                    IsPrimary = entityId == _primaryId
                };
            }


            return _entities[entityId];
            
        }

        public void ApplyDamage(double amount, int victimId, string damageType) {
            var dmg = new DamageDealtEntry {
                Amount = amount,
                Target = victimId,
                Type = convertDamage(damageType),
                Time = DateTime.UtcNow
            };

            var victim = GetOrCreate(victimId);
            var taken = new DamageTakenEntry {
                Amount = amount,
                Attacker = _attackerId,
                Type = convertDamage(damageType),
                Time = DateTime.UtcNow
            };
            victim.DamageTaken.Add(taken);

            var attacker = GetOrCreate(_attackerId);
            attacker.DamageDealt.Add(dmg);
            
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


        /// <summary>
        /// At the end of a combat this may be called
        /// Damage entries taken between combat sessions has no attacker and usually refer to environmental damage
        /// </summary>
        public void RemoveAttackerId() {
            _attackerId = 0;
        }

        public void SetAttackerId(int id) {
            if (!string.IsNullOrEmpty(_attackerName)) {
                var entity = GetOrCreate(id);
                if (string.IsNullOrEmpty(entity.Name)) {
                    entity.Name = _entityNamingService.GetName(_attackerName);
                    entity.Type = Classify(_attackerName);
                }
            }

            _attackerId = id;
        }

        public void SetDefenderName(string name) {
            _defenderName = name;
        }

        public void SetDefenderId(int id) {
            if (!string.IsNullOrEmpty(_defenderName)) {
                var entity = GetOrCreate(id);
                if (string.IsNullOrEmpty(entity.Name)) {
                    entity.Name = _entityNamingService.GetName(_defenderName);
                    entity.Type = Classify(_defenderName);
                }
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
            var entity = GetOrCreate(_defenderId);
            var blocked = new DamageBlockedEntry {
                Amount = total,
                Attacker = _attackerId,
                Time = DateTime.UtcNow
            };

            entity.DamageBlocked.Add(blocked);
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
            if (record.Contains(Pattern.Player)) {
                return EntityType.Player;
            }
            else if (record.Contains(Pattern.Pet0) && record.Contains(Pattern.Pet1)) {
                return EntityType.Pet;
            }
            else if (record.Contains(Pattern.Nemesis)) {
                return EntityType.Nemsis;
            }
            else if (record.Contains(Pattern.Hero)) {
                return EntityType.Hero;
            }
            else if (record.Contains(Pattern.Boss)) {
                return EntityType.Boss;
            }
            else if (record.Contains(Pattern.Bounties)) {
                return EntityType.Bounty;
            }
            else { 
                return EntityType.Monster;
            }
        }
    }
}
