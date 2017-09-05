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
using GrimDamage.GD.Logger;
using GrimDamage.Parser.Service;
using GrimDamage.Settings;
using GrimDamage.Utility;

namespace GrimDamage.GUI.Forms {
    public partial class DebugSettings : Form {
        private readonly CombatFileLogger _combatFileLogger;
        private readonly DamageParsingService _damageParsingService;
        private bool _isLogging;
        private CombatFileReader _combatFileReader;
        private bool _isMockCombat;
        private System.Windows.Forms.Timer _mockCombatTimer;

        public DebugSettings(CombatFileLogger combatFileLogger, DamageParsingService damageParsingService) {
            InitializeComponent();
            _combatFileLogger = combatFileLogger;
            _damageParsingService = damageParsingService;
        }

        private void btnStartLogging_Click(object sender, EventArgs e) {
            if (_isLogging) {
                _combatFileLogger.Enabled = false;
                var file = Path.Combine(GlobalSettings.BaseFolder, $"{Timestamp.UTC}-log.txt");
                _combatFileLogger.WriteToFile(file);
                btnStartLogging.Text = "Start Logging";
                _isLogging = !_isLogging;
            }
            else {
                _combatFileLogger.Enabled = true;
                btnStartLogging.Text = "Save log..";
                _isLogging = !_isLogging;
            }
                
        }

        private void btnLoadLog_Click(object sender, EventArgs e) {
            var ofd = new OpenFileDialog {
                InitialDirectory = GlobalSettings.LogPath,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };


            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName)) {
                _combatFileReader = new CombatFileReader(
                    _damageParsingService, 
                    new List<string>(File.ReadAllLines(ofd.FileName))
                );
            }

            if (_isMockCombat)
                EnableTimer();

        }

        private void radioLiveCombatListener_CheckedChanged(object sender, EventArgs e) {
            _isMockCombat = false;
            _mockCombatTimer?.Stop();
            _mockCombatTimer = null;
        }

        private void radioMockCombatListener_CheckedChanged(object sender, EventArgs e) {
            _isMockCombat = true;
            EnableTimer();
        }

        private void EnableTimer() {
            if (_mockCombatTimer == null) {
                var timer = new System.Windows.Forms.Timer();
                timer.Tick += (o, args) => {
                    _combatFileReader?.Next();
                };
                timer.Interval = 1000;
                timer.Start();

                _mockCombatTimer = timer;
            }

        }

        private void DebugSettings_Load(object sender, EventArgs e) {
            this.Dock = DockStyle.Fill;
            radioLiveCombatListener.Checked = true;
        }
    }
}
