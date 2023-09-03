using System;
using System.Windows.Forms;

namespace ItakuDesktop.Tools
{
    public partial class ExtensionErrorForm : Form
    {
        public ExtensionHost host;
        public ExtensionListItem item;
        public ExtensionErrorForm()
        {
            InitializeComponent();
        }

        public void OnLog(object sender, ExtensionLog error)
        {
            Invoke(new Action(() =>
            {
                errorList.Items.Add(new ListViewItem(new string[] { error.type.ToString(), error.message }) { Tag = error });
            }));
        }

        public ExtensionErrorForm(ExtensionListItem listItem)
        {
            InitializeComponent();

            Shown += (s, e) =>
            {
                item = listItem;
                host = listItem.host;
                Text = $"Itaku: {host.manifest.name} Console";
                titleLabel.Text = Text;
                host.onLog += OnLog;
                FormClosed += ExtensionErrorForm_FormClosed;
                var errors = host.GetLogs();
                foreach (var error in errors)
                    OnLog(host, error);
            };
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            host.ClearLogs();
            errorList.Items.Clear();
        }

        private void ExtensionErrorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            item.errorForm = null;
            host.onLog -= OnLog;
        }

        private void errorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(errorList.SelectedItems.Count > 0)
            {
                var item = errorList.SelectedItems[0];
                var log = (ExtensionLog)item.Tag;
                errorBox.Text = log.message;
            }
        }
    }
}
