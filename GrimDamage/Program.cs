using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Forms;

namespace GrimDamage {
    static class Program {

        class WebViewJsInteractor {
            
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "content", "index.html");
            if (!File.Exists(url)) {
                MessageBox.Show("Error - It appears the stat view is missing", "Error");
            }

            using (var browser = new CefBrowserHandler()) {
                browser.InitializeChromium(url, new WebViewJsInteractor(), null);
                Application.Run(new Form1(browser));
            }
        }
    }
}
