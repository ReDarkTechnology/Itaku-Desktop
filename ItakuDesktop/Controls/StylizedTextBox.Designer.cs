/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 2/10/2022
 * Time: 6:29 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ItakuDesktop
{
	partial class StylizedTextBox
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the control.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.TypingBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TypingBox
            // 
            this.TypingBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TypingBox.AutoCompleteCustomSource.AddRange(new string[] {
            "https://itaku.ee/",
            "https://itaku.ee/home",
            "https://itaku.ee/dms",
            "https://itaku.ee/tags",
            "https://itaku.ee/commission-inbox",
            "https://itaku.ee/tag-suggestions",
            "https://itaku.ee/submission-inbox",
            "https://itaku.ee/about",
            "https://itaku.ee/help",
            "https://itaku.ee/help/advertise",
            "https://itaku.ee/new/post",
            "https://itaku.ee/new/video",
            "https://itaku.ee/new/images",
            "https://itaku.ee/new/image",
            "https://itaku.ee/new/commission",
            "https://itaku.ee/new/tag",
            "https://itaku.ee/settings/account"});
            this.TypingBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.TypingBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TypingBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.TypingBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TypingBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.TypingBox.Location = new System.Drawing.Point(4, 5);
            this.TypingBox.Margin = new System.Windows.Forms.Padding(0);
            this.TypingBox.Name = "TypingBox";
            this.TypingBox.Size = new System.Drawing.Size(100, 13);
            this.TypingBox.TabIndex = 0;
            // 
            // StylizedTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.Controls.Add(this.TypingBox);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "StylizedTextBox";
            this.Size = new System.Drawing.Size(108, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        public System.Windows.Forms.TextBox TypingBox;
    }
}
