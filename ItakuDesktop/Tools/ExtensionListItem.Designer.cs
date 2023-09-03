namespace ItakuDesktop.Tools
{
    partial class ExtensionListItem
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pathLabel = new System.Windows.Forms.Label();
            this.nameEnableBox = new System.Windows.Forms.CheckBox();
            this.consoleButton = new System.Windows.Forms.Button();
            this.versionLabel = new System.Windows.Forms.Label();
            this.folderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pathLabel
            // 
            this.pathLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathLabel.AutoEllipsis = true;
            this.pathLabel.Location = new System.Drawing.Point(24, 27);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(175, 13);
            this.pathLabel.TabIndex = 1;
            this.pathLabel.Text = "Path";
            // 
            // nameEnableBox
            // 
            this.nameEnableBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameEnableBox.AutoEllipsis = true;
            this.nameEnableBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.nameEnableBox.Location = new System.Drawing.Point(8, 7);
            this.nameEnableBox.Name = "nameEnableBox";
            this.nameEnableBox.Size = new System.Drawing.Size(262, 21);
            this.nameEnableBox.TabIndex = 2;
            this.nameEnableBox.Text = "Name";
            this.nameEnableBox.UseVisualStyleBackColor = true;
            this.nameEnableBox.CheckedChanged += new System.EventHandler(this.nameEnableBox_CheckedChanged);
            // 
            // consoleButton
            // 
            this.consoleButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.consoleButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.consoleButton.Location = new System.Drawing.Point(259, 26);
            this.consoleButton.Name = "consoleButton";
            this.consoleButton.Size = new System.Drawing.Size(59, 23);
            this.consoleButton.TabIndex = 3;
            this.consoleButton.Text = "Console";
            this.consoleButton.UseVisualStyleBackColor = true;
            this.consoleButton.Click += new System.EventHandler(this.consoleButton_Click);
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(276, 7);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(42, 13);
            this.versionLabel.TabIndex = 4;
            this.versionLabel.Text = "Version";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // folderButton
            // 
            this.folderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.folderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.folderButton.Location = new System.Drawing.Point(205, 26);
            this.folderButton.Name = "folderButton";
            this.folderButton.Size = new System.Drawing.Size(51, 23);
            this.folderButton.TabIndex = 5;
            this.folderButton.Text = "Folder";
            this.folderButton.UseVisualStyleBackColor = true;
            this.folderButton.Click += new System.EventHandler(this.folderButton_Click);
            // 
            // ExtensionListItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.folderButton);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.consoleButton);
            this.Controls.Add(this.nameEnableBox);
            this.Controls.Add(this.pathLabel);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ExtensionListItem";
            this.Size = new System.Drawing.Size(322, 53);
            this.Load += new System.EventHandler(this.ExtensionListItem_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Button consoleButton;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Button folderButton;
        public System.Windows.Forms.CheckBox nameEnableBox;
    }
}
