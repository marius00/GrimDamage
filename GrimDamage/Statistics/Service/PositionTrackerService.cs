using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Statistics.model;
using log4net;

namespace GrimDamage.Statistics.Service {
    class PositionTrackerService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PositionTrackerService));
        // Crossroads -720355282, -1281368434, 1803970569
        // lower crossing 1063600470, -2110175027, 1746663183
        // lower crossing 2099529530, -1809042412, -1049290392
        //914181873, -1210726088, -685126223 hidden labo (warden), near rift
        //1013990166, -1912634668, -567970793 => (93,481, 1, 44,25755, 584712392) living quarters (warden)
        //2074428718, -1368476616, -198502410 => (127,4427, -4, 41,67922, 584715192) inner labo
        //-1643822716, -1092302023, 628224669 => (80,94996, 2,548401, 66,27589, 584710152) mr warden himself
        private readonly List<RecognizedPosition> _knownPositions = new List<RecognizedPosition> {
            new RecognizedPosition {
                Zone = -720355282,
                Name = "Crossroads"
            },
            new RecognizedPosition {
                Zone = 1063600470,
                Name = "Lower Crossing"
            },
            new RecognizedPosition {
                Zone = 2099529530,
                Name = "Lower Crossing"
            },
            new RecognizedPosition {
                Zone = 914181873,
                Name = "Hidden Laboratory"
            },
            new RecognizedPosition {
                Zone = 1013990166,
                Name = "Hidden Laboratory" // Living quarters
            },
            new RecognizedPosition {
                Zone = 2074428718,
                Name = "Hidden Laboratory" // Inner lab
            },
            new RecognizedPosition {
                Zone = -1643822716,
                Name = "Boss (The Warden)"
            },
        };

        public PlayerPosition PlayerPosition { get; private set; }

        public void SetPlayerPosition(PlayerPosition playerPosition) {
            PlayerPosition = playerPosition;
        }
        
        public string GetPlayerLocation() {
            if (PlayerPosition != null) {
                var loc = _knownPositions.FirstOrDefault(p => p.Zone == PlayerPosition.Zone);
                if (loc != null)
                    return loc.Name;

                //Logger.Debug($"Unknown location: {PlayerPosition.Zone}");
            }

            return "Unknown";
        }
    }
}
