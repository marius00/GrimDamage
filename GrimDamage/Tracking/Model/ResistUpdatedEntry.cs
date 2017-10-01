using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Tracking.Model {
    public class ResistUpdatedEntry {
        public double Amount { get; set; }
        public ResistType Type { get; set; }
        public DateTime Time { get; set; }
    }
}
