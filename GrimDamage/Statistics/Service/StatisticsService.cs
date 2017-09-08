using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Service;
using GrimDamage.Statistics.dto;
using GrimDamage.Tracking.Model;

namespace GrimDamage.Statistics.Service {
    public class StatisticsService {
        private DateTime _lastUpdateTimeDamageDealt = DateTime.UtcNow;
        private DateTime _lastUpdateTimeDamageDealtSingleTarget = DateTime.UtcNow;
        private DateTime _lastUpdateTimeDamageTaken = DateTime.UtcNow;
        private readonly DamageParsingService _damageParsingService;

        public StatisticsService(DamageParsingService damageParsingService) {
            _damageParsingService = damageParsingService;
        }

        public List<EntityJson> GetPlayers() {
            return _damageParsingService.Values.Where(entity => entity.Type == EntityType.Player)
                .Select(m => new EntityJson {
                    Id = m.Id,
                    Name = m.Name,
                    IsPrimary = m.IsPrimary
                })
                .ToList();
        }

        public List<EntityJson> GetPets() {
            return _damageParsingService.Values.Where(entity => entity.Type == EntityType.Pet)
                .Select(m => new EntityJson {
                    Id = m.Id,
                    Name = m.Name,
                    IsPrimary = false
                })
                .ToList();
        }

        private List<DamageEntryJson> Normalize(List<DamageEntryJson> entries) {
            foreach (DamageType type in Enum.GetValues(typeof(DamageType))) {
                if (type != DamageType.Unknown) {
                    if (!entries.Exists(m => m.DamageType == type.ToString())) {
                        entries.Add(new DamageEntryJson {
                            Amount = 0,
                            DamageType = type.ToString()
                        });
                    }
                }
            }

            entries.Add(new DamageEntryJson {
                Amount = entries.Sum(m => m.Amount),
                DamageType = "Total"
            });

            return entries;
        }

        public List<DetailedDamageEntryJson> GetDetailedLatestDamageTaken(int playerId) {
            var player = _damageParsingService.GetEntity(playerId);
            if (player == null || player.DamageTaken.Count == 0) {
                return new List<DetailedDamageEntryJson>();
            }
            else {
                var result = player.DamageTaken
                    .Where(dmg => dmg.Time > _lastUpdateTimeDamageTaken)
                    .Select(m => new DetailedDamageEntryJson {
                        Attacker = _damageParsingService.GetEntity(m.Attacker)?.Name ?? "Unknown",
                        DamageType = m.Type.ToString(),
                        Amount = m.Amount
                    })
                    .ToList();

                _lastUpdateTimeDamageTaken = player.DamageTaken.Max(m => m.Time);
                return result;
            }
        }

        public List<DamageEntryJson> GetLatestDamageTaken(int playerId) {
            var player = _damageParsingService.GetEntity(playerId);

            if (player == null || player.DamageTaken.Count == 0) {
                return Normalize(new List<DamageEntryJson>());
            }
            else {
                var result = player.DamageTaken
                    .Where(dmg => dmg.Time > _lastUpdateTimeDamageTaken)
                    .GroupBy(m => m.Type)
                    .Select(m => new DamageEntryJson {
                        DamageType = m.Key.ToString(),
                        Amount = m.Sum(s => s.Amount)
                    })
                    .ToList();

                _lastUpdateTimeDamageTaken = player.DamageTaken.Max(m => m.Time);
                return Normalize(result);
            }
        }

        public List<DamageEntryJson> GetLatestDamageDealt(int playerId) {
            var player = _damageParsingService.GetEntity(playerId);

            if (player == null || player.DamageDealt.Count == 0) {
                return Normalize(new List<DamageEntryJson>());
            }
            else {
                var result = player.DamageDealt
                    .Where(dmg => dmg.Time > _lastUpdateTimeDamageDealt)
                    .GroupBy(m => m.Type)
                    .Select(m => new DamageEntryJson {
                        DamageType = m.Key.ToString(),
                        Amount = m.Sum(s => s.Amount)
                    })
                    .ToList();

                _lastUpdateTimeDamageDealt = player.DamageDealt.Max(m => m.Time);
                return Normalize(result);
            }
        }
        
        public List<DamageEntryJson> GetLatestDamageDealtToSingleTarget(int playerId) {
            var player = _damageParsingService.GetEntity(playerId);

            if (player == null || player.DamageDealt.Count == 0) {
                return Normalize(new List<DamageEntryJson>());
            }
            else {
                var result = player.DamageDealt
                    .Where(dmg => dmg.Time > _lastUpdateTimeDamageDealtSingleTarget)
                    .GroupBy(m => m.Target)
                    .OrderByDescending(m => m.Sum(e => e.Amount))
                    .FirstOrDefault()
                    ?.GroupBy(m => m.Type)
                    .Select(m => new DamageEntryJson {
                        DamageType = m.Key.ToString(),
                        Amount = m.Sum(s => s.Amount)
                        }
                    )
                    .ToList();

                _lastUpdateTimeDamageDealtSingleTarget = player.DamageDealt.Max(m => m.Time);

                if (result == null)
                    return Normalize(new List<DamageEntryJson>());

                return Normalize(result);
            }
        }
    }
}
