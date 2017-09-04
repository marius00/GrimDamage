using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Statistics.dto;
using GrimDamage.Tracking.Model;
using Newtonsoft.Json;

namespace GrimDamage.GUI.Browser {
    public class WebViewJsInteractor {
        private JsonSerializerSettings _settings;

        public WebViewJsInteractor() {
            _settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = System.Globalization.CultureInfo.InvariantCulture,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }

        public Dictionary<int, List<DamageEntryJson>> DamageTaken {
            set {
                damageTakenJson = JsonConvert.SerializeObject(value, _settings);
            }
        }

        public Dictionary<int, List<DamageEntryJson>> DamageDealt {
            set {
                damageDealtJson = JsonConvert.SerializeObject(value, _settings);
            }
        }

        public List<PlayerJson> Players {
            set {
                playersJson = JsonConvert.SerializeObject(value, _settings);
            }
        }

        public string damageTakenJson { get; set; }
        public string damageDealtJson { get; set; }
        public string playersJson { get; set; }



    }
}
