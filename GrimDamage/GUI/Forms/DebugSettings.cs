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

        public DebugSettings() {
            InitializeComponent();
        }

        private void DebugSettings_Load(object sender, EventArgs e) {
            this.Dock = DockStyle.Fill;
        }

        private void cbEnableInvestigativeLogging_CheckedChanged(object sender, EventArgs e) {
            if (cbEnableInvestigativeLogging.Checked)
                MessageBox.Show(
                    "Sorry, not yet available\n\nLook for the following line in MessageProcessorCore:\n(bt.Type == 45000)", "Sorry!");
        }
    }
}
