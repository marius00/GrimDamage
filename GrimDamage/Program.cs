using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons.Exceptions;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Forms;
using GrimDamage.Settings;
using GrimDamage.Tracking.Model;
using log4net;
using log4net.Repository.Hierarchy;

namespace GrimDamage {
    static class Program {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Thread.CurrentThread.Name == null) {
                Thread.CurrentThread.Name = "Main/UI";
                ExceptionReporter.EnableLogUnhandledOnThread();
            }

            ExceptionReporter.UrlCrashreport = "http://ribbs.dreamcrash.org/gddamage/crashreport.php";
            ExceptionReporter.UrlStats = "http://ribbs.dreamcrash.org/gddamage/stats.php";
            ExceptionReporter.LogExceptions = true;
#if !DEBUG
#endif
            Logger.Info("Anonymous usage statistics and crash reports will be collected.");
            Logger.Info("Statistics and crash reports can be found at http://ribbs.dreamcrash.org/gddamage/logs.html");

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hook.dll"))) {
                MessageBox.Show("Error - It appears that hook.dll is missing\nMost likely this installation has been corrupted.", "Error");
                return;
            }

            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content", "index.html");
            if (!File.Exists(url)) {
                MessageBox.Show("Error - It appears the stat view is missing", "Error");
            }

            using (var browser = new CefBrowserHandler()) {
                WebViewJsPojo jsPojo = new WebViewJsPojo();
                browser.InitializeChromium(url, jsPojo, null);
                Application.Run(new Form1(browser, GetSettings()));
            }
        }

        private static AppSettings GetSettings() {
            return new AppSettings {

            };
        }
    }
}
