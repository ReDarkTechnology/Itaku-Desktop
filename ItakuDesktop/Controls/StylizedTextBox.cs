using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ItakuDesktop
{
	public partial class StylizedTextBox : UserControl
	{

        public override Font Font { get => base.Font;
            set
            {
                base.Font = value;
                TypingBox.Font = value;
            }
        }

        public override Color BackColor
        {
			get => base.BackColor;
			set
            {
				base.BackColor = value;
				TypingBox.BackColor = value;
			}
		}
		
		public override Color ForeColor
        {
			get => base.ForeColor;
			set {
				base.ForeColor = value;
                TypingBox.ForeColor = value;
			}
        }

        [Category("TextBox")]
        public override string Text
        {
            get
            {
                return TypingBox.Text;
            }
            set
            {
                TypingBox.Text = value;
            }
        }

        public StylizedTextBox()
		{
			InitializeComponent();
            ControlUtil.AddBorder(this);
			TypingBox.KeyDown += (ee, aa) => OnKeyDown(aa);
		}

		void BorderClick(object sender, EventArgs e)
		{
			TypingBox.Focus();
		}
	}
}
