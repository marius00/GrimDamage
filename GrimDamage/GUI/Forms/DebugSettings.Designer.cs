namespace GrimDamage.GUI.Forms {
    partial class DebugSettings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.cbEnableInvestigativeLogging = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbEnableInvestigativeLogging
            // 
            this.cbEnableInvestigativeLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbEnableInvestigativeLogging.AutoSize = true;
            this.cbEnableInvestigativeLogging.Location = new System.Drawing.Point(12, 309);
            this.cbEnableInvestigativeLogging.Name = "cbEnableInvestigativeLogging";
            this.cbEnableInvestigativeLogging.Size = new System.Drawing.Size(193, 17);
            this.cbEnableInvestigativeLogging.TabIndex = 4;
            this.cbEnableInvestigativeLogging.Text = "Log unknown messages to console";
            this.cbEnableInvestigativeLogging.UseVisualStyleBackColor = true;
            this.cbEnableInvestigativeLogging.CheckedChanged += new System.EventHandler(this.cbEnableInvestigativeLogging_CheckedChanged);
            // 
            // DebugSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 338);
            this.Controls.Add(this.cbEnableInvestigativeLogging);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DebugSettings";
            this.Text = "DebugSettings";
            this.Load += new System.EventHandler(this.DebugSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbEnableInvestigativeLogging;
    }
}