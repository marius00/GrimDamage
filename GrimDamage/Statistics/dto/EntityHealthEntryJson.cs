using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Statistics.dto {
    public class EntityHealthEntryJson {
        public int Id { get; set; }
        public long Timestamp { get; set; }
        public float Amount { get; set; }
    }
}
