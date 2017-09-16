using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Service;
using GrimDamage.Statistics.Service;
using log4net;
using Newtonsoft.Json;

namespace GrimDamage.GUI.Browser {
    class CSharpJsStateMapper {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(CSharpJsStateMapper));
        private readonly CefBrowserHandler _browser;
        private readonly StatisticsService _statisticsService;
        private readonly GeneralStateService _generalStateService;
        private readonly JsonSerializerSettings _settings;

        public CSharpJsStateMapper(CefBrowserHandler browser, StatisticsService statisticsService, GeneralStateService generalStateService) {
            _browser = browser;
            _statisticsService = statisticsService;
            _generalStateService = generalStateService;
            _settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Culture = System.Globalization.CultureInfo.InvariantCulture,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
        }

        private string Serialize(object value) {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public void RequestData(DataRequestType data, long timestamp, int? entityId, string callback) {
            switch (data) {
                case DataRequestType.States:
                    TransferStates(timestamp, callback);
                    break;

                case DataRequestType.DetailedDamageTaken:
                    if (entityId.HasValue) {
                        TransferDetailedDamage(entityId.Value, timestamp, callback);
                    }
                    else {
                        Logger.Warn($"Data request for {data} was not handled due to the entityId being null.");
                    }
                    break;

                default:
                    Logger.Warn($"Data request for {data} was not handled, unknown type.");
                    break;
            }
        }
        private void TransferDetailedDamage(int entityId, long timestamp, string callback) {
            _browser.JsCallback(callback, Serialize(_statisticsService.GetDetailedLatestDamageTaken(entityId, timestamp)));
        }

        private void TransferStates(long timestamp, string callback) {
            _browser.JsCallback(callback, Serialize(_generalStateService.GetStates(timestamp)));
        }
    }
}
