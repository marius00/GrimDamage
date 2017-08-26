using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDawnTestListener.Tracking.Model {
    class Entity {
        public bool IsPlayer { get; set; }
        public List<DamageType> DamageDealt { get; set; }
        public List<DamageType> DamageTaken { get; set; }
    }
}
