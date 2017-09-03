using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    public class Entity {
        public Entity() {
            this.DamageDealt = new List<DamageEntry>();
            this.DamageTaken = new List<DamageEntry>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsPlayer { get; set; }
        public List<DamageEntry> DamageDealt { get; set; }
        public List<DamageEntry> DamageTaken { get; set; }

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
