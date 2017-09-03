using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrimDamage.GUI.Browser;

namespace GrimDamage.GUI.Forms {
    public partial class WebView : Form {
        private readonly CefBrowserHandler browser;
        public WebView(CefBrowserHandler browser) {
            InitializeComponent();
            this.browser = browser;
        }

        private void WebView_Load(object sender, EventArgs e) {
            this.Dock = DockStyle.Fill;
            Controls.Add(browser.BrowserControl);
        }
    }
}
