using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrimDamage.GD.Logger {
    public class CombatFileLogger {

        private DateTime _loggingStartTime;
        private bool _loggingEnabled;
        private List<string> _events = new List<string>();

        public bool Enabled {
            get {
                return _loggingEnabled;
            }
            set {
                _loggingEnabled = value;
                _loggingStartTime = DateTime.UtcNow;
            }
        }


        public void AddEvent(string s) {
            if (Enabled) {
                int idx = (int)(DateTime.UtcNow - _loggingStartTime).TotalSeconds;
                _events.Add($"{idx} {s}");
            }
        }

        public void WriteToFile(string file) {
            File.WriteAllLines(file, _events);
        }
    }
}
