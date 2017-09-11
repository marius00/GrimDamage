using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using GrimDamage.Utility;

namespace GrimDamage.GUI.Forms {
    public partial class DebugSettings : Form {
        private readonly AppSettings _appSettings;

        public DebugSettings(AppSettings appSettings) {
            InitializeComponent();
            _appSettings = appSettings;
        }

        private void DebugSettings_Load(object sender, EventArgs e) {
            this.Dock = DockStyle.Fill;
            cbLogStateChanges.Checked = _appSettings.LogStateChanges;
            cbLogAllLogStatements.Checked = _appSettings.LogAllEvents;
            cbLogUnknownLogStatements.Checked = _appSettings.LogUnknownEvents;
            cbEnableInvestigativeLogging.Checked = _appSettings.LogProcessedMessages;
            cbLogPlayerMovement.Checked = _appSettings.LogPlayerMovement;
            cbLogPlayerDetection.Checked = _appSettings.LogPlayerDetection;
        }

        private void cbEnableInvestigativeLogging_CheckedChanged(object sender, EventArgs e) {
            _appSettings.LogProcessedMessages = (sender as CheckBox)?.Checked ?? false;
        }

        private void cbLogStateChanges_CheckedChanged(object sender, EventArgs e) {
            _appSettings.LogStateChanges = (sender as CheckBox)?.Checked ?? false;
        }

        private void cbLogAllLogStatements_CheckedChanged(object sender, EventArgs e) {
            _appSettings.LogAllEvents = (sender as CheckBox)?.Checked ?? false;
        }

        private void cbLogUnknownLogStatements_CheckedChanged(object sender, EventArgs e) {
            _appSettings.LogUnknownEvents = (sender as CheckBox)?.Checked ?? false;
        }

        private void cbLogPlayerMovement_CheckedChanged(object sender, EventArgs e) {
            _appSettings.LogPlayerMovement = (sender as CheckBox)?.Checked ?? false;
        }

        private void cbLogPlayerDetection_CheckedChanged(object sender, EventArgs e) {
            _appSettings.LogPlayerDetection = (sender as CheckBox)?.Checked ?? false;
        }
    }
}
