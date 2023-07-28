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
#endregion

namespace ItakuDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        public WebView2 webBrowser;
        public Timer timer;

        public NotifyIcon trayIcon;
        public ContextMenuStrip trayContextMenu;
        public ToolStripItem startupMenuItem;
        public ToolStripItem checkMenuItem;

        const string notificationBadgeXPath = "//*[@id=\"mat-badge-content-5\"]";

        public bool isAtStartup;
        public bool reloadToCheckNotification = true;
        public int notificationCount;
        public bool isCoreInitialized;
        private bool dontClose = true;
        #endregion

        #region Initialization
        public MainWindow()
        {
            string browserPath = null;
            if (Directory.Exists("WebView2".FixPath()))
                browserPath = "WebView2".FixPath();
            else if(!WebViewIsInstalled())
            {
                MessageBox.Show("You haven't installed any WebView2 Runtime, please download and install it first!", "Error");
                return;
            }

            var envTask = CoreWebView2Environment.CreateAsync(browserPath, "ProfileData".FixPath());
            envTask.Wait();
            var webViewEnvironment = envTask.Result;

            InitializeComponent();

            webBrowser = new WebView2();
            webBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            webBrowser.EnsureCoreWebView2Async(webViewEnvironment);

            timer = new Timer((e) => OnTimerElapsed(), null, 0, 1000 * 60 * 5);

            mainGrid.Children.Insert(1, webBrowser);
            webBrowser.VerticalAlignment = VerticalAlignment.Stretch;
            webBrowser.HorizontalAlignment = HorizontalAlignment.Stretch;
            webBrowser.KeyDown += MainGrid_KeyDown;
            webBrowser.Margin = new Thickness(0, 30, 0, 0);
            KeyDown += MainGrid_KeyDown;
            DockPanel.SetDock(webBrowser, Dock.Top);

            InitializeNotifyIcon();
        }

        private void WebBrowser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webBrowser.CoreWebView2.Navigate("https://itaku.ee/");

            webBrowser.SourceChanged += WebBrowser_SourceChanged;
            webBrowser.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            webBrowser.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            webBrowser.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;

            webBrowser.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            webBrowser.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;

            isCoreInitialized = true;
        }

        private void WebBrowser_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {
            UrlTextBox.Text = webBrowser.Source.AbsoluteUri;
            CheckNotifications();
        }

        public void InitializeNotifyIcon()
        {
            trayContextMenu = new ContextMenuStrip();
            startupMenuItem = trayContextMenu.Items.Add("Start at startup");
            startupMenuItem.Click += (s, a) => SetStartupToggle();

            checkMenuItem = trayContextMenu.Items.Add("Disable background reload");
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
        #endregion

        #region Events
        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if(!e.Uri.Contains("://itaku.ee/"))
            {
                if(MessageBox.Show($"This webpage leads to {new Uri(e.Uri).Host}, are you sure you want to go there?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
            if (dontClose)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void OnTimerElapsed()
        {
            if (reloadToCheckNotification)
            {
                Application.Current.Dispatcher.Invoke(() => { 
                    if(isCoreInitialized) webBrowser.Reload();
                });
                Console.WriteLine("INFO: Reload browser");
            }
        }
        
        private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                webBrowser.NavigateToString(UrlTextBox.Text);
            
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
            return JsonConvert.DeserializeObject(html).ToString();
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

            var badgeNode = htmlDoc.DocumentNode.SelectSingleNode(notificationBadgeXPath);
            if (badgeNode != null)
            {
                int currentCount = 0;
                if (int.TryParse(badgeNode.InnerText, out currentCount))
                {
                    if (currentCount > notificationCount)
                    {
                        trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
                        trayIcon.BalloonTipTitle = "Itaku - Notification";
                        trayIcon.BalloonTipText = $"New notifications! - {currentCount}";
                        trayIcon.ShowBalloonTip(3000);
                        Console.WriteLine("INFO: " + trayIcon.BalloonTipText);
                    }
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

            var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("body");
            if (bodyNode != null)
            {
                var value = bodyNode.GetAttributeValue("class", null);
                Console.WriteLine($"INFO: Get class of body {value}");
                var color = ChangeTheme(value);
                Console.WriteLine($"INFO: Color fetched {color}");
                toolbarGrid.Background = new SolidColorBrush(color);
            }
            else
            {
                Console.WriteLine($"WARNING: Body element isn't found.. somehow?");
            }
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

        #region Utility
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
