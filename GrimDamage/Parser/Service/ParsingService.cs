using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrimDamage.Parser.Config;
using GrimDamage.Parser.Model;
using GrimDamage.Tracking.Model;
using GrimDamage.Utility;
using log4net;

namespace GrimDamage.Parser.Service {
    public class ParsingService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ParsingService));
        private DamageType convertDamage(string type) {
            if (EventMapping.DamageTypes.ContainsKey(type)) {
                return EventMapping.DamageTypes[type];
            }
            else {
                Logger.Warn($"Unknown damage type \"{type}\"");
                return DamageType.Unknown;
            }
        }

        float toFloat(string s) {
            return float.Parse(s.Replace(".", ","));
        }

        public void Parse(string entry) {
            string pattern = EventMapping.PatternMap[EventType.DamageDealt];
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var match = regex.Match(entry);

            if (match.Success) {
                var amount = toFloat(match.Groups[1].Value);
                var defender = int.Parse(match.Groups[2].Value, System.Globalization.NumberStyles.HexNumber);
                var damageType = match.Groups[3].Value;
                
                var dmg = new DamageDealtEntry {
                    Amount = amount,
                    Target = defender,
                    Type = convertDamage(damageType),
                    Time = DateTime.UtcNow
                };
            }

            int x = 9;
        }
    }
}
