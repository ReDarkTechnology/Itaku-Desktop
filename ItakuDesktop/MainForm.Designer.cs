namespace ItakuDesktop
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.browserPanel = new System.Windows.Forms.Panel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.downloadsButton = new ItakuDesktop.RoundedButton();
            this.favoritesButton = new ItakuDesktop.RoundedButton();
            this.moreButton = new ItakuDesktop.RoundedButton();
            this.urlBox = new ItakuDesktop.StylizedTextBox();
            this.reloadButton = new ItakuDesktop.RoundedButton();
            this.nextButton = new ItakuDesktop.RoundedButton();
            this.previousButton = new ItakuDesktop.RoundedButton();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // browserPanel
            // 
            this.browserPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserPanel.ForeColor = System.Drawing.Color.White;
            this.browserPanel.Location = new System.Drawing.Point(0, 35);
            this.browserPanel.Name = "browserPanel";
            this.browserPanel.Size = new System.Drawing.Size(800, 415);
            this.browserPanel.TabIndex = 0;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(149)))), ((int)(((byte)(221)))), ((int)(((byte)(255)))));
            this.topPanel.Controls.Add(this.downloadsButton);
            this.topPanel.Controls.Add(this.favoritesButton);
            this.topPanel.Controls.Add(this.moreButton);
            this.topPanel.Controls.Add(this.urlBox);
            this.topPanel.Controls.Add(this.reloadButton);
            this.topPanel.Controls.Add(this.nextButton);
            this.topPanel.Controls.Add(this.previousButton);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 35);
            this.topPanel.TabIndex = 1;
            // 
            // downloadsButton
            // 
            this.downloadsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadsButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.downloadsButton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.downloadsButton.BorderColor = System.Drawing.Color.Black;
            this.downloadsButton.BorderRadius = 5;
            this.downloadsButton.BorderSize = 1;
            this.downloadsButton.FlatAppearance.BorderSize = 0;
            this.downloadsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.downloadsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.downloadsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.downloadsButton.ForeColor = System.Drawing.Color.Black;
            this.downloadsButton.Location = new System.Drawing.Point(717, 4);
            this.downloadsButton.Name = "downloadsButton";
            this.downloadsButton.OnDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.downloadsButton.OnHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.downloadsButton.Size = new System.Drawing.Size(25, 25);
            this.downloadsButton.TabIndex = 6;
            this.downloadsButton.Text = "⬇";
            this.downloadsButton.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.downloadsButton.TextColor = System.Drawing.Color.Black;
            this.downloadsButton.UseVisualStyleBackColor = false;
            // 
            // favoritesButton
            // 
            this.favoritesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.favoritesButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.favoritesButton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.favoritesButton.BorderColor = System.Drawing.Color.Black;
            this.favoritesButton.BorderRadius = 5;
            this.favoritesButton.BorderSize = 1;
            this.favoritesButton.FlatAppearance.BorderSize = 0;
            this.favoritesButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.favoritesButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.favoritesButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.favoritesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.favoritesButton.ForeColor = System.Drawing.Color.Black;
            this.favoritesButton.Location = new System.Drawing.Point(744, 4);
            this.favoritesButton.Name = "favoritesButton";
            this.favoritesButton.OnDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.favoritesButton.OnHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.favoritesButton.Size = new System.Drawing.Size(25, 25);
            this.favoritesButton.TabIndex = 5;
            this.favoritesButton.Text = "☆";
            this.favoritesButton.TextColor = System.Drawing.Color.Black;
            this.favoritesButton.UseVisualStyleBackColor = false;
            // 
            // moreButton
            // 
            this.moreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.moreButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.moreButton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.moreButton.BorderColor = System.Drawing.Color.Black;
            this.moreButton.BorderRadius = 5;
            this.moreButton.BorderSize = 1;
            this.moreButton.FlatAppearance.BorderSize = 0;
            this.moreButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.moreButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.moreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.moreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moreButton.ForeColor = System.Drawing.Color.Black;
            this.moreButton.Location = new System.Drawing.Point(771, 4);
            this.moreButton.Name = "moreButton";
            this.moreButton.OnDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.moreButton.OnHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.moreButton.Size = new System.Drawing.Size(25, 25);
            this.moreButton.TabIndex = 4;
            this.moreButton.Text = "⋯";
            this.moreButton.TextColor = System.Drawing.Color.Black;
            this.moreButton.UseVisualStyleBackColor = false;
            this.moreButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // urlBox
            // 
            this.urlBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.urlBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.urlBox.ForeColor = System.Drawing.Color.Black;
            this.urlBox.Location = new System.Drawing.Point(86, 6);
            this.urlBox.Margin = new System.Windows.Forms.Padding(0);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(625, 22);
            this.urlBox.TabIndex = 3;
            this.urlBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.urlBox_KeyDown);
            // 
            // reloadButton
            // 
            this.reloadButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.reloadButton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.reloadButton.BorderColor = System.Drawing.Color.Black;
            this.reloadButton.BorderRadius = 5;
            this.reloadButton.BorderSize = 1;
            this.reloadButton.FlatAppearance.BorderSize = 0;
            this.reloadButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.reloadButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.reloadButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reloadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reloadButton.ForeColor = System.Drawing.Color.Black;
            this.reloadButton.Location = new System.Drawing.Point(55, 4);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.OnDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.reloadButton.OnHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.reloadButton.Size = new System.Drawing.Size(25, 25);
            this.reloadButton.TabIndex = 2;
            this.reloadButton.Text = "@";
            this.reloadButton.TextColor = System.Drawing.Color.Black;
            this.reloadButton.UseVisualStyleBackColor = false;
            this.reloadButton.Click += new System.EventHandler(this.reloadButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.nextButton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.nextButton.BorderColor = System.Drawing.Color.Black;
            this.nextButton.BorderRadius = 5;
            this.nextButton.BorderSize = 1;
            this.nextButton.FlatAppearance.BorderSize = 0;
            this.nextButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nextButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nextButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextButton.ForeColor = System.Drawing.Color.Black;
            this.nextButton.Location = new System.Drawing.Point(29, 4);
            this.nextButton.Name = "nextButton";
            this.nextButton.OnDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nextButton.OnHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nextButton.Size = new System.Drawing.Size(25, 25);
            this.nextButton.TabIndex = 1;
            this.nextButton.Text = ">";
            this.nextButton.TextColor = System.Drawing.Color.Black;
            this.nextButton.UseVisualStyleBackColor = false;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // previousButton
            // 
            this.previousButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.previousButton.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.previousButton.BorderColor = System.Drawing.Color.Black;
            this.previousButton.BorderRadius = 5;
            this.previousButton.BorderSize = 1;
            this.previousButton.FlatAppearance.BorderSize = 0;
            this.previousButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.previousButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.previousButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.previousButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.previousButton.ForeColor = System.Drawing.Color.Black;
            this.previousButton.Location = new System.Drawing.Point(3, 4);
            this.previousButton.Name = "previousButton";
            this.previousButton.OnDownColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.previousButton.OnHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.previousButton.Size = new System.Drawing.Size(25, 25);
            this.previousButton.TabIndex = 0;
            this.previousButton.Text = "<";
            this.previousButton.TextColor = System.Drawing.Color.Black;
            this.previousButton.UseVisualStyleBackColor = false;
            this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.browserPanel);
            this.Controls.Add(this.topPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Itaku";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel browserPanel;
        private System.Windows.Forms.Panel topPanel;
        private ItakuDesktop.RoundedButton previousButton;
        private ItakuDesktop.RoundedButton nextButton;
        private ItakuDesktop.RoundedButton reloadButton;
        private StylizedTextBox urlBox;
        private RoundedButton moreButton;
        private RoundedButton favoritesButton;
        private RoundedButton downloadsButton;
    }
}

