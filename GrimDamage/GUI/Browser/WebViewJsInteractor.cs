using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Statistics.dto;
using Newtonsoft.Json;

namespace GrimDamage.GUI.Browser {
    public class WebViewJsInteractor {
        private readonly JsonSerializerSettings _settings;
        private readonly WebViewJsPojo _js;

        public WebViewJsInteractor(WebViewJsPojo js) {
            _settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = System.Globalization.CultureInfo.InvariantCulture,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            _js = js;
        }


        public void SetPlayerLocation(string location) {
            _js.playerLocationName = location;
        }

        public void SetDamageTaken(Dictionary<int, List<DamageEntryJson>> value) {
            _js.damageTakenJson = JsonConvert.SerializeObject(value, _settings);
        }


        public void SetDamageDealt(Dictionary<int, List<DamageEntryJson>> value) {
            _js.damageDealtJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetDamageDealtToSingleTarget(Dictionary<int, List<DamageEntryJson>> value) {
            _js.damageDealtToSingleTargetJson = JsonConvert.SerializeObject(value, _settings);
        }

        public void SetPlayers(List<PlayerJson> value) {
            _js.playersJson = JsonConvert.SerializeObject(value, _settings);
        }

    }
}
