using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Statistics.dto {
    public class DetailedDamageDealtJson {
        public int VictimId { get; set; }
        public string DamageType { get; set; }
        public double Amount { get; set; }
    }
}
