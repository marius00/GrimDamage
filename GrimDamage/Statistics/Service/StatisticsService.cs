using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Service;
using GrimDamage.Statistics.dto;
using GrimDamage.Tracking.Model;
using GrimDamage.Utility;

namespace GrimDamage.Statistics.Service {
    public class StatisticsService {
        private DateTime _lastUpdateTimeDamageDealt = DateTime.UtcNow;
        private readonly DamageParsingService _damageParsingService;

        public StatisticsService(DamageParsingService damageParsingService) {
            _damageParsingService = damageParsingService;
        }

        private EntityJson toJson(Entity m) {
            return new EntityJson {
                Id = m.Id,
                Name = m.Name,
                IsPrimary = m.IsPrimary,
                Type = m.Type.ToString()
            };
        }

        public List<EntityJson> GetEntities() {
            return _damageParsingService.Values.Select(toJson).ToList();
        }

        public List<EntityJson> GetPlayers() {
            return _damageParsingService.Values.Where(entity => entity.Type == EntityType.Player).Select(toJson).ToList();
        }

        private static List<SimpleDamageEntryJson> Normalize(List<SimpleDamageEntryJson> entries) {
            foreach (DamageType type in Enum.GetValues(typeof(DamageType))) {
                if (type != DamageType.Unknown) {
                    if (!entries.Exists(m => m.DamageType == type.ToString())) {
                        entries.Add(new SimpleDamageEntryJson {
                            Amount = 0,
                            DamageType = type.ToString()
                        });
                    }
                }
            }

            entries.Add(new SimpleDamageEntryJson {
                Amount = entries.Sum(m => m.Amount),
                DamageType = "Total"
            });

            return entries;
        }

        public List<SimpleDamageEntryJson> GetSimpleDamageTaken(int playerId, long start, long end) {
            var player = _damageParsingService.GetEntity(playerId);

            if (player == null || player.DamageTaken.Count == 0) {
                return Normalize(new List<SimpleDamageEntryJson>());
            }
            else {
                var from = Timestamp.ToDateTimeFromMilliseconds(start);
                var to = Timestamp.ToDateTimeFromMilliseconds(end);
                var result = player.DamageTaken
                    .Where(dmg => dmg.Time > from && dmg.Time < to)
                    .GroupBy(m => m.Type)
                    .Select(m => new SimpleDamageEntryJson {
                        DamageType = m.Key.ToString(),
                        Amount = m.Sum(s => s.Amount)
                    })
                    .ToList();

                return Normalize(result);
            }
        }
        
        public List<EntityHealthEntryJson> GetEntityHealth(int entityId, long start, long end) {
            var player = _damageParsingService.GetEntity(entityId);

            if (player == null || player.Health.Count == 0) {
                return new List<EntityHealthEntryJson>();
            }
            else {
                var result = player.Health
                    .Where(entry => entry.Timestamp > start && entry.Timestamp < end)
                    .Select(m => new EntityHealthEntryJson {
                        Amount = m.Health,
                        Timestamp = m.Timestamp,
                        Id = entityId
                    })
                    .ToList();

                return result;
            }
        }
        public List<SimpleDamageEntryJson> GetSimpleDamageDealt(int playerId, long start, long end) {
            var player = _damageParsingService.GetEntity(playerId);

            if (player == null || player.DamageDealt.Count == 0) {
                return Normalize(new List<SimpleDamageEntryJson>());
            }
            else {
                var from = Timestamp.ToDateTimeFromMilliseconds(start);
                var to = Timestamp.ToDateTimeFromMilliseconds(end);
                var result = player.DamageDealt
                    .Where(dmg => dmg.Time > from && dmg.Time < to)
                    .GroupBy(m => m.Type)
                    .Select(m => new SimpleDamageEntryJson {
                        DamageType = m.Key.ToString(),
                        Amount = m.Sum(s => s.Amount)
                    })
                    .ToList();

                return Normalize(result);
            }
        }

        public List<DetailedDamageDealtJson> GetDetailedLatestDamageDealt(int playerId, long start, long end) {
            var player = _damageParsingService.GetEntity(playerId);
            if (player == null || player.DamageDealt.Count == 0) {
                return new List<DetailedDamageDealtJson>();
            }
            else {
                var from = Timestamp.ToDateTimeFromMilliseconds(start);
                var to = Timestamp.ToDateTimeFromMilliseconds(end);
                var result = player.DamageDealt
                    .Where(dmg => dmg.Time > from && dmg.Time < to)
                    .Select(m => new DetailedDamageDealtJson {
                        VictimId = m.Target,
                        DamageType = m.Type.ToString(),
                        Amount = m.Amount,
                        Timestamp = Timestamp.ToUtcMilliseconds(m.Time)
                    })
                    .ToList();

                return result;
            }
        }
        
        public List<ResistEntryJson> GetResists(int entityId, long start, long end) {
            var entity = _damageParsingService.GetEntity(entityId);
            if (entity == null || entity.Resists.Count == 0) {
                return new List<ResistEntryJson>();
            }
            else {
                var from = Timestamp.ToDateTimeFromMilliseconds(start);
                var to = Timestamp.ToDateTimeFromMilliseconds(end);
                var result = entity.Resists
                    .Where(dmg => dmg.Time > from && dmg.Time < to)
                    .Select(m => new ResistEntryJson {
                        EntityId = entityId,
                        Type = m.Type.ToString(),
                        Amount = m.Amount,
                        Timestamp = Timestamp.ToUtcMilliseconds(m.Time)
                    })
                    .ToList();

                return result;
            }
        }


        public List<DetailedDamageTakenJson> GetDetailedLatestDamageTaken(int playerId, long start, long end) {
            var player = _damageParsingService.GetEntity(playerId);
            if (player == null || player.DamageTaken.Count == 0) {
                return new List<DetailedDamageTakenJson>();
            }
            else {
                var from = Timestamp.ToDateTimeFromMilliseconds(start);
                var to = Timestamp.ToDateTimeFromMilliseconds(end);
                var result = player.DamageTaken
                    .Where(dmg => dmg.Time > from && dmg.Time < to)
                    .Select(m => new DetailedDamageTakenJson {
                        AttackerId = m.Attacker,
                        DamageType = m.Type.ToString(),
                        Amount = m.Amount,
                        Timestamp = Timestamp.ToUtcMilliseconds(m.Time)
                    })
                    .ToList();

                return result;
            }
        }

        [Obsolete]
        public List<SimpleDamageEntryJson> GetLatestDamageDealt(int playerId) {
            var player = _damageParsingService.GetEntity(playerId);

            if (player == null || player.DamageDealt.Count == 0) {
                return Normalize(new List<SimpleDamageEntryJson>());
            }
            else {
                var result = player.DamageDealt
                    .Where(dmg => dmg.Time > _lastUpdateTimeDamageDealt)
                    .GroupBy(m => m.Type)
                    .Select(m => new SimpleDamageEntryJson {
                        DamageType = m.Key.ToString(),
                        Amount = m.Sum(s => s.Amount)
                    })
                    .ToList();

                _lastUpdateTimeDamageDealt = player.DamageDealt.Max(m => m.Time);
                return Normalize(result);
            }
        }
    }
}
