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
using GrimDamage.Utility;
using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Win32;

namespace GrimDamage {
    static class Program {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static string GetUuid() {

            using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(@"Software\EvilSoft\GrimDamage")) {
                string uuid = (string)registryKey?.GetValue("uuid");
                if (!string.IsNullOrEmpty(uuid)) {
                    return uuid;
                }

                UuidGenerator g = Guid.NewGuid();
                uuid = g.ToString().Replace("-", "");

                registryKey?.SetValue("uuid", uuid);
                return uuid;
            }
        }


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
            ExceptionReporter.Uuid = GetUuid();
#if !DEBUG
#endif
            Logger.Info("Anonymous usage statistics and crash reports will be collected.");
            Logger.Info("Statistics and crash reports can be found at http://ribbs.dreamcrash.org/gddamage/logs.html");

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hook.dll"))) {
                MessageBox.Show("Error - It appears that hook.dll is missing\nMost likely this installation has been corrupted.", "Error");
                return;
            }

            
            string url = Properties.Settings.Default.DarkModeEnabled 
                    ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content", "darkmode.html")
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content", "index.html");

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
