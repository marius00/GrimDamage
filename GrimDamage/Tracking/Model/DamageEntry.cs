using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    public class DamageEntry {
        public int Target { get; set; }
        public double Amount { get; set; }
        public DamageType Type { get; set; }
        public DateTime Time { get; set; }

    }
}
