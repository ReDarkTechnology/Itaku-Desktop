using System;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Newtonsoft.Json;
using Microsoft.Win32;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using ItakuDesktop.Tools;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace ItakuDesktop
{
    public partial class MainForm : Form
    {
        #region Variables
        public static MainForm self;
        public static SettingsForm settings;

        public WebView2 webBrowser;
        public Timer timer;
        public ProfileInfo profileInfo;

        public NotifyIcon trayIcon;
        public ContextMenuStrip trayContextMenu;
        public ToolStripItem startupMenuItem;
        public ToolStripItem checkMenuItem;
        public ToolStripItem extensionMenuItem;

        const int appVersion = 4;
        const string updateLink = "https://raw.githubusercontent.com/ReDarkTechnology/Itaku-Desktop/main/current.json";

        public string WebView2Path;
        public string ProfilePath;
        public string SettingsPath;
        public bool customProfile;

        public bool goToExternal;
        public bool checkForUpdates = true;
        public bool isAtStartup;
        public bool reloadToCheckNotification = true;
        public bool hideToTray = true;
        public bool isEnhanced = true;
        private int _reloadInterval = 5;
        public int reloadInterval
        {
            get => _reloadInterval;
            set
            {
                _reloadInterval = value;
                if(timer != null)
                    timer.Interval = 60000 * value;
            }
        }

        public int notificationCount;
        public int submissionCount;
        public int messagesCount;

        public bool isCoreInitialized;
        private bool dontClose = true;

        public UnreadNotifications previousNotification = new UnreadNotifications();
        #endregion

        #region Form
        public MainForm()
        {
            self = this;
            // InitializeComponent();
            InitializeWebView();
        }
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

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoBack) webBrowser.GoBack();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (webBrowser.CanGoForward) webBrowser.GoForward();
        }

        private void reloadButton_Click(object sender, EventArgs e)
        {
            webBrowser.Reload();
        }

        private void urlBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                webBrowser.Source = new Uri(urlBox.Text);

            if (e.KeyCode == Keys.Escape)
                urlBox.Text = webBrowser.Source.AbsoluteUri;
        }
        #endregion

        #region WebView
        public CoreWebView2Environment PrepareWebViewEnvironment()
        {
            string browserPath = null;

            WebView2Path = "WebView2".FixPath();
            ProfilePath = "ProfileData".FixPath();
            if (!Directory.Exists(ProfilePath)) Directory.CreateDirectory(ProfilePath);
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Itaku", "settings.json");
            var settingsDir = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(settingsDir)) Directory.CreateDirectory(settingsDir);
            Console.WriteLine(SettingsPath);

            LoadSettings();
            LoadArgs();

            if (Directory.Exists(WebView2Path))
                browserPath = WebView2Path;
            else if (!WebViewIsInstalled())
            {
                MessageBox.Show("You haven't installed any WebView2 Runtime - maybe you don't have Microsoft Edge, please download and install it first!", "Itaku: Error");
                return null;
            }

            var envOptions = new CoreWebView2EnvironmentOptions() { };
            var envTask = CoreWebView2Environment.CreateAsync(browserPath, ProfilePath);
            envTask.Wait();
            if (!customProfile)
            {
                var profileJson = Path.Combine(ProfilePath, "profile.json");
                if (!File.Exists(profileJson))
                {
                    profileInfo = new ProfileInfo() { name = "_Default_INTERNAL", path = profileJson };
                    profileInfo.Save(profileJson);
                }
            }
            return envTask.Result;
        }

        public void InitializeWebView()
        {
            var webViewEnvironment = PrepareWebViewEnvironment();
            if (webViewEnvironment == null) return;

            InitializeComponent();

            webBrowser = new WebView2();
            webBrowser.Dock = DockStyle.Fill;
            browserPanel.Controls.Add(webBrowser);
            webBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;
            webBrowser.KeyDown += MainForm_KeyDown;
            webBrowser.KeyUp += MainForm_KeyUp;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webBrowser.EnsureCoreWebView2Async(webViewEnvironment);

            timer = new Timer();
            timer.Interval = 60000 * reloadInterval;
            timer.Tick += Timer_Tick;

            InitializeNotifyIcon();
            CheckNotificationAPI();

            var theme = LoadTheme();
            ChangeTheme(theme);

            if (checkForUpdates)
                CheckUpdate();
        }

        private void WebBrowser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webBrowser.CoreWebView2.Navigate("https://itaku.ee/");

            webBrowser.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            webBrowser.SourceChanged += WebBrowser_SourceChanged;
            webBrowser.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            webBrowser.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            webBrowser.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            webBrowser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;

            webBrowser.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            webBrowser.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;

            isCoreInitialized = true;

            var extFolder = "Extensions".FixPath();
            if (!Directory.Exists(extFolder)) Directory.CreateDirectory(extFolder);
            ExtensionManager.LoadAllInFolder(extFolder);
        }

        private void CoreWebView2_DocumentTitleChanged(object sender, object e)
        {
            ChangeTitle(webBrowser.CoreWebView2.DocumentTitle);
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            if (goToExternal)
            {
                Process.Start(e.Uri);
                e.Handled = true;
            }
        }

        private void WebBrowser_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            urlBox.Text = webBrowser.Source.AbsoluteUri;
            UpdateStyleFromHtml();
            if (isEnhanced)
                ExtensionManager.CallAllScript("OnSourceChanged", webBrowser.Source.AbsoluteUri);
        }

        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (!e.Uri.Contains("://itaku.ee"))
            {
                if (MessageBox.Show($"This webpage leads to {e.Uri}, are you sure you want to go there?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Process.Start(e.Uri);
                e.Cancel = true;
            }
            else
            {
                if (isEnhanced)
                    ExtensionManager.CallAllScript("OnNavigationStarting", e.Uri);
            }
        }

        // Turns out we don't need the cookie to contact the notification API
        private async void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            urlBox.Text = webBrowser.Source.AbsoluteUri;
            UpdateStyleFromHtml();
            if (isEnhanced)
                ExtensionManager.CallAllScript("OnNavigationCompleted", webBrowser.Source.AbsoluteUri);
            // await SaveBrowserCookies();
            await GetTokenFromBrowser();
        }

        private async void CoreWebView2_DOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            if (isEnhanced)
                ExtensionManager.CallAllScript("OnDOMContentLoaded");
            // await SaveBrowserCookies();
            await GetTokenFromBrowser();
        }

        public async Task SaveBrowserCookies()
        {
            var cookies = await webBrowser.CoreWebView2.CookieManager.GetCookiesAsync("https://itaku.ee/");
            var container = new CookieContainer();
            foreach (var cookie in cookies)
            {
                Console.WriteLine($"COOKIE: {cookie.Name}={cookie.Value}");
                container.Add(cookie.ToSystemNetCookie());
            }
            Console.WriteLine($"COOKIE: All cookies fetched from itaku.ee");
            ItakuScraper.SaveCookieContainer(container);
        }

        public async Task GetTokenFromBrowser()
        {
            var result = await webBrowser.ExecuteScriptAsync("localStorage.getItem('token');");
            Console.WriteLine($"TOKEN: {result}");
            if (!string.IsNullOrWhiteSpace(result))
                ItakuScraper.SaveToken(result.Replace("\\", null).Replace("\"", null));
        }

        private void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
        }
        #endregion

        #region Notification
        public async void CheckNotificationAPI()
        {
            Console.WriteLine("INFO: Getting notification client");
            var client = ItakuScraper.GetNotificationClient();
            if (string.IsNullOrWhiteSpace(client.auth_token)) return;

            Console.WriteLine("INFO: Contacting itaku API");
            var response = await client.GetResponseAsync();
            if(response.isContentFetched)
            {
                Console.WriteLine("INFO: Notification response fetched");
                var notification = ItakuScraper.FromStringToNotification(response.content);
                if(notification != null)
                {
                    if(!previousNotification.Equals(notification))
                    {
                        trayIcon.BalloonTipIcon = ToolTipIcon.Info;
                        trayIcon.BalloonTipTitle = "New Itaku Notifications!";
                        trayIcon.BalloonTipText = notification.GetNotificationText();
                        trayIcon.ShowBalloonTip(5000);
                    }
                }
                else
                {
                    Console.WriteLine($"INFO: Unable to parsee {response.content}");
                }
            }
            else
            {
                Console.WriteLine($"INFO: API responds incorrectly [{response.message.StatusCode}]");
            }
        }
        #endregion

        #region Style
        public async void UpdateStyleFromHtml()
        {
            Console.WriteLine("INFO: Checking style");
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

            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("body");
            if (bodyNode != null)
            {
                var value = bodyNode.GetAttributeValue("class", null);
                Console.WriteLine($"INFO: Get class of body {value}");
                ChangeTheme(value);
                SaveTheme(value);
            }
            else
            {
                Console.WriteLine($"WARNING: Body element isn't found.. somehow?");
            }
        }

        public void ChangeTheme(string value)
        {
            var color = GetColorTheme(value);
            var foreground = GetColorAccentTheme(value);
            ChangeTheme(color, foreground);
            ChangeIconTheme(value);
        }

        public void ChangeTheme(Color color, Color foreground)
        {
            topPanel.BackColor = color;

            previousButton.BackColor = color;
            previousButton.ForeColor = foreground;
            previousButton.BorderColor = foreground;
            nextButton.BackColor = color;
            nextButton.ForeColor = foreground;
            nextButton.BorderColor = foreground;
            downloadsButton.BackColor = color;
            downloadsButton.ForeColor = foreground;
            downloadsButton.BorderColor = foreground;
            favoritesButton.BackColor = color;
            favoritesButton.ForeColor = foreground;
            favoritesButton.BorderColor = foreground;
            moreButton.BackColor = color;
            moreButton.ForeColor = foreground;
            moreButton.BorderColor = foreground;
            reloadButton.BackColor = color;
            reloadButton.ForeColor = foreground;
            reloadButton.BorderColor = foreground;
            urlBox.BackColor = color;
            urlBox.ForeColor = foreground;
        }

        public int Limit(int v) => v > 255 ? 255 : v < 0 ? 0 : v;

        public void ChangeIconTheme(string themeString)
        {
            switch (themeString)
            {
                case "sky-pink-light-theme":
                    trayIcon.Icon = ItakuResources.favicon_light;
                    break;
                case "sky-pink-navy-theme":
                    trayIcon.Icon = ItakuResources.favicon_blue;
                    break;
                case "rain-dusk-theme":
                    trayIcon.Icon = ItakuResources.favicon_dusk;
                    break;
                default:
                    trayIcon.Icon = ItakuResources.favicon_yellow;
                    break;
            }
        }

        public Color GetColorAccentTheme(string themeString)
        {
            switch (themeString)
            {
                case "sky-pink-light-theme":
                case "indigo-pink-light-theme":
                case "brown-wine-light-theme":
                    return GetColor("#ffffff");
                default:
                    return GetColor("#000000");
            }
        }

        public Color GetColorTheme(string themeString)
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

        public Color GetColor(string hex) => (Color)new ColorConverter().ConvertFromString(hex);

        public void SaveTheme(string value)
        {
            File.WriteAllText(Path.Combine(ProfilePath, "theme.ini"), value);
        }

        public string LoadTheme()
        {
            var path = Path.Combine(ProfilePath, "theme.ini");
            if (File.Exists(path))
                return File.ReadAllText(path);
            return null;
        }
        #endregion

        #region Notify Icon
        public void InitializeNotifyIcon()
        {
            trayContextMenu = new ContextMenuStrip();
            startupMenuItem = trayContextMenu.Items.Add("Start at startup");
            startupMenuItem.Click += (s, a) => SetStartupToggle();

            checkMenuItem = trayContextMenu.Items.Add("Disable notification service");
            checkMenuItem.Click += (s, a) => SetReloadToggle();

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

        #region Updates
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
                    File.WriteAllText("UpdateLogError.log".FixPath(), e.Error.Message + e.Error.StackTrace);
                }

                if (!e.Cancelled)
                {
                    var data = JsonConvert.DeserializeObject<UpdateInfo>(e.Result);
                    if (data.version > appVersion)
                    {
                        if (MessageBox.Show("There's a new update found! Do you want to download it?", "Itaku: Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Process.Start(data.url);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                File.WriteAllText("UpdateLogError.log".FixPath(), err.Message + err.StackTrace + (err.InnerException != null ? err.InnerException.Message + err.InnerException.StackTrace : null));
            }
        }

        public class UpdateInfo
        {
            public int version;
            public string url;
        }
        #endregion

        #region Extensions
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

        #region Settings
        public void LoadSettings()
        {
            if (File.Exists(SettingsPath))
            {
                var data = JsonConvert.DeserializeObject<SettingsData>(File.ReadAllText(SettingsPath));
                hideToTray = data.hiddenInTray;
                reloadToCheckNotification = data.autoReload;
                isEnhanced = data.withEnchantment;
                reloadInterval = data.reloadInterval;
                checkForUpdates = data.checkForUpdates;
                goToExternal = data.goToExternal;
            }
        }

        public void SaveSettings()
        {
            File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(GetSettingsData(), Formatting.Indented));
        }

        public SettingsData GetSettingsData()
        {
            return new SettingsData()
            {
                hiddenInTray = hideToTray,
                autoReload = reloadToCheckNotification,
                withEnchantment = isEnhanced,
                reloadInterval = reloadInterval,
                checkForUpdates = checkForUpdates,
                goToExternal = goToExternal
            };
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            if (settings == null) settings = new SettingsForm();
            settings.UpdateValues();
            settings.Show();
        }

        public class SettingsData
        {
            public bool checkForUpdates = true;
            public bool goToExternal;
            public bool hiddenInTray = true;
            public bool autoReload = true;
            public bool withEnchantment;
            public int reloadInterval = 5;
        }
        #endregion

        #region Utility
        private void Timer_Tick(object sender, EventArgs e)
        {
            CheckNotificationAPI();
        }
        
        public void SetToolbar(bool to)
        {
            topPanel.Visible = to;
        }

        public void LoadArgs()
        {
            string[] args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if (Directory.Exists(arg))
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
            checkMenuItem.Text = to ? "Disable notification service" : "Enable notification service";
            reloadToCheckNotification = to;
        }

        public void ChangeTitle(string text)
        {
            Text = text == "Itaku" ? "Itaku" : "Itaku: " + text;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dontClose)
            {
                e.Cancel = true;
                Hide();
            }
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

        #region Keys
        bool ctrlPressed;
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4) SetToolbar(!topPanel.Visible);
            if (e.KeyCode == Keys.F5) webBrowser.Reload();
            if (e.KeyCode == Keys.ControlKey) ctrlPressed = true;
            if (e.KeyCode == Keys.L && ctrlPressed)
            {
                urlBox.TypingBox.Select();
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) ctrlPressed = false;
        }
        #endregion

        #region Local Favorites
        private void favoritesButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yet to be implemented", "Itaku: Local Favorites");
        }
        #endregion

        #region Local Saved Posts
        private void downloadsButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yet to be implemented", "Itaku: Local Saved Posts");
        }
        #endregion
    }
}
