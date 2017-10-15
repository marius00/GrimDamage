using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public event EventHandler OnSave;
        public event EventHandler OnLog;
        public event EventHandler OnRequestData;
        public event EventHandler OnSetLightMode;
        
        public string api { get; set; }


        public string version {
            get {

                var v = Assembly.GetExecutingAssembly().GetName().Version;
                DateTime buildDate = new DateTime(2000, 1, 1)
                    .AddDays(v.Build)
                    .AddSeconds(v.Revision * 2);

                int daysAgo = (DateTime.UtcNow - buildDate).Days;
                if (daysAgo > 0) {
                    return
                        $"Running version {v.Major}.{v.Minor}.{v.Build}.{v.Revision} released {buildDate:dd/MM/yyyy} ({daysAgo} days ago)";
                }
                else {
                    return $"Running version {v.Major}.{v.Minor}.{v.Build}.{v.Revision} released {buildDate:dd/MM/yyyy} (today)";
                }
            }
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

        public void setLightMode(string mode) {
            OnSetLightMode?.Invoke(this, new LightModeArgument {
                IsDarkMode = mode.ToLower() == "dark"
            });
        }


        public void requestData(int type, string start, string end, int id, string callback) {
            OnRequestData?.Invoke(this, new RequestDataArgument {
                Type = (DataRequestType)type,
                TimestampStart = start,
                TimestampEnd = end,
                Callback = callback,
                EntityId = id
            });
        }
        
    }
    // ReSharper enable InconsistentNaming
    // ReSharper enable MemberCanBePrivate.Global
    // ReSharper enable UnusedAutoPropertyAccessor.Global
}
