using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ItakuDesktop
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        bool isUpdating;
        public SettingsWindow()
        {
            isUpdating = true;
            InitializeComponent();
            isUpdating = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow.settings = null;
        }

        public void UpdateValues()
        {
            isUpdating = true;
            ReloadSlider.Value = MainWindow.window.reloadInterval;
            ReloadTextBox.Text = MainWindow.window.reloadInterval.ToString();
            ReloadCheckbox.IsChecked = MainWindow.window.reloadToCheckNotification;
            HideInTrayCheckbox.IsChecked = MainWindow.window.hideToTray;
            EnhancementCheckbox.IsChecked = MainWindow.window.isEnhanced;
            StartupCheckbox.IsChecked = MainWindow.window.isAtStartup;
            UpdateCheckbox.IsChecked = MainWindow.window.checkForUpdates;
            ExternalCheckbox.IsChecked = MainWindow.window.goToExternal;
            isUpdating = false;

            ProfileStackPanel.Children.Clear();
            var paths = Directory.GetDirectories(PathFixer.startPath);
            Console.WriteLine("INFO: " + string.Join("\n", paths));
            foreach (var path in paths)
                AddFolder(path);
        }

        // Reload inverval
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isUpdating) return;
            MainWindow.window.reloadInterval = (int)ReloadSlider.Value;
            ReloadTextBox.Text = ReloadSlider.Value.ToString();
            MainWindow.window.SaveSettings();
        }

        // Reload interval
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isUpdating) return;
            isUpdating = true;
            if (double.TryParse(ReloadTextBox.Text, out double val))
                ReloadSlider.Value = val;
            MainWindow.window.SaveSettings();
            isUpdating = false;
        }

        // Auto-reload
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (isUpdating) return;
            MainWindow.window.SetReload(ReloadCheckbox.IsChecked.HasValue ? ReloadCheckbox.IsChecked.Value : false);
            MainWindow.window.SaveSettings();
        }

        // Enhancement
        private async void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            if (isUpdating) return;
            isUpdating = true;
            bool to = await MainWindow.window.LoadEnhancementExtension();
            EnhancementCheckbox.IsChecked = to;
            MainWindow.window.SaveSettings();
            isUpdating = false;
        }

        // Hide in tray
        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            if (isUpdating) return;
            MainWindow.window.hideToTray = HideInTrayCheckbox.IsChecked.HasValue ? HideInTrayCheckbox.IsChecked.Value : false;
            MainWindow.window.SaveSettings();
        }

        // Startup
        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            if (isUpdating) return;
            MainWindow.window.SetStartup(StartupCheckbox.IsChecked.HasValue ? StartupCheckbox.IsChecked.Value : false);
            MainWindow.window.SaveSettings();
        }

        // Update
        private void CheckBox_Checked_4(object sender, RoutedEventArgs e)
        {
            if (isUpdating) return;
            MainWindow.window.checkForUpdates = UpdateCheckbox.IsChecked.HasValue ? UpdateCheckbox.IsChecked.Value : false;
            MainWindow.window.SaveSettings();
        }

        // External Browser
        private void CheckBox_Checked_5(object sender, RoutedEventArgs e)
        {
            if (isUpdating) return;
            MainWindow.window.goToExternal = ExternalCheckbox.IsChecked.HasValue ? ExternalCheckbox.IsChecked.Value : false;
            MainWindow.window.SaveSettings();
        }

        private void AddProfileButton_Click(object sender, RoutedEventArgs e)
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
                button.Content = info.name == "_Default_INTERNAL" ? "Default" : info.name;
                button.Height = 25;
                button.Click += (e, a) => {
                    Process.Start(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Replace("file:\\", ""), $"\"{path}\"");
                };
                if (info.name == "_Default_INTERNAL")
                    ProfileStackPanel.Children.Insert(0, button);
                else
                    ProfileStackPanel.Children.Add(button);
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
    }
}
