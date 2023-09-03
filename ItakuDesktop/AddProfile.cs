using System;
using System.Windows.Forms;

namespace ItakuDesktop
{
    public partial class AddProfile : Form
    {
        public Func<string, AddNameArgs> func;
        public AddProfile()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            var result = func.Invoke(nameBox.Text);
            if(result.isSuccessful)
                Close();
            else
                infoLabel.Text = result.errorMessage;
        }
    }

    public class AddNameArgs
    {
        public bool isSuccessful;
        public string errorMessage;
    }
}
