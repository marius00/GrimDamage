using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Forms;
using GrimDamage.Tracking.Model;

namespace GrimDamage {
    static class Program {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hook.dll"))) {
                MessageBox.Show("Error - It appears that hook.dll is missing\nMost likely this installation has been corrupted.", "Error");
                return;
            }

            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content", "index.html");
            if (!File.Exists(url)) {
                MessageBox.Show("Error - It appears the stat view is missing", "Error");
            }

            using (var browser = new CefBrowserHandler()) {
                WebViewJsInteractor jsInteractor = new WebViewJsInteractor();
                browser.InitializeChromium(url, jsInteractor, null);
                Application.Run(new Form1(browser));
            }
        }
    }
}
