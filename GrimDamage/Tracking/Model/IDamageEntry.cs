using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    interface IDamageEntry {
        double Amount { get; }
        DamageType Type { get; }
        DateTime Time { get; }
    }
}
