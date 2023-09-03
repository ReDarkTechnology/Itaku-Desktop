using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ItakuDesktop.Tools
{
    public partial class ExtensionForm : Form
    {
        private static ExtensionForm _self;
        public static ExtensionForm self
        {
            get
            {
                _self = _self ?? new ExtensionForm();
                return _self;
            }
            set
            {
                _self = value;
            }
        }
        public List<ExtensionListItem> items = new List<ExtensionListItem>();

        public ExtensionForm()
        {
            InitializeComponent();
            FormClosed += (s, e) => { if (_self == this) _self = null; };

            Shown += (s, e) =>
            {
                foreach (var host in ExtensionManager.hosts)
                {
                    var item = new ExtensionListItem(host);
                    items.Add(item);
                    item.AdjustSelf();
                    extensionFlowLayout.Controls.Add(item);
                }
            };
        }

        private void extensionFlowLayout_Resize(object sender, EventArgs e)
        {
            foreach(Control control in extensionFlowLayout.Controls)
                control.Size = new Size(extensionFlowLayout.Size.Width - 10, control.Size.Height);
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            foreach(var item in items)
                item.Dispose();
            items.Clear();

            ExtensionManager.Refresh();
            foreach (var host in ExtensionManager.hosts)
            {
                var item = new ExtensionListItem(host);
                items.Add(item);
                item.AdjustSelf();
                extensionFlowLayout.Controls.Add(item);
            }
        }

        private void disableAllButton_Click(object sender, EventArgs e)
        {
            foreach(var item in items)
                item.nameEnableBox.Checked = false;
        }

        private void enableAllButton_Click(object sender, EventArgs e)
        {
            foreach (var item in items)
                item.nameEnableBox.Checked = true;
        }
    }
}
