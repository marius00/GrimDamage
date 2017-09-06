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

        private readonly List<RecognizedPosition> _knownPositions = new List<RecognizedPosition> {
            new RecognizedPosition {
                Zone = 0x1A1E89C0,
                Name = "Crossroads"
            }
        };

        public PlayerPosition PlayerPosition { get; private set; }

        public void SetPlayerPosition(PlayerPosition playerPosition) {
            PlayerPosition = playerPosition;
            Logger.Debug(playerPosition.ToString());
        }
        
        public string GetPlayerLocation() {
            if (PlayerPosition != null) {
                var loc = _knownPositions.FirstOrDefault(p => p.Zone == PlayerPosition.Zone);
                if (loc != null)
                    return loc.Name;
            }

            return "Unknown";
        }
    }
}
