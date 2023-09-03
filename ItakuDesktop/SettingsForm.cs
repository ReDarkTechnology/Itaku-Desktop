using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ItakuDesktop
{
    public partial class SettingsForm : Form
    {
        bool isUpdating;
        public SettingsForm()
        {
            InitializeComponent();
            FormClosed += (s, e) => MainForm.settings = null;
        }

        public void UpdateValues()
        {
            isUpdating = true;
            ReloadNumeric.Value = MainForm.self.reloadInterval < 5 ? 5 : MainForm.self.reloadInterval > 120 ? 120 : MainForm.self.reloadInterval;
            ReloadCheckbox.Checked = MainForm.self.reloadToCheckNotification;
            HideInTrayCheckbox.Checked = MainForm.self.hideToTray;
            EnhancementCheckbox.Checked = MainForm.self.isEnhanced;
            StartupCheckbox.Checked = MainForm.self.isAtStartup;
            UpdateCheckbox.Checked = MainForm.self.checkForUpdates;
            ExternalCheckbox.Checked = MainForm.self.goToExternal;
            isUpdating = false;

            ProfileStackPanel.Controls.Clear();
            var paths = Directory.GetDirectories(PathFixer.startPath);
            foreach (var path in paths)
                AddFolder(path);
        }

        private void HideInTrayCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            MainForm.self.hideToTray = HideInTrayCheckbox.Checked;
            MainForm.self.SaveSettings();
        }

        private void UpdateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            MainForm.self.checkForUpdates = UpdateCheckbox.Checked;
            MainForm.self.SaveSettings();
        }

        private void StartupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            MainForm.self.SetStartup(StartupCheckbox.Checked);
            MainForm.self.SaveSettings();
        }

        private async void EnhancementCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            isUpdating = true;
            MainForm.self.isEnhanced = EnhancementCheckbox.Checked;
            MainForm.self.SaveSettings();
            isUpdating = false;
        }

        private void ExternalCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            MainForm.self.goToExternal = ExternalCheckbox.Checked;
            MainForm.self.SaveSettings();
        }

        private void ReloadCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            MainForm.self.SetReload(ReloadCheckbox.Checked);
            MainForm.self.SaveSettings();
        }

        private void ReloadNumeric_ValueChanged(object sender, EventArgs e)
        {
            if (isUpdating) return;
            MainForm.self.reloadInterval = (int)ReloadNumeric.Value;
            MainForm.self.SaveSettings();
        }

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            var addDialog = new AddProfile();
            addDialog.func += AddProfile;
            addDialog.Show();
        }

        public void AddFolder(string path)
        {
            var infoPath = Path.Combine(path, "profile.json");
            Console.WriteLine("INFO: Checking: " + infoPath);
            if (File.Exists(infoPath))
            {
                var info = ProfileInfo.Read(infoPath);
                var button = new Button();
                button.Text = info.name == "_Default_INTERNAL" ? "Default" : info.name;
                button.Height = 25;
                button.BackColor = Color.FromArgb(255, 40, 40, 40);
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
                button.Click += (e, a) => {
                    Process.Start(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:\\", ""), $"\"{path}\"");
                };
                if (info.name == "_Default_INTERNAL")
                {
                    ProfileStackPanel.Controls.Add(button);
                    ProfileStackPanel.Controls.SetChildIndex(button, 0);
                }
                else
                {
                    ProfileStackPanel.Controls.Add(button);
                }
                Console.WriteLine("INFO: Added: " + info.name);
            }
        }

        public AddNameArgs AddProfile(string name)
        {
            name = SanitizeName(name);
            var path = name.FixPath();
            if (name == "_Default_INTERNAL")
            {
                return new AddNameArgs()
                {
                    isSuccessful = false,
                    errorMessage = "This name isn't allowed " + name
                };
            }

            if (Directory.Exists(path))
            {
                return new AddNameArgs()
                {
                    isSuccessful = false,
                    errorMessage = "We can't create a new profile in: " + path
                };
            }
            else
            {
                Directory.CreateDirectory(path);
                var info = new ProfileInfo() { name = name, path = path };
                info.Save(Path.Combine(path, "profile.json"));
                AddFolder(path);
                return new AddNameArgs()
                {
                    isSuccessful = true
                };
            }
        }

        public string SanitizeName(string name)
        {
            foreach (var ch in Path.GetInvalidPathChars())
                name = name.Replace(ch, char.MinValue);
            return name;
        }

        private void aboutDiscordButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/cWWnEgHUa3");
        }

        private void aboutKofiButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://ko-fi.com/bunzhizendi");
        }

        private void aboutGitHubButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ReDarkTechnology/Itaku-Desktop");
            // Process.Start("");
        }

        private void aboutTwitterButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://twitter.com/bunzhida");
        }

        private void extensionsButton_Click(object sender, EventArgs e)
        {
            Tools.ExtensionForm.self.Show();
        }

        private void aboutBugsButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ReDarkTechnology/Itaku-Desktop/issues/new");
        }

        private void aboutFeedbackButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ReDarkTechnology/Itaku-Desktop/issues/new");
        }

        private void aboutShortcutsButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shortcuts: \nF3 - Find on page\nF4 - Hide toolbar\nF5 - Refresh page\nCtrl + L - Go to url bar\nCtrl + Shift + I - Inspect Tools", "Itaku: Shortcuts");
        }
    }
}
