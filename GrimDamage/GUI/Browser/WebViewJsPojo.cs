using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrimDamage.GUI.Browser.dto;
using GrimDamage.Statistics.dto;
using GrimDamage.Tracking.Model;
using Newtonsoft.Json;

namespace GrimDamage.GUI.Browser {
    // ReSharper disable InconsistentNaming
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class WebViewJsPojo {
        public event EventHandler OnRequestUpdate;
        public event EventHandler OnSuggestLocationName;
        public event EventHandler OnSave;
        public event EventHandler OnLog;


        public string stateChangesJson { get; set; }

        public string playerLocationName { get; set; }
        // FROM JS ONLY
        public string damageTakenJson { get; set; }

        public string damageDealtJson { get; set; }

        public string detailedDamageTakenJson { get; set; }

        public string damageDealtToSingleTargetJson { get; set; }

        public string playersJson { get; set; }

        public string petsJson { get; set; }

        public void requestUpdate() {
            OnRequestUpdate?.Invoke(this, null);
        }

        public void suggestLocationName(string name) {
            OnSuggestLocationName?.Invoke(this, new SuggestLocationNameArgument() {
                Suggestion = name
            });
        }

        public void save(string json) {
            OnSave?.Invoke(this, new SaveParseArgument() {
                Data = json
            });
        }
        public void log(string json) {
            OnLog?.Invoke(this, new SaveParseArgument() {
                Data = json
            });
        }

    }
    // ReSharper enable InconsistentNaming
    // ReSharper enable MemberCanBePrivate.Global
    // ReSharper enable UnusedAutoPropertyAccessor.Global
}
