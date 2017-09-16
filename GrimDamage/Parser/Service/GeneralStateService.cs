using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.Parser.Model;
using GrimDamage.Settings;
using GrimDamage.Statistics.dto;
using GrimDamage.Utility;
using log4net;

namespace GrimDamage.Parser.Service {
    class GeneralStateService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(GeneralStateService));
        private List<GrimDawnStateEventJson> _states = new List<GrimDawnStateEventJson>();
        private readonly AppSettings _appSettings;

        public GeneralStateService(AppSettings settings) {
            _appSettings = settings;
        }

        public void PushState(GrimState state) {
            _states.Add(new GrimDawnStateEventJson {
                Event = state.ToString(),
                Timestamp = Timestamp.UTCMillisecondsNow
            });
            if (_appSettings.LogStateChanges) {
                Logger.Debug($"GD State has been set to \"{state}\"");
            }
        }

        public List<GrimDawnStateEventJson> GetStates(long timestamp) {
            return _states.Where(m => m.Timestamp > timestamp).OrderByDescending(m => m.Timestamp).ToList();
        }
    }
}
