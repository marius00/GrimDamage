using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Statistics.dto {
    public class DetailedDamageEntryJson {
        public int AttackerId { get; set; }
        public string DamageType { get; set; }
        public double Amount { get; set; }
    }
}
