using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Dictionary<int, Entity> Dataset {
            set {
                DatasetJson = JsonConvert.SerializeObject(value, _settings);
            }
        }

        public string DatasetJson { get; set; }


        
    }
}
