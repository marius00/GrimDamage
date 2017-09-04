using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    public class Entity {
        public Entity() {
            this.DamageDealt = new ConcurrentBag<DamageEntry>();
            this.DamageTaken = new ConcurrentBag<DamageEntry>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPlayer { get; set; }
        public ConcurrentBag<DamageEntry> DamageDealt { get; }
        public ConcurrentBag<DamageEntry> DamageTaken { get; }

        public DateTime LastSeen {
            get {
                var lastDealt = DamageDealt.DefaultIfEmpty().Max(m => m?.Time)?? DateTime.MinValue;
                var lastTaken = DamageTaken.DefaultIfEmpty().Max(m => m?.Time) ?? DateTime.MinValue;
                if (lastDealt > lastTaken)
                    return lastDealt;
                else {
                    return lastTaken;
                }
            }
        }
    }
}
