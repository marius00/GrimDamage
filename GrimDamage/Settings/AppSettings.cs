using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Settings {
    public class AppSettings {
        public bool LogStateChanges { get; set; }
        public bool LogProcessedMessages { get; set; }
        public bool LogPlayerMovement { get; set; }
        public bool LogUnknownEvents { get; set; }
        public bool LogAllEvents { get; set; }
        public bool LogPlayerDetection { get; set; }
        public bool LogEntityHitpointEvent { get; set; }
    }
}
