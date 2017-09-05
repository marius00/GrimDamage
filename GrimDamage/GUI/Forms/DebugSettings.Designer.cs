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
            this.btnStartLogging = new System.Windows.Forms.Button();
            this.btnLoadLog = new System.Windows.Forms.Button();
            this.radioMockCombatListener = new System.Windows.Forms.RadioButton();
            this.radioLiveCombatListener = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnStartLogging
            // 
            this.btnStartLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartLogging.Location = new System.Drawing.Point(529, 12);
            this.btnStartLogging.Name = "btnStartLogging";
            this.btnStartLogging.Size = new System.Drawing.Size(111, 23);
            this.btnStartLogging.TabIndex = 0;
            this.btnStartLogging.Text = "Start Logging";
            this.btnStartLogging.UseVisualStyleBackColor = true;
            this.btnStartLogging.Click += new System.EventHandler(this.btnStartLogging_Click);
            // 
            // btnLoadLog
            // 
            this.btnLoadLog.Location = new System.Drawing.Point(12, 67);
            this.btnLoadLog.Name = "btnLoadLog";
            this.btnLoadLog.Size = new System.Drawing.Size(119, 23);
            this.btnLoadLog.TabIndex = 1;
            this.btnLoadLog.Text = "Load log";
            this.btnLoadLog.UseVisualStyleBackColor = true;
            this.btnLoadLog.Click += new System.EventHandler(this.btnLoadLog_Click);
            // 
            // radioMockCombatListener
            // 
            this.radioMockCombatListener.AutoSize = true;
            this.radioMockCombatListener.Location = new System.Drawing.Point(12, 35);
            this.radioMockCombatListener.Name = "radioMockCombatListener";
            this.radioMockCombatListener.Size = new System.Drawing.Size(126, 17);
            this.radioMockCombatListener.TabIndex = 2;
            this.radioMockCombatListener.TabStop = true;
            this.radioMockCombatListener.Text = "Mock combat listener";
            this.radioMockCombatListener.UseVisualStyleBackColor = true;
            this.radioMockCombatListener.CheckedChanged += new System.EventHandler(this.radioMockCombatListener_CheckedChanged);
            // 
            // radioLiveCombatListener
            // 
            this.radioLiveCombatListener.AutoSize = true;
            this.radioLiveCombatListener.Location = new System.Drawing.Point(12, 12);
            this.radioLiveCombatListener.Name = "radioLiveCombatListener";
            this.radioLiveCombatListener.Size = new System.Drawing.Size(119, 17);
            this.radioLiveCombatListener.TabIndex = 3;
            this.radioLiveCombatListener.TabStop = true;
            this.radioLiveCombatListener.Text = "Live combat listener";
            this.radioLiveCombatListener.UseVisualStyleBackColor = true;
            this.radioLiveCombatListener.CheckedChanged += new System.EventHandler(this.radioLiveCombatListener_CheckedChanged);
            // 
            // DebugSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 338);
            this.Controls.Add(this.radioLiveCombatListener);
            this.Controls.Add(this.radioMockCombatListener);
            this.Controls.Add(this.btnLoadLog);
            this.Controls.Add(this.btnStartLogging);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DebugSettings";
            this.Text = "DebugSettings";
            this.Load += new System.EventHandler(this.DebugSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartLogging;
        private System.Windows.Forms.Button btnLoadLog;
        private System.Windows.Forms.RadioButton radioMockCombatListener;
        private System.Windows.Forms.RadioButton radioLiveCombatListener;
    }
}