using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Statistics.dto {
    public class ResistEntryJson {
        public int EntityId { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public long Timestamp { get; set; }
    }
}
