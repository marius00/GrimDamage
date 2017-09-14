using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    public class Entity {
        public Entity() {
            this.DamageDealt = new ConcurrentBag<DamageDealtEntry>();
            this.DamageTaken = new ConcurrentBag<DamageTakenEntry>();
        }



        public int Id { get; set; }

        public bool IsPrimary { get; set; }

        public string Name { get; set; }

        public EntityType Type { get; set; }

        public float Health { get; set; }

        public ConcurrentBag<DamageDealtEntry> DamageDealt { get; }
        public ConcurrentBag<DamageTakenEntry> DamageTaken { get; }

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
