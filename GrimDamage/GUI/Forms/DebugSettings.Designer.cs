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
            this.cbLogStateChanges = new System.Windows.Forms.CheckBox();
            this.cbLogAllLogStatements = new System.Windows.Forms.CheckBox();
            this.cbLogUnknownLogStatements = new System.Windows.Forms.CheckBox();
            this.cbLogPlayerMovement = new System.Windows.Forms.CheckBox();
            this.cbLogPlayerDetection = new System.Windows.Forms.CheckBox();
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
            // cbLogStateChanges
            // 
            this.cbLogStateChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbLogStateChanges.AutoSize = true;
            this.cbLogStateChanges.Location = new System.Drawing.Point(12, 286);
            this.cbLogStateChanges.Name = "cbLogStateChanges";
            this.cbLogStateChanges.Size = new System.Drawing.Size(114, 17);
            this.cbLogStateChanges.TabIndex = 5;
            this.cbLogStateChanges.Text = "Log state changes";
            this.cbLogStateChanges.UseVisualStyleBackColor = true;
            this.cbLogStateChanges.CheckedChanged += new System.EventHandler(this.cbLogStateChanges_CheckedChanged);
            // 
            // cbLogAllLogStatements
            // 
            this.cbLogAllLogStatements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLogAllLogStatements.AutoSize = true;
            this.cbLogAllLogStatements.Location = new System.Drawing.Point(514, 239);
            this.cbLogAllLogStatements.Name = "cbLogAllLogStatements";
            this.cbLogAllLogStatements.Size = new System.Drawing.Size(92, 17);
            this.cbLogAllLogStatements.TabIndex = 6;
            this.cbLogAllLogStatements.Text = "Log all events";
            this.cbLogAllLogStatements.UseVisualStyleBackColor = true;
            this.cbLogAllLogStatements.CheckedChanged += new System.EventHandler(this.cbLogAllLogStatements_CheckedChanged);
            // 
            // cbLogUnknownLogStatements
            // 
            this.cbLogUnknownLogStatements.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLogUnknownLogStatements.AutoSize = true;
            this.cbLogUnknownLogStatements.Location = new System.Drawing.Point(514, 262);
            this.cbLogUnknownLogStatements.Name = "cbLogUnknownLogStatements";
            this.cbLogUnknownLogStatements.Size = new System.Drawing.Size(126, 17);
            this.cbLogUnknownLogStatements.TabIndex = 7;
            this.cbLogUnknownLogStatements.Text = "Log unknown events";
            this.cbLogUnknownLogStatements.UseVisualStyleBackColor = true;
            this.cbLogUnknownLogStatements.CheckedChanged += new System.EventHandler(this.cbLogUnknownLogStatements_CheckedChanged);
            // 
            // cbLogPlayerMovement
            // 
            this.cbLogPlayerMovement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLogPlayerMovement.AutoSize = true;
            this.cbLogPlayerMovement.Location = new System.Drawing.Point(514, 285);
            this.cbLogPlayerMovement.Name = "cbLogPlayerMovement";
            this.cbLogPlayerMovement.Size = new System.Drawing.Size(127, 17);
            this.cbLogPlayerMovement.TabIndex = 8;
            this.cbLogPlayerMovement.Text = "Log player movement";
            this.cbLogPlayerMovement.UseVisualStyleBackColor = true;
            this.cbLogPlayerMovement.CheckedChanged += new System.EventHandler(this.cbLogPlayerMovement_CheckedChanged);
            // 
            // cbLogPlayerDetection
            // 
            this.cbLogPlayerDetection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLogPlayerDetection.AutoSize = true;
            this.cbLogPlayerDetection.Location = new System.Drawing.Point(513, 308);
            this.cbLogPlayerDetection.Name = "cbLogPlayerDetection";
            this.cbLogPlayerDetection.Size = new System.Drawing.Size(122, 17);
            this.cbLogPlayerDetection.TabIndex = 9;
            this.cbLogPlayerDetection.Text = "Log player detection";
            this.cbLogPlayerDetection.UseVisualStyleBackColor = true;
            this.cbLogPlayerDetection.CheckedChanged += new System.EventHandler(this.cbLogPlayerDetection_CheckedChanged);
            // 
            // DebugSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 338);
            this.Controls.Add(this.cbLogPlayerDetection);
            this.Controls.Add(this.cbLogPlayerMovement);
            this.Controls.Add(this.cbLogUnknownLogStatements);
            this.Controls.Add(this.cbLogAllLogStatements);
            this.Controls.Add(this.cbLogStateChanges);
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
        private System.Windows.Forms.CheckBox cbLogStateChanges;
        private System.Windows.Forms.CheckBox cbLogAllLogStatements;
        private System.Windows.Forms.CheckBox cbLogUnknownLogStatements;
        private System.Windows.Forms.CheckBox cbLogPlayerMovement;
        private System.Windows.Forms.CheckBox cbLogPlayerDetection;
    }
}