using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.Statistics.model {
    class PlayerPosition {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public int Zone { get; set; }

        public override string ToString() {
            return $"PlayerPosition[({X},{Y},{Z}),{Zone:X}]";
        }
    }
}
