using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace ItakuDesktop.Tools
{
    public partial class ExtensionListItem : UserControl
    {
        public ExtensionHost host;
        public ExtensionErrorForm errorForm;

        public ExtensionListItem()
        {
            InitializeComponent();
            Disposed += (s, err) => { if (errorForm != null) errorForm.Close(); };
        }

        public ExtensionListItem(ExtensionHost host)
        {
            InitializeComponent();
            this.host = host;

            nameEnableBox.Checked = host.enabled;
            nameEnableBox.Text = host.manifest.name;
            pathLabel.Text = host.directoryPath;
            versionLabel.Text = "v" + host.manifest.version.ToString();

            
        }

        private void folderButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(host.directoryPath))
                Process.Start(host.directoryPath);
        }

        private void consoleButton_Click(object sender, EventArgs e)
        {
            if(errorForm == null)
                errorForm = new ExtensionErrorForm(this);
            errorForm.Show();
        }

        public void AdjustSelf()
        {
            if(Parent != null)
            {
                Size = new System.Drawing.Size(Parent.Width - 10, Size.Height);
            }
        }

        private void nameEnableBox_CheckedChanged(object sender, EventArgs e)
        {
            host.enabled = nameEnableBox.Checked;
        }

        private void ExtensionListItem_Load(object sender, EventArgs e)
        {
            AdjustSelf();
        }
    }
}
