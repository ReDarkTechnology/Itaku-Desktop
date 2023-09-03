namespace ItakuDesktop.Tools
{
    partial class ExtensionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionForm));
            this.reloadButton = new System.Windows.Forms.Button();
            this.disableAllButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.enableAllButton = new System.Windows.Forms.Button();
            this.extensionFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // reloadButton
            // 
            this.reloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.reloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reloadButton.Location = new System.Drawing.Point(320, 12);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 0;
            this.reloadButton.Text = "Reload All";
            this.reloadButton.UseVisualStyleBackColor = true;
            this.reloadButton.Click += new System.EventHandler(this.reloadButton_Click);
            // 
            // disableAllButton
            // 
            this.disableAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.disableAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.disableAllButton.Location = new System.Drawing.Point(242, 12);
            this.disableAllButton.Name = "disableAllButton";
            this.disableAllButton.Size = new System.Drawing.Size(72, 23);
            this.disableAllButton.TabIndex = 1;
            this.disableAllButton.Text = "Disable All";
            this.disableAllButton.UseVisualStyleBackColor = true;
            this.disableAllButton.Click += new System.EventHandler(this.disableAllButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 17);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(98, 13);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Extension Manager";
            // 
            // enableAllButton
            // 
            this.enableAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.enableAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enableAllButton.Location = new System.Drawing.Point(164, 12);
            this.enableAllButton.Name = "enableAllButton";
            this.enableAllButton.Size = new System.Drawing.Size(72, 23);
            this.enableAllButton.TabIndex = 3;
            this.enableAllButton.Text = "Enable All";
            this.enableAllButton.UseVisualStyleBackColor = true;
            this.enableAllButton.Click += new System.EventHandler(this.enableAllButton_Click);
            // 
            // extensionFlowLayout
            // 
            this.extensionFlowLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionFlowLayout.AutoScroll = true;
            this.extensionFlowLayout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.extensionFlowLayout.Location = new System.Drawing.Point(12, 41);
            this.extensionFlowLayout.Name = "extensionFlowLayout";
            this.extensionFlowLayout.Size = new System.Drawing.Size(383, 379);
            this.extensionFlowLayout.TabIndex = 4;
            this.extensionFlowLayout.Resize += new System.EventHandler(this.extensionFlowLayout_Resize);
            // 
            // ExtensionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(407, 432);
            this.Controls.Add(this.extensionFlowLayout);
            this.Controls.Add(this.enableAllButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.disableAllButton);
            this.Controls.Add(this.reloadButton);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtensionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Itaku: Extension Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.Button disableAllButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button enableAllButton;
        private System.Windows.Forms.FlowLayoutPanel extensionFlowLayout;
    }
}