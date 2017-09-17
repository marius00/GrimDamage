namespace GrimDamage {
    partial class Form1 {
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
            this.labelHookStatus = new System.Windows.Forms.Label();
            this.webViewPanel = new System.Windows.Forms.Panel();
            this.btnShowDevtools = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panelDebugView = new System.Windows.Forms.Panel();
            this.linkDiscord = new System.Windows.Forms.LinkLabel();
            this.btnLoadSave = new System.Windows.Forms.Button();
            this.linkDonate = new System.Windows.Forms.LinkLabel();
            this.linkItemAssistant = new System.Windows.Forms.LinkLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelHookStatus
            // 
            this.labelHookStatus.AutoSize = true;
            this.labelHookStatus.ForeColor = System.Drawing.Color.Red;
            this.labelHookStatus.Location = new System.Drawing.Point(6, 3);
            this.labelHookStatus.Name = "labelHookStatus";
            this.labelHookStatus.Size = new System.Drawing.Size(115, 13);
            this.labelHookStatus.TabIndex = 0;
            this.labelHookStatus.Text = "Hook not yet activated";
            // 
            // webViewPanel
            // 
            this.webViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webViewPanel.Location = new System.Drawing.Point(2, 2);
            this.webViewPanel.Name = "webViewPanel";
            this.webViewPanel.Size = new System.Drawing.Size(1175, 611);
            this.webViewPanel.TabIndex = 3;
            // 
            // btnShowDevtools
            // 
            this.btnShowDevtools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowDevtools.Location = new System.Drawing.Point(1113, -1);
            this.btnShowDevtools.Name = "btnShowDevtools";
            this.btnShowDevtools.Size = new System.Drawing.Size(75, 23);
            this.btnShowDevtools.TabIndex = 4;
            this.btnShowDevtools.Text = "DevTools";
            this.btnShowDevtools.UseVisualStyleBackColor = true;
            this.btnShowDevtools.Click += new System.EventHandler(this.btnShowDevtools_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1185, 639);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.webViewPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1177, 613);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelDebugView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1177, 613);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Debug";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelDebugView
            // 
            this.panelDebugView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDebugView.Location = new System.Drawing.Point(1, 2);
            this.panelDebugView.Name = "panelDebugView";
            this.panelDebugView.Size = new System.Drawing.Size(1176, 611);
            this.panelDebugView.TabIndex = 7;
            // 
            // linkDiscord
            // 
            this.linkDiscord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkDiscord.AutoSize = true;
            this.linkDiscord.Location = new System.Drawing.Point(1090, 25);
            this.linkDiscord.Name = "linkDiscord";
            this.linkDiscord.Size = new System.Drawing.Size(98, 13);
            this.linkDiscord.TabIndex = 7;
            this.linkDiscord.TabStop = true;
            this.linkDiscord.Text = "Discuss on Discord";
            this.linkDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDiscord_LinkClicked);
            // 
            // btnLoadSave
            // 
            this.btnLoadSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSave.Location = new System.Drawing.Point(1012, -1);
            this.btnLoadSave.Name = "btnLoadSave";
            this.btnLoadSave.Size = new System.Drawing.Size(95, 23);
            this.btnLoadSave.TabIndex = 8;
            this.btnLoadSave.Text = "Load Save";
            this.btnLoadSave.UseVisualStyleBackColor = true;
            this.btnLoadSave.Click += new System.EventHandler(this.btnLoadSave_Click);
            // 
            // linkDonate
            // 
            this.linkDonate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkDonate.AutoSize = true;
            this.linkDonate.Location = new System.Drawing.Point(1009, 25);
            this.linkDonate.Name = "linkDonate";
            this.linkDonate.Size = new System.Drawing.Size(42, 13);
            this.linkDonate.TabIndex = 9;
            this.linkDonate.TabStop = true;
            this.linkDonate.Text = "Donate";
            this.linkDonate.Visible = false;
            this.linkDonate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkDonate_LinkClicked);
            // 
            // linkItemAssistant
            // 
            this.linkItemAssistant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkItemAssistant.AutoSize = true;
            this.linkItemAssistant.Location = new System.Drawing.Point(467, 9);
            this.linkItemAssistant.Name = "linkItemAssistant";
            this.linkItemAssistant.Size = new System.Drawing.Size(181, 13);
            this.linkItemAssistant.TabIndex = 10;
            this.linkItemAssistant.TabStop = true;
            this.linkItemAssistant.Text = "Having trouble managing your items?";
            this.linkItemAssistant.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkItemAssistant_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 667);
            this.Controls.Add(this.linkItemAssistant);
            this.Controls.Add(this.linkDonate);
            this.Controls.Add(this.btnLoadSave);
            this.Controls.Add(this.linkDiscord);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnShowDevtools);
            this.Controls.Add(this.labelHookStatus);
            this.Name = "Form1";
            this.Text = "Project GD Damage Parser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHookStatus;
        private System.Windows.Forms.Panel webViewPanel;
        private System.Windows.Forms.Button btnShowDevtools;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panelDebugView;
        private System.Windows.Forms.LinkLabel linkDiscord;
        private System.Windows.Forms.Button btnLoadSave;
        private System.Windows.Forms.LinkLabel linkDonate;
        private System.Windows.Forms.LinkLabel linkItemAssistant;
    }
}

