namespace ovixInstaller
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            MainLabel = new Label();
            InstallRedist = new CheckBox();
            InstallBtn = new Button();
            UninstallBtn = new Button();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // MainLabel
            // 
            MainLabel.AutoSize = true;
            MainLabel.Font = new Font("Segoe UI Light", 48F, FontStyle.Regular, GraphicsUnit.Point);
            MainLabel.Location = new Point(-12, 9);
            MainLabel.Name = "MainLabel";
            MainLabel.Size = new Size(282, 86);
            MainLabel.TabIndex = 0;
            MainLabel.Text = "Ovix GTA";
            MainLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // InstallRedist
            // 
            InstallRedist.AutoSize = true;
            InstallRedist.Checked = true;
            InstallRedist.CheckState = CheckState.Checked;
            InstallRedist.Location = new Point(27, 128);
            InstallRedist.Name = "InstallRedist";
            InstallRedist.Size = new Size(211, 19);
            InstallRedist.TabIndex = 1;
            InstallRedist.Text = "Install C++ Redist (Recommended)";
            InstallRedist.UseVisualStyleBackColor = true;
            InstallRedist.CheckedChanged += InstallRedist_CheckedChanged;
            // 
            // InstallBtn
            // 
            InstallBtn.Location = new Point(12, 153);
            InstallBtn.Name = "InstallBtn";
            InstallBtn.Size = new Size(75, 23);
            InstallBtn.TabIndex = 2;
            InstallBtn.Text = "Install!";
            InstallBtn.UseVisualStyleBackColor = true;
            InstallBtn.Click += InstallBtn_Click;
            // 
            // UninstallBtn
            // 
            UninstallBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UninstallBtn.Location = new Point(174, 153);
            UninstallBtn.Name = "UninstallBtn";
            UninstallBtn.Size = new Size(75, 23);
            UninstallBtn.TabIndex = 3;
            UninstallBtn.Text = "Uninstall";
            UninstallBtn.UseVisualStyleBackColor = true;
            UninstallBtn.Click += UninstallBtn_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 185);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(261, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(261, 207);
            Controls.Add(statusStrip1);
            Controls.Add(UninstallBtn);
            Controls.Add(InstallBtn);
            Controls.Add(InstallRedist);
            Controls.Add(MainLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ovix Installer";
            Load += Form1_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label MainLabel;
        private CheckBox InstallRedist;
        private Button InstallBtn;
        private Button UninstallBtn;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
