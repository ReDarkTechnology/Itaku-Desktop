namespace ItakuDesktop
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.appHeader = new System.Windows.Forms.Panel();
            this.appHeaderLabel = new System.Windows.Forms.Label();
            this.applicationArea = new System.Windows.Forms.Panel();
            this.StartupCheckbox = new System.Windows.Forms.CheckBox();
            this.UpdateCheckbox = new System.Windows.Forms.CheckBox();
            this.HideInTrayCheckbox = new System.Windows.Forms.CheckBox();
            this.browsingArea = new System.Windows.Forms.Panel();
            this.extensionsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ReloadNumeric = new System.Windows.Forms.NumericUpDown();
            this.ReloadCheckbox = new System.Windows.Forms.CheckBox();
            this.ExternalCheckbox = new System.Windows.Forms.CheckBox();
            this.EnhancementCheckbox = new System.Windows.Forms.CheckBox();
            this.browsingHeader = new System.Windows.Forms.Panel();
            this.browsingHeaderLabel = new System.Windows.Forms.Label();
            this.profileHeader = new System.Windows.Forms.Panel();
            this.addProfileButton = new System.Windows.Forms.Button();
            this.profileHeaderLabel = new System.Windows.Forms.Label();
            this.ProfileStackPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.helpHeader = new System.Windows.Forms.Panel();
            this.aboutShortcutsButton = new System.Windows.Forms.Button();
            this.aboutBugsButton = new System.Windows.Forms.Button();
            this.aboutFeedbackButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.helpArea = new System.Windows.Forms.Panel();
            this.aboutTwitterButton = new System.Windows.Forms.Button();
            this.aboutGitHubButton = new System.Windows.Forms.Button();
            this.aboutKofiButton = new System.Windows.Forms.Button();
            this.aboutDiscordButton = new System.Windows.Forms.Button();
            this.appHeader.SuspendLayout();
            this.applicationArea.SuspendLayout();
            this.browsingArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReloadNumeric)).BeginInit();
            this.browsingHeader.SuspendLayout();
            this.profileHeader.SuspendLayout();
            this.helpHeader.SuspendLayout();
            this.helpArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // appHeader
            // 
            this.appHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.appHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.appHeader.Controls.Add(this.appHeaderLabel);
            this.appHeader.Location = new System.Drawing.Point(12, 12);
            this.appHeader.Name = "appHeader";
            this.appHeader.Size = new System.Drawing.Size(360, 30);
            this.appHeader.TabIndex = 0;
            // 
            // appHeaderLabel
            // 
            this.appHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appHeaderLabel.AutoSize = true;
            this.appHeaderLabel.Location = new System.Drawing.Point(7, 7);
            this.appHeaderLabel.Name = "appHeaderLabel";
            this.appHeaderLabel.Size = new System.Drawing.Size(59, 13);
            this.appHeaderLabel.TabIndex = 0;
            this.appHeaderLabel.Text = "Application";
            // 
            // applicationArea
            // 
            this.applicationArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applicationArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.applicationArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.applicationArea.Controls.Add(this.StartupCheckbox);
            this.applicationArea.Controls.Add(this.UpdateCheckbox);
            this.applicationArea.Controls.Add(this.HideInTrayCheckbox);
            this.applicationArea.Location = new System.Drawing.Point(12, 47);
            this.applicationArea.Name = "applicationArea";
            this.applicationArea.Size = new System.Drawing.Size(360, 88);
            this.applicationArea.TabIndex = 1;
            // 
            // StartupCheckbox
            // 
            this.StartupCheckbox.AutoSize = true;
            this.StartupCheckbox.Location = new System.Drawing.Point(10, 58);
            this.StartupCheckbox.Name = "StartupCheckbox";
            this.StartupCheckbox.Size = new System.Drawing.Size(95, 17);
            this.StartupCheckbox.TabIndex = 2;
            this.StartupCheckbox.Text = "Start at startup";
            this.StartupCheckbox.UseVisualStyleBackColor = true;
            this.StartupCheckbox.CheckedChanged += new System.EventHandler(this.StartupCheckbox_CheckedChanged);
            // 
            // UpdateCheckbox
            // 
            this.UpdateCheckbox.AutoSize = true;
            this.UpdateCheckbox.Location = new System.Drawing.Point(10, 35);
            this.UpdateCheckbox.Name = "UpdateCheckbox";
            this.UpdateCheckbox.Size = new System.Drawing.Size(144, 17);
            this.UpdateCheckbox.TabIndex = 1;
            this.UpdateCheckbox.Text = "Check for updates online";
            this.UpdateCheckbox.UseVisualStyleBackColor = true;
            this.UpdateCheckbox.CheckedChanged += new System.EventHandler(this.UpdateCheckbox_CheckedChanged);
            // 
            // HideInTrayCheckbox
            // 
            this.HideInTrayCheckbox.AutoSize = true;
            this.HideInTrayCheckbox.Location = new System.Drawing.Point(10, 12);
            this.HideInTrayCheckbox.Name = "HideInTrayCheckbox";
            this.HideInTrayCheckbox.Size = new System.Drawing.Size(118, 17);
            this.HideInTrayCheckbox.TabIndex = 0;
            this.HideInTrayCheckbox.Text = "Hide on system tray";
            this.HideInTrayCheckbox.UseVisualStyleBackColor = true;
            this.HideInTrayCheckbox.CheckedChanged += new System.EventHandler(this.HideInTrayCheckbox_CheckedChanged);
            // 
            // browsingArea
            // 
            this.browsingArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browsingArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.browsingArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.browsingArea.Controls.Add(this.extensionsButton);
            this.browsingArea.Controls.Add(this.label3);
            this.browsingArea.Controls.Add(this.label2);
            this.browsingArea.Controls.Add(this.ReloadNumeric);
            this.browsingArea.Controls.Add(this.ReloadCheckbox);
            this.browsingArea.Controls.Add(this.ExternalCheckbox);
            this.browsingArea.Controls.Add(this.EnhancementCheckbox);
            this.browsingArea.Location = new System.Drawing.Point(12, 176);
            this.browsingArea.Name = "browsingArea";
            this.browsingArea.Size = new System.Drawing.Size(360, 110);
            this.browsingArea.TabIndex = 4;
            // 
            // extensionsButton
            // 
            this.extensionsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extensionsButton.Location = new System.Drawing.Point(279, 3);
            this.extensionsButton.Name = "extensionsButton";
            this.extensionsButton.Size = new System.Drawing.Size(76, 23);
            this.extensionsButton.TabIndex = 2;
            this.extensionsButton.Text = "Extensions";
            this.extensionsButton.UseVisualStyleBackColor = true;
            this.extensionsButton.Click += new System.EventHandler(this.extensionsButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Check for notification every";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(206, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "minutes";
            // 
            // ReloadNumeric
            // 
            this.ReloadNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ReloadNumeric.Location = new System.Drawing.Point(148, 79);
            this.ReloadNumeric.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.ReloadNumeric.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.ReloadNumeric.Name = "ReloadNumeric";
            this.ReloadNumeric.Size = new System.Drawing.Size(51, 20);
            this.ReloadNumeric.TabIndex = 3;
            this.ReloadNumeric.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.ReloadNumeric.ValueChanged += new System.EventHandler(this.ReloadNumeric_ValueChanged);
            // 
            // ReloadCheckbox
            // 
            this.ReloadCheckbox.AutoSize = true;
            this.ReloadCheckbox.Location = new System.Drawing.Point(10, 58);
            this.ReloadCheckbox.Name = "ReloadCheckbox";
            this.ReloadCheckbox.Size = new System.Drawing.Size(220, 17);
            this.ReloadCheckbox.TabIndex = 2;
            this.ReloadCheckbox.Text = "Check for notifications in the background";
            this.ReloadCheckbox.UseVisualStyleBackColor = true;
            this.ReloadCheckbox.CheckedChanged += new System.EventHandler(this.ReloadCheckbox_CheckedChanged);
            // 
            // ExternalCheckbox
            // 
            this.ExternalCheckbox.AutoSize = true;
            this.ExternalCheckbox.Location = new System.Drawing.Point(10, 35);
            this.ExternalCheckbox.Name = "ExternalCheckbox";
            this.ExternalCheckbox.Size = new System.Drawing.Size(220, 17);
            this.ExternalCheckbox.TabIndex = 1;
            this.ExternalCheckbox.Text = "Redirect new window to external browser";
            this.ExternalCheckbox.UseVisualStyleBackColor = true;
            this.ExternalCheckbox.CheckedChanged += new System.EventHandler(this.ExternalCheckbox_CheckedChanged);
            // 
            // EnhancementCheckbox
            // 
            this.EnhancementCheckbox.AutoSize = true;
            this.EnhancementCheckbox.Location = new System.Drawing.Point(10, 12);
            this.EnhancementCheckbox.Name = "EnhancementCheckbox";
            this.EnhancementCheckbox.Size = new System.Drawing.Size(140, 17);
            this.EnhancementCheckbox.TabIndex = 0;
            this.EnhancementCheckbox.Text = "Enable built-in extension";
            this.EnhancementCheckbox.UseVisualStyleBackColor = true;
            this.EnhancementCheckbox.CheckedChanged += new System.EventHandler(this.EnhancementCheckbox_CheckedChanged);
            // 
            // browsingHeader
            // 
            this.browsingHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browsingHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.browsingHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.browsingHeader.Controls.Add(this.browsingHeaderLabel);
            this.browsingHeader.Location = new System.Drawing.Point(12, 141);
            this.browsingHeader.Name = "browsingHeader";
            this.browsingHeader.Size = new System.Drawing.Size(360, 30);
            this.browsingHeader.TabIndex = 3;
            // 
            // browsingHeaderLabel
            // 
            this.browsingHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browsingHeaderLabel.AutoSize = true;
            this.browsingHeaderLabel.Location = new System.Drawing.Point(7, 7);
            this.browsingHeaderLabel.Name = "browsingHeaderLabel";
            this.browsingHeaderLabel.Size = new System.Drawing.Size(50, 13);
            this.browsingHeaderLabel.TabIndex = 0;
            this.browsingHeaderLabel.Text = "Browsing";
            // 
            // profileHeader
            // 
            this.profileHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profileHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.profileHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.profileHeader.Controls.Add(this.addProfileButton);
            this.profileHeader.Controls.Add(this.profileHeaderLabel);
            this.profileHeader.Location = new System.Drawing.Point(12, 292);
            this.profileHeader.Name = "profileHeader";
            this.profileHeader.Size = new System.Drawing.Size(360, 30);
            this.profileHeader.TabIndex = 4;
            // 
            // addProfileButton
            // 
            this.addProfileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addProfileButton.Location = new System.Drawing.Point(308, 2);
            this.addProfileButton.Name = "addProfileButton";
            this.addProfileButton.Size = new System.Drawing.Size(47, 23);
            this.addProfileButton.TabIndex = 1;
            this.addProfileButton.Text = "Add";
            this.addProfileButton.UseVisualStyleBackColor = true;
            this.addProfileButton.Click += new System.EventHandler(this.addProfileButton_Click);
            // 
            // profileHeaderLabel
            // 
            this.profileHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profileHeaderLabel.AutoSize = true;
            this.profileHeaderLabel.Location = new System.Drawing.Point(7, 7);
            this.profileHeaderLabel.Name = "profileHeaderLabel";
            this.profileHeaderLabel.Size = new System.Drawing.Size(41, 13);
            this.profileHeaderLabel.TabIndex = 0;
            this.profileHeaderLabel.Text = "Profiles";
            // 
            // ProfileStackPanel
            // 
            this.ProfileStackPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProfileStackPanel.AutoScroll = true;
            this.ProfileStackPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ProfileStackPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProfileStackPanel.Location = new System.Drawing.Point(12, 328);
            this.ProfileStackPanel.Name = "ProfileStackPanel";
            this.ProfileStackPanel.Size = new System.Drawing.Size(360, 90);
            this.ProfileStackPanel.TabIndex = 5;
            // 
            // helpHeader
            // 
            this.helpHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.helpHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.helpHeader.Controls.Add(this.aboutShortcutsButton);
            this.helpHeader.Controls.Add(this.aboutBugsButton);
            this.helpHeader.Controls.Add(this.aboutFeedbackButton);
            this.helpHeader.Controls.Add(this.label1);
            this.helpHeader.Location = new System.Drawing.Point(12, 424);
            this.helpHeader.Name = "helpHeader";
            this.helpHeader.Size = new System.Drawing.Size(360, 30);
            this.helpHeader.TabIndex = 5;
            // 
            // aboutShortcutsButton
            // 
            this.aboutShortcutsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutShortcutsButton.Location = new System.Drawing.Point(119, 2);
            this.aboutShortcutsButton.Name = "aboutShortcutsButton";
            this.aboutShortcutsButton.Size = new System.Drawing.Size(78, 23);
            this.aboutShortcutsButton.TabIndex = 3;
            this.aboutShortcutsButton.Text = "Shortcuts";
            this.aboutShortcutsButton.UseVisualStyleBackColor = true;
            this.aboutShortcutsButton.Click += new System.EventHandler(this.aboutShortcutsButton_Click);
            // 
            // aboutBugsButton
            // 
            this.aboutBugsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutBugsButton.Location = new System.Drawing.Point(199, 2);
            this.aboutBugsButton.Name = "aboutBugsButton";
            this.aboutBugsButton.Size = new System.Drawing.Size(78, 23);
            this.aboutBugsButton.TabIndex = 2;
            this.aboutBugsButton.Text = "Bug Report";
            this.aboutBugsButton.UseVisualStyleBackColor = true;
            this.aboutBugsButton.Click += new System.EventHandler(this.aboutBugsButton_Click);
            // 
            // aboutFeedbackButton
            // 
            this.aboutFeedbackButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutFeedbackButton.Location = new System.Drawing.Point(279, 2);
            this.aboutFeedbackButton.Name = "aboutFeedbackButton";
            this.aboutFeedbackButton.Size = new System.Drawing.Size(78, 23);
            this.aboutFeedbackButton.TabIndex = 1;
            this.aboutFeedbackButton.Text = "Feedback";
            this.aboutFeedbackButton.UseVisualStyleBackColor = true;
            this.aboutFeedbackButton.Click += new System.EventHandler(this.aboutFeedbackButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Help";
            // 
            // helpArea
            // 
            this.helpArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.helpArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.helpArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.helpArea.Controls.Add(this.aboutTwitterButton);
            this.helpArea.Controls.Add(this.aboutGitHubButton);
            this.helpArea.Controls.Add(this.aboutKofiButton);
            this.helpArea.Controls.Add(this.aboutDiscordButton);
            this.helpArea.Location = new System.Drawing.Point(12, 460);
            this.helpArea.Name = "helpArea";
            this.helpArea.Size = new System.Drawing.Size(360, 33);
            this.helpArea.TabIndex = 6;
            // 
            // aboutTwitterButton
            // 
            this.aboutTwitterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutTwitterButton.Location = new System.Drawing.Point(224, 4);
            this.aboutTwitterButton.Name = "aboutTwitterButton";
            this.aboutTwitterButton.Size = new System.Drawing.Size(72, 23);
            this.aboutTwitterButton.TabIndex = 6;
            this.aboutTwitterButton.Text = "Twitter";
            this.aboutTwitterButton.UseVisualStyleBackColor = true;
            this.aboutTwitterButton.Click += new System.EventHandler(this.aboutTwitterButton_Click);
            // 
            // aboutGitHubButton
            // 
            this.aboutGitHubButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutGitHubButton.Location = new System.Drawing.Point(150, 4);
            this.aboutGitHubButton.Name = "aboutGitHubButton";
            this.aboutGitHubButton.Size = new System.Drawing.Size(72, 23);
            this.aboutGitHubButton.TabIndex = 5;
            this.aboutGitHubButton.Text = "GitHub";
            this.aboutGitHubButton.UseVisualStyleBackColor = true;
            this.aboutGitHubButton.Click += new System.EventHandler(this.aboutGitHubButton_Click);
            // 
            // aboutKofiButton
            // 
            this.aboutKofiButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutKofiButton.Location = new System.Drawing.Point(76, 4);
            this.aboutKofiButton.Name = "aboutKofiButton";
            this.aboutKofiButton.Size = new System.Drawing.Size(72, 23);
            this.aboutKofiButton.TabIndex = 4;
            this.aboutKofiButton.Text = "Ko-fi";
            this.aboutKofiButton.UseVisualStyleBackColor = true;
            this.aboutKofiButton.Click += new System.EventHandler(this.aboutKofiButton_Click);
            // 
            // aboutDiscordButton
            // 
            this.aboutDiscordButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutDiscordButton.Location = new System.Drawing.Point(2, 4);
            this.aboutDiscordButton.Name = "aboutDiscordButton";
            this.aboutDiscordButton.Size = new System.Drawing.Size(72, 23);
            this.aboutDiscordButton.TabIndex = 3;
            this.aboutDiscordButton.Text = "Discord";
            this.aboutDiscordButton.UseVisualStyleBackColor = true;
            this.aboutDiscordButton.Click += new System.EventHandler(this.aboutDiscordButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(384, 505);
            this.Controls.Add(this.helpArea);
            this.Controls.Add(this.helpHeader);
            this.Controls.Add(this.ProfileStackPanel);
            this.Controls.Add(this.profileHeader);
            this.Controls.Add(this.browsingArea);
            this.Controls.Add(this.applicationArea);
            this.Controls.Add(this.browsingHeader);
            this.Controls.Add(this.appHeader);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Itaku: Desktop Settings";
            this.appHeader.ResumeLayout(false);
            this.appHeader.PerformLayout();
            this.applicationArea.ResumeLayout(false);
            this.applicationArea.PerformLayout();
            this.browsingArea.ResumeLayout(false);
            this.browsingArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ReloadNumeric)).EndInit();
            this.browsingHeader.ResumeLayout(false);
            this.browsingHeader.PerformLayout();
            this.profileHeader.ResumeLayout(false);
            this.profileHeader.PerformLayout();
            this.helpHeader.ResumeLayout(false);
            this.helpHeader.PerformLayout();
            this.helpArea.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel appHeader;
        private System.Windows.Forms.Label appHeaderLabel;
        private System.Windows.Forms.Panel applicationArea;
        private System.Windows.Forms.CheckBox HideInTrayCheckbox;
        private System.Windows.Forms.CheckBox UpdateCheckbox;
        private System.Windows.Forms.CheckBox StartupCheckbox;
        private System.Windows.Forms.Panel browsingArea;
        private System.Windows.Forms.CheckBox ReloadCheckbox;
        private System.Windows.Forms.CheckBox ExternalCheckbox;
        private System.Windows.Forms.CheckBox EnhancementCheckbox;
        private System.Windows.Forms.Panel browsingHeader;
        private System.Windows.Forms.Label browsingHeaderLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ReloadNumeric;
        private System.Windows.Forms.Panel profileHeader;
        private System.Windows.Forms.Label profileHeaderLabel;
        private System.Windows.Forms.Button addProfileButton;
        private System.Windows.Forms.FlowLayoutPanel ProfileStackPanel;
        private System.Windows.Forms.Panel helpHeader;
        private System.Windows.Forms.Button aboutFeedbackButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel helpArea;
        private System.Windows.Forms.Button aboutBugsButton;
        private System.Windows.Forms.Button aboutDiscordButton;
        private System.Windows.Forms.Button aboutShortcutsButton;
        private System.Windows.Forms.Button aboutKofiButton;
        private System.Windows.Forms.Button aboutGitHubButton;
        private System.Windows.Forms.Button aboutTwitterButton;
        private System.Windows.Forms.Button extensionsButton;
    }
}