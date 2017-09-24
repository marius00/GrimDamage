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
            //
            
            new RecognizedPosition {
                Zone = -1460778184,
                Name = "Homestead"
            },
            new RecognizedPosition {
                Zone = 2045987642,
                Name = "Sorrows Bastion"
            },
            new RecognizedPosition {
                Zone = 1878609095,
                Name = "Kymon's Sanctuary"
            },
            new RecognizedPosition {
                Zone = -1813626772,
                Name = "Fort Ikon"
            },
            new RecognizedPosition {
                Zone = -1919595015,
                Name = "Fort Ikon Prison"
            },
            new RecognizedPosition {
                Zone = 450184872,
                Name = "Gates of Necropolis"
            },
            new RecognizedPosition {
                Zone = 1563117993,
                Name = "Gates of Necropolis"
            },
            new RecognizedPosition {
                Zone = -1258075253,
                Name = "Gates of Necropolis"
            },
            new RecognizedPosition {
                Zone = -651869254,
                Name = "Gates of Necropolis"
            },
            new RecognizedPosition {
                Zone = 1613778671,
                Name = "Gates of Necropolis"
            },
            new RecognizedPosition {
                Zone = -326480122,
                Name = "Gates of Necropolis"
            },
            new RecognizedPosition {
                Zone = -1955509690,
                Name = "Plains of Strife"
            },
            new RecognizedPosition {
                Zone = -806730780,
                Name = "The Black Sepulcher"
            },
            new RecognizedPosition {
                Zone = -1660598104,
                Name = "Necropolis Interior"
            },
            new RecognizedPosition {
                Zone = 1756316616,
                Name = "Necropolis Interior"
            },
            new RecognizedPosition {
                Zone = -1252309047,
                Name = "Tomb of the watchers (Necropolis)"
            },
            new RecognizedPosition {
                Zone = -1316927244,
                Name = "Darkvale Gate"
            },


            new RecognizedPosition {
                Zone = 1018840070,
                Name = "Astakarn Road"
            },
            new RecognizedPosition {
                Zone = -848277937,
                Name = "Astakarn Road Rift"
            },
            new RecognizedPosition {
                Zone = -330478143,
                Name = "Astakarn Road Rift"
            },
            new RecognizedPosition {
                Zone = 1359564347,
                Name = "Astakarn Valley"
            },
            new RecognizedPosition {
                Zone = 619267388,
                Name = "Rotting Croplands"
            },
            new RecognizedPosition {
                Zone = -1843310828,
                Name = "Tomb of Korvaak"
            },
            new RecognizedPosition {
                Zone = -1373552043,
                Name = "Tomb of Korvaak"
            },
            new RecognizedPosition {
                Zone = -349223825,
                Name = "Tomb of Korvaak"
            },
            new RecognizedPosition {
                Zone = -1679340999,
                Name = "Tomb of Korvaak"
            },
            new RecognizedPosition {
                Zone = 196166454,
                Name = "Wardens Cellar"
            },
            new RecognizedPosition {
                Zone = 1792298421,
                Name = "Underground Transit (Warden #2)"
            },
            new RecognizedPosition {
                Zone = 1191657842,
                Name = "Burrowitch Village"
            },
            new RecognizedPosition {
                Zone = 954744927,
                Name = "Moldering Fields (Burrowitch Village)"
            },
            new RecognizedPosition {
                Zone = 1623802494,
                Name = "Gutworm (Boss)"
            },
            new RecognizedPosition {
                Zone = -2035265569,
                Name = "Cronley's Hideout"
            },
            new RecognizedPosition {
                Zone = 1434796673,
                Name = "Cronley's Hideout"
            },
            new RecognizedPosition {
                Zone = -873773682,
                Name = "Cronley's Hideout"
            },
            new RecognizedPosition {
                Zone = 1863730804,
                Name = "Cronley's Hideout"
            },
            new RecognizedPosition {
                Zone = -132952783,
                Name = "Cronley's Hideout"
            },
            new RecognizedPosition {
                Zone = -810203939,
                Name = "Cronley's Hideout"
            },
            new RecognizedPosition {
                Zone = -1098758220,
                Name = "Cronley (Boss)"
            },
            new RecognizedPosition {
                Zone = -936296167,
                Name = "Arkovian Foothills"
            },
            new RecognizedPosition {
                Zone = -771013302,
                Name = "Deadmans Gulch"
            },
            new RecognizedPosition {
                Zone = 464142942,
                Name = "Deadmans Gulch"
            },
            new RecognizedPosition {
                Zone = -1605221695,
                Name = "Deadmans Gulch"
            },
            new RecognizedPosition {
                Zone = -1494005580,
                Name = "Deadmans Gulch"
            },
            new RecognizedPosition {
                Zone = 1621574431,
                Name = "Deadmans Gulch / The Immolation"
            },
            new RecognizedPosition {
                Zone = 1701594579,
                Name = "Smugglers Pass"
            },
            new RecognizedPosition {
                Zone = -41269260,
                Name = "Smugglers Pass"
            },
            new RecognizedPosition {
                Zone = -128563231,
                Name = "The Immolation"
            },
            new RecognizedPosition {
                Zone = -925743329,
                Name = "The Immolation"
            },
            new RecognizedPosition {
                Zone = 244338406,
                Name = "The Immolation"
            },
        };

        public PlayerPosition PlayerPosition { get; private set; }

        HashSet<int> seen = new HashSet<int>();

        public void SetPlayerPosition(PlayerPosition playerPosition) {
            PlayerPosition = playerPosition;
            if (!seen.Contains(playerPosition.Zone) && !_knownPositions.Exists(m => m.Zone == playerPosition.Zone)) {
                Logger.Warn($"New zone: {playerPosition.Zone}");
                seen.Add(playerPosition.Zone);
            }
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
