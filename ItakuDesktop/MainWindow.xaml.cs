#region Usings
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using Microsoft.Win32;
using System.Threading;
using HtmlAgilityPack;

using NotifyIcon = System.Windows.Forms.NotifyIcon;
using ContextMenuStrip = System.Windows.Forms.ContextMenuStrip;
using ToolStripItem = System.Windows.Forms.ToolStripItem;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
#endregion

namespace ItakuDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        public static SettingsWindow settings;
        public static MainWindow window;
        public WebView2 webBrowser;
        public Timer timer;
        public ProfileInfo profileInfo;

        public NotifyIcon trayIcon;
        public ContextMenuStrip trayContextMenu;
        public ToolStripItem startupMenuItem;
        public ToolStripItem checkMenuItem;
        public ToolStripItem extensionMenuItem;

        const int appVersion = 2;
        const string updateLink = "https://raw.githubusercontent.com/ReDarkTechnology/Itaku-Desktop/main/current.json";
        const string notificationBadgeXPath = "//*[@id=\"mat-badge-content-5\"]";
        const string submissionBadgeXPath = "//*[@id=\"mat-badge-content-1\"]";
        const string messagesBadgeXPath = "//*[@id=\"mat-badge-content-0\"]";

        public string WebView2Path;
        public string ProfilePath;
        public string SettingsPath;
        public bool customProfile;

        public bool goToExternal;
        public bool checkForUpdates = true;
        public bool isAtStartup;
        public bool reloadToCheckNotification = true;
        public bool hideToTray = true;
        public bool isEnhanced;
        public bool queueEnhance;
        private int _reloadInterval = 5;
        public int reloadInterval
        {
            get => _reloadInterval;
            set
            {
                _reloadInterval = value;
                timer?.Change(0, 60000 * value);
            }
        }

        public int notificationCount;
        public int submissionCount;
        public int messagesCount;

        public bool isCoreInitialized;
        private bool dontClose = true;
        #endregion

        #region Initialization
        public MainWindow()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            window = this;
            string browserPath = null;
            settings = new SettingsWindow();

            WebView2Path = "WebView2".FixPath();
            ProfilePath = "ProfileData".FixPath();
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Itaku", "settings.json");
            var settingsDir = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(settingsDir)) Directory.CreateDirectory(settingsDir);
            Console.WriteLine(SettingsPath);
            LoadSettings();
            LoadArgs();

            if (Directory.Exists(WebView2Path))
                browserPath = WebView2Path;
            else if(!WebViewIsInstalled())
            {
                MessageBox.Show("You haven't installed any WebView2 Runtime - maybe you don't have Microsoft Edge, please download and install it first!", "Itaku: Error");
                return;
            }

            var envOptions = new CoreWebView2EnvironmentOptions() { AreBrowserExtensionsEnabled = true };
            var envTask = CoreWebView2Environment.CreateAsync(browserPath, ProfilePath);
            envTask.Wait();
            if(!customProfile)
            {
                var profileJson = Path.Combine(ProfilePath, "profile.json");
                if (!File.Exists(profileJson))
                {
                    profileInfo = new ProfileInfo() { name = "_Default_INTERNAL", path = profileJson };
                    profileInfo.Save(profileJson);
                }
            }
            var webViewEnvironment = envTask.Result;

            InitializeComponent();

            webBrowser = new WebView2();
            webBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webBrowser.EnsureCoreWebView2Async(webViewEnvironment);

            timer = new Timer((e) => OnTimerElapsed(), null, 0, 60000 * reloadInterval);
            mainGrid.Children.Insert(1, webBrowser);
            webBrowser.VerticalAlignment = VerticalAlignment.Stretch;
            webBrowser.HorizontalAlignment = HorizontalAlignment.Stretch;
            webBrowser.KeyDown += MainGrid_KeyDown;
            webBrowser.Margin = new Thickness(0, 30, 0, 0);
            KeyDown += MainGrid_KeyDown;
            DockPanel.SetDock(webBrowser, Dock.Top);

            InitializeNotifyIcon();
            if (checkForUpdates)
                CheckUpdate();
        }

        public void LoadArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if(Directory.Exists(arg))
                {
                    var profilePath = Path.Combine(arg, "profile.json");
                    if (File.Exists(profilePath))
                    {
                        ProfilePath = arg;
                        customProfile = true;
                    }
                }
            }
        }

        private async void WebBrowser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webBrowser.CoreWebView2.Navigate("https://itaku.ee/");

            webBrowser.SourceChanged += WebBrowser_SourceChanged;
            webBrowser.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            webBrowser.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            webBrowser.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            webBrowser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            webBrowser.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            webBrowser.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;

            isCoreInitialized = true;
            if (queueEnhance)
                await LoadEnhancementExtension();
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if(goToExternal)
            {
                Process.Start(e.Uri);
                e.Handled = true;
            }
        }

        private void WebBrowser_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            UrlTextBox.Text = webBrowser.Source.AbsoluteUri;
            ChangeTitle(webBrowser.CoreWebView2.DocumentTitle);
            CheckNotifications();
        }

        public void InitializeNotifyIcon()
        {
            trayContextMenu = new ContextMenuStrip();
            startupMenuItem = trayContextMenu.Items.Add("Start at startup");
            startupMenuItem.Click += (s, a) => SetStartupToggle();

            checkMenuItem = trayContextMenu.Items.Add("Disable background reload");
            checkMenuItem.Click += (s, a) => SetReloadToggle();

            extensionMenuItem = trayContextMenu.Items.Add("Enable enhancement extension");
            extensionMenuItem.Click += async (s, a) =>
            {
                await LoadEnhancementExtension();
            };

            trayContextMenu.Items.Add("-");

            var openContext = trayContextMenu.Items.Add("Open");
            openContext.Click += (s, a) => Show();
            var exitContext = trayContextMenu.Items.Add("Exit");
            exitContext.Click += (s, a) =>
            {
                trayIcon.Dispose();
                dontClose = false;
                Close();
            };

            trayIcon = new NotifyIcon();
            trayIcon.Icon = ItakuResources.favicon_yellow;
            trayIcon.Text = "Itaku";
            trayIcon.ContextMenuStrip = trayContextMenu;
            trayIcon.Click += (s, a) => Show();
            trayIcon.Visible = true;

            CheckStartup();
        }
        #endregion
        
        #region Update
        public void CheckUpdate()
        {
            using (WebClient client = new WebClient())
            {
                client.DownloadStringCompleted += Client_DownloadStringCompleted;
                client.DownloadStringAsync(new Uri(updateLink));
            }
        }

        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    File.WriteAllText("UpdateLogError.log", e.Error.Message + e.Error.StackTrace);
                }

                if (!e.Cancelled)
                {
                    var data = JsonConvert.DeserializeObject<UpdateInfo>(e.Result);
                    if(data.version > appVersion)
                    {
                        if(MessageBox.Show("There's a new update found! Do you want to download it?", "Itaku: Update", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            Process.Start(data.url);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                File.WriteAllText("UpdateLogError.log", err.Message + err.StackTrace + (err.InnerException != null ? err.InnerException.Message + err.InnerException.StackTrace : null));
            }
        }

        public class UpdateInfo
        {
            public int version;
            public string url;
        }
        #endregion

        #region Events
        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if(!e.Uri.Contains("://itaku.ee"))
            {
                if(MessageBox.Show($"This webpage leads to {e.Uri}, are you sure you want to go there?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    Process.Start(e.Uri);
                e.Cancel = true;
            }
        }

        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            UrlTextBox.Text = webBrowser.Source.AbsoluteUri;
            ChangeTitle(webBrowser.CoreWebView2.DocumentTitle);
            CheckNotifications();
        }

        private void CoreWebView2_DOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            ChangeTitle(webBrowser.CoreWebView2.DocumentTitle);
        }

        private void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dontClose && hideToTray)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public void OnTimerElapsed()
        {
            if (reloadToCheckNotification)
            {
                if (!reloadToCheckNotification) return;
                Application.Current.Dispatcher.Invoke(() => { 
                    if(isCoreInitialized) webBrowser.Reload();
                });
                Console.WriteLine("INFO: Reload browser");
            }
        }
        
        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                webBrowser.Source = new Uri(UrlTextBox.Text);
            
            if (e.Key == Key.Escape)
                UrlTextBox.Text = webBrowser.Source.AbsoluteUri;
        }

        private void MainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F4)
            {
                SetToolbar(!toolbarShown);
            }
        }

        public bool toolbarShown = true;
        public void SetToolbar(bool to)
        {
            toolbarShown = to;
            toolbarGrid.Visibility = to ? Visibility.Visible : Visibility.Hidden;
            webBrowser.Margin = to ? new Thickness(0, 30, 0, 0) : new Thickness(0, 0, 0, 0);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => webBrowser.GoBack();
        private void ForwardButton_Click(object sender, RoutedEventArgs e) => webBrowser.GoForward();
        private void ReloadButton_Click(object sender, RoutedEventArgs e) => webBrowser.Reload();
        #endregion

        #region Browser
        private bool WebViewIsInstalled()
        {
            string regKey = @"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients";
            using (RegistryKey edgeKey = Registry.LocalMachine.OpenSubKey(regKey))
            {
                if (edgeKey != null)
                {
                    string[] productKeys = edgeKey.GetSubKeyNames();
                    if (productKeys.Any())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public async Task<string> GetHtmlFromBrowser()
        {
            var task = webBrowser.CoreWebView2.ExecuteScriptAsync("document.body.outerHTML");
            var html = await task;
            try
            {
                return JsonConvert.DeserializeObject(html).ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return null;
            }
        }
        #endregion

        #region Notification
        public async void CheckNotifications()
        {
            Console.WriteLine("INFO: Checking notification");
            var html = await GetHtmlFromBrowser();
            var htmlDoc = new HtmlDocument();
            try
            {
                htmlDoc.LoadHtml(html);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e.Message}");
            }

            List<string> messages = new List<string>();
            #region Notification
            var badgeNode = htmlDoc.DocumentNode.SelectSingleNode(notificationBadgeXPath);
            if (badgeNode != null)
            {
                int currentCount = 0;
                if (int.TryParse(badgeNode.InnerText, out currentCount))
                {
                    if (currentCount > notificationCount)
                        messages.Add($"New notifications! - {currentCount}");
                    notificationCount = currentCount;
                }
                else
                {
                    Console.WriteLine($"ERROR: Unable to parse {badgeNode.InnerText} to System.Int32");
                }
            }
            else
            {
                Console.WriteLine($"WARNING: The notification badge element isn't found? Try to sign in?");
            }
            #endregion
            #region Submissions
            var subNode = htmlDoc.DocumentNode.SelectSingleNode(submissionBadgeXPath);
            if (subNode != null)
            {
                int currentCount = 0;
                if (int.TryParse(subNode.InnerText, out currentCount))
                {
                    if (currentCount > submissionCount)
                        messages.Add($"New submissions! - {currentCount}");
                    submissionCount = currentCount;
                }
                else
                {
                    Console.WriteLine($"ERROR: Unable to parse {subNode.InnerText} to System.Int32");
                }
            }
            else
            {
                Console.WriteLine($"WARNING: The submissions badge element isn't found?");
            }
            #endregion
            #region Messages
            var msgNode = htmlDoc.DocumentNode.SelectSingleNode(messagesBadgeXPath);
            if (msgNode != null)
            {
                int currentCount = 0;
                if (int.TryParse(msgNode.InnerText, out currentCount))
                {
                    if (currentCount > messagesCount)
                        messages.Add($"New messages! - {currentCount}");
                    messagesCount = currentCount;
                }
                else
                {
                    Console.WriteLine($"ERROR: Unable to parse {msgNode.InnerText} to System.Int32");
                }
            }
            else
            {
                Console.WriteLine($"WARNING: The messages badge element isn't found?");
            }
            #endregion

            if (messages.Count > 0)
            {
                trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                trayIcon.BalloonTipTitle = "Itaku - Notification";
                trayIcon.BalloonTipText = string.Join(", ", messages);
                trayIcon.ShowBalloonTip(3000);
                Console.WriteLine("INFO: " + trayIcon.BalloonTipText);
            }

            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("body");
            if (bodyNode != null)
            {
                var value = bodyNode.GetAttributeValue("class", null);
                Console.WriteLine($"INFO: Get class of body {value}");
                var color = ChangeTheme(value);
                Console.WriteLine($"INFO: Color fetched {color}");
                toolbarGrid.Background = new SolidColorBrush(color);
                ChangeIconTheme(value);
            }
            else
            {
                Console.WriteLine($"WARNING: Body element isn't found.. somehow?");
            }
        }

        public void ChangeIconTheme(string themeString)
        {
            switch (themeString)
            {
                case "sky-pink-light-theme":
                    Icon = GetBitmap("Resources/favicon-light.ico");
                    trayIcon.Icon = ItakuResources.favicon_light;
                    break;
                case "sky-pink-navy-theme":
                    Icon = GetBitmap("Resources/favicon-blue.ico");
                    trayIcon.Icon = ItakuResources.favicon_blue;
                    break;
                case "rain-dusk-theme":
                    Icon = GetBitmap("Resources/favicon-dusk.ico");
                    trayIcon.Icon = ItakuResources.favicon_dusk;
                    break;
                default:
                    Icon = GetBitmap("Resources/favicon-yellow.ico");
                    trayIcon.Icon = ItakuResources.favicon_yellow;
                    break;
            }
        }

        public BitmapFrame GetBitmap(string name)
        {
            return BitmapFrame.Create(Application.GetResourceStream(new Uri(name, UriKind.RelativeOrAbsolute)).Stream);
        }

        public Color ChangeTheme(string themeString)
        {
            switch (themeString)
            {
                case "sky-pink-light-theme":
                case "sky-pink-navy-theme":
                    return GetColor("#95ddff");
                case "rain-dusk-theme":
                    return GetColor("#607d8b");
                case "indigo-pink-light-theme":
                    return GetColor("#3f51b5");
                case "brown-wine-light-theme":
                    return GetColor("#3e2723");
                default:
                    return GetColor("#ffeb3b");
            }
        }

        public Color GetColor(string hex) => (Color)ColorConverter.ConvertFromString(hex);
        #endregion
        
        #region Settings
        public void LoadSettings()
        {
            if(File.Exists(SettingsPath))
            {
                var data = JsonConvert.DeserializeObject<SettingsData>(File.ReadAllText(SettingsPath));
                hideToTray = data.hiddenInTray;
                reloadToCheckNotification = data.autoReload;
                queueEnhance = data.withEnchantment;
                reloadInterval = data.reloadInterval;
                checkForUpdates = data.checkForUpdates;
                goToExternal = data.goToExternal;
            }
        }

        public void SaveSettings()
        {
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(new SettingsData() {
                hiddenInTray = hideToTray,
                autoReload = reloadToCheckNotification,
                withEnchantment = isEnhanced,
                reloadInterval = reloadInterval,
                checkForUpdates = checkForUpdates,
                goToExternal = goToExternal
            }, Formatting.Indented));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (settings == null) settings = new SettingsWindow();
            settings.UpdateValues();
            settings.Show();
        }

        public class SettingsData
        {
            public bool checkForUpdates = true;
            public bool goToExternal;
            public bool hiddenInTray;
            public bool autoReload;
            public bool withEnchantment;
            public int reloadInterval;
        }
        #endregion

        #region Extensions
        public CoreWebView2BrowserExtension enhancementExt;
        public async Task<bool> LoadEnhancementExtension()
        {
            if(!isCoreInitialized)
            {
                MessageBox.Show("Browser isn't initialized yet!", "Itaku: Error");
                return false;
            }

            if (!Directory.Exists("Extensions/itaku-enhancement-suite".FixPath()))
            {
                MessageBox.Show("Extension isn't found!", "Itaku: Error");
                return false;
            }

            Console.WriteLine("INFO: WebBrowser Version = " + webBrowser.CoreWebView2.Environment.BrowserVersionString);
            var version = GetBrowserVersions();
            if (version[0] >= 117)
            {
                if (isEnhanced)
                {
                    try
                    {
                        await enhancementExt.RemoveAsync();
                        isEnhanced = false;
                        return false;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error occurred when removing extension: " + e.Message, "Itaku: Error");
                        return true;
                    }
                }
                else
                {
                    try
                    {
                        enhancementExt = await webBrowser.CoreWebView2.Profile.AddBrowserExtensionAsync("Extensions/itaku-enhancement-suite".FixPath());
                        await enhancementExt.EnableAsync(true);
                        isEnhanced = true;
                        return true;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error occurred when enabling extension: " + e.Message, "Itaku: Error");
                        return false;
                    }
                }
            }
            else
            {
                MessageBox.Show($"Your edge version doesn't support WebView2 extensions yet: {webBrowser.CoreWebView2.Environment.BrowserVersionString}", "Itaku: Warning");
                return false;
            }
        }

        public int[] GetBrowserVersions()
        {
            string[] split = webBrowser.CoreWebView2.Environment.BrowserVersionString.Split('.');
            int[] versions = new int[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                if (int.TryParse(split[i], out int version))
                    versions[i] = version;
            }
            return versions;
        }
        #endregion

        #region Utility
        public void CheckStartup()
        {
            isAtStartup = WindowsUtil.IsApplicationOnStartup();
            startupMenuItem.Text = isAtStartup ? "Delete from startup" : "Start at startup";
        }

        public void SetStartupToggle()
        {
            SetStartup(!isAtStartup);
        }

        public void SetReloadToggle()
        {
            SetReload(!reloadToCheckNotification);
        }

        public void SetStartup(bool to)
        {
            isAtStartup = to;
            startupMenuItem.Text = to ? "Delete from startup" : "Start at startup";
            WindowsUtil.SetApplicationStartup(to);
        }

        public void SetReload(bool to)
        {
            checkMenuItem.Text = to ? "Disable background reload" : "Enable background reload";
            reloadToCheckNotification = to;
        }

        public void ChangeTitle(string text)
        {
            Title = text == "Itaku" ? "Itaku" : "Itaku: " + text;
        }

        /// <summary>Returns true if the current application has focus, false otherwise</summary>
        public static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;       // No window is currently activated
            }

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        #endregion
    }
}
