using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    class Entity {
        public bool IsPlayer { get; set; }
        public List<DamageEntry> DamageDealt { get; set; }
        public List<DamageEntry> DamageTaken { get; set; }
    }
}
