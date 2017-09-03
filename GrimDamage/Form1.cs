using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvilsoftCommons;
using EvilsoftCommons.DllInjector;
using GrimDamage.GD.Processors;
using GrimDamage.GUI.Browser;
using GrimDamage.GUI.Forms;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using log4net;

namespace GrimDamage {
    public partial class Form1 : Form {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Form1));

        private readonly DamageParsingService _damageParsingService = new DamageParsingService();
        private readonly CefBrowserHandler _browser;
        private readonly MessageProcessorCore _messageProcessorCore;


        public Form1(CefBrowserHandler browser) {
            InitializeComponent();
            this._browser = browser;
            _messageProcessorCore = new MessageProcessorCore(_damageParsingService);
            
        }

        private void Form1_Load(object sender, EventArgs e) {
            Logger.Debug("Starting..");

            this.Closing += Form1_Closing;

            var webView = new WebView(_browser);
            webView.TopLevel = false;
            this.panel1.Controls.Add(webView);
            webView.Show();
        }

        private void Save() {
            //string filename = Path.Combine(GlobalSettings.LogPath, DateTime.UtcNow.ToFileTime().ToString() + ".txt");
            //File.WriteAllLines(filename, _events);
        }

        private void Form1_Closing(object sender, CancelEventArgs e) {
            _messageProcessorCore?.Dispose();

            Save();
        }


        private void btnSave_Click(object sender, EventArgs e) {
            Save();
        }

        private void btnReadFile_Click(object sender, EventArgs e) {
            ParsingService parser = new ParsingService();
            string filename = Path.Combine(GlobalSettings.LogPath, "131482213376447842.txt");
            string[] dataset = File.ReadAllLines(filename);
            foreach (var entry in dataset) {
                parser.Parse(entry);
            }

        }

        private void btnShowDevtools_Click(object sender, EventArgs e) {
            _browser.ShowDevTools();
        }

        private void btnUpdateData_Click(object sender, EventArgs e) {
            _browser.JsInteractor.Dataset = _damageParsingService.GetNames();
        }
    }
}
