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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReadFile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnShowDevtools = new System.Windows.Forms.Button();
            this.btnUpdateData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelHookStatus
            // 
            this.labelHookStatus.AutoSize = true;
            this.labelHookStatus.ForeColor = System.Drawing.Color.Red;
            this.labelHookStatus.Location = new System.Drawing.Point(12, 9);
            this.labelHookStatus.Name = "labelHookStatus";
            this.labelHookStatus.Size = new System.Drawing.Size(115, 13);
            this.labelHookStatus.TabIndex = 0;
            this.labelHookStatus.Text = "Hook not yet activated";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 42);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Now";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReadFile
            // 
            this.btnReadFile.Location = new System.Drawing.Point(139, 42);
            this.btnReadFile.Name = "btnReadFile";
            this.btnReadFile.Size = new System.Drawing.Size(75, 23);
            this.btnReadFile.TabIndex = 2;
            this.btnReadFile.Text = "Read File";
            this.btnReadFile.UseVisualStyleBackColor = true;
            this.btnReadFile.Click += new System.EventHandler(this.btnReadFile_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(28, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(905, 363);
            this.panel1.TabIndex = 3;
            // 
            // btnShowDevtools
            // 
            this.btnShowDevtools.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowDevtools.Location = new System.Drawing.Point(858, 12);
            this.btnShowDevtools.Name = "btnShowDevtools";
            this.btnShowDevtools.Size = new System.Drawing.Size(75, 23);
            this.btnShowDevtools.TabIndex = 4;
            this.btnShowDevtools.Text = "DevTools";
            this.btnShowDevtools.UseVisualStyleBackColor = true;
            this.btnShowDevtools.Click += new System.EventHandler(this.btnShowDevtools_Click);
            // 
            // btnUpdateData
            // 
            this.btnUpdateData.Location = new System.Drawing.Point(759, 12);
            this.btnUpdateData.Name = "btnUpdateData";
            this.btnUpdateData.Size = new System.Drawing.Size(93, 23);
            this.btnUpdateData.TabIndex = 5;
            this.btnUpdateData.Text = "Update Data";
            this.btnUpdateData.UseVisualStyleBackColor = true;
            this.btnUpdateData.Click += new System.EventHandler(this.btnUpdateData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 481);
            this.Controls.Add(this.btnUpdateData);
            this.Controls.Add(this.btnShowDevtools);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnReadFile);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.labelHookStatus);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelHookStatus;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReadFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnShowDevtools;
        private System.Windows.Forms.Button btnUpdateData;
    }
}

