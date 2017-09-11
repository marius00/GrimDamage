using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EvilsoftCommons.Exceptions;
using GrimDamage.Statistics.model;
using log4net;

namespace GrimDamage.Crowdsourced.Web {
    class NameSuggestionService {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(NameSuggestionService));
        private readonly string _host;

        private string UploadUrl => $"{_host}/suggestZoneName.php";

        public NameSuggestionService(string host) {
            this._host = host;
        }

        public bool SuggestName(PlayerPosition position, string name) {
            return !string.IsNullOrEmpty(Post($"x={position.X}&y={position.Y}&z={position.Z}&zone={position.Zone}&name={name}", UploadUrl));
        }

        private static string Post(string postData, string URL) {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            if (request == null) {
                Logger.Warn("Could not create HttpWebRequest");
                return null;
            }
            var encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            try {
                using (Stream stream = request.GetRequestStream()) {
                    stream.Write(data, 0, data.Length);
                }
                // threshold
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                    if (response.StatusCode != HttpStatusCode.OK) {
                        Logger.Info("Failed to upload buddy item data.");
                        return null;
                    }

                    string result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return result;
                }
            }
            catch (WebException ex) {
                if (ex.Status != WebExceptionStatus.NameResolutionFailure && ex.Status != WebExceptionStatus.Timeout) {
                    Logger.Warn(ex.Message);
                }
                else {
                    Logger.Info("Could not resolve DNS for server, delaying post.");
                }
            }
            catch (IOException ex) {
                Logger.Warn(ex.Message);
                Logger.Warn(ex.StackTrace);
            }
            catch (Exception ex) {
                Logger.Error(ex.Message);
                Logger.Error(ex.StackTrace);
                ExceptionReporter.ReportException(ex);
            }

            return null;
        }
    }
}
