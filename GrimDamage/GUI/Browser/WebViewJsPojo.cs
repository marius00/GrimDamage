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
    public class WebViewJsPojo {
        public event EventHandler OnRequestUpdate;
        public event EventHandler OnSuggestLocationName;


        public string stateChangesJson { get; set; }

        public string playerLocationName { get; set; }
        // FROM JS ONLY
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once MemberCanBePrivate.Global
        public string damageTakenJson { get; set; }

        // ReSharper disable once InconsistentNaming
        public string damageDealtJson { get; set; }

        public string damageDealtToSingleTargetJson { get; set; }

        // ReSharper disable once InconsistentNaming
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


    }
}
