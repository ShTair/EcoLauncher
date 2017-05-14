using EcoLauncher.Properties;
using EcoLauncher.ViewModels;
using mshtml;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EcoLauncher.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime _lastLogin;

        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            BrowserUtils.SetFeatureBrowserEmulation(11001);
            BrowserUtils.SetFeatureGpuRendering(true);

            InitializeComponent();

            Width = Settings.Default.Width;
            Height = Settings.Default.Height;

            DataContext = new MainViewModel();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                Settings.Default.Width = Width;
                Settings.Default.Height = Height;
            }
            Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.Navigate("https://member.gungho.jp/front/member/webgs/eccenter_old.aspx");
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AccountSettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var w = new AccountSettingsWindow { Owner = this };
            w.UserNameBox.Text = Settings.Default.UserName;
            w.PasswordBox.Text = Settings.Default.Password;

            if (w.ShowDialog() == true)
            {
                Settings.Default.UserName = w.UserNameBox.Text;
                Settings.Default.Password = w.PasswordBox.Text;
                Settings.Default.Save();
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                case WindowState.Maximized:
                    ShowInTaskbar = true;
                    break;
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    break;
            }
        }

        private void NotifyIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
        }

        private void ListMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window_Loaded(null, null);
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            Console.WriteLine(e.Uri.LocalPath);

            switch (e.Uri.LocalPath)
            {
                case "/front/guest/login.aspx":
                    ViewModel.Status = "ログインしています...";
                    Login();
                    break;
                case "/front/safetylock/verify.aspx":
                    ViewModel.Status = "IDロックが設定されています。";
                    break;
                case "/front/guest/imageauth.aspx":
                    ViewModel.Status = "画像認証が必要です。";
                    break;
                case "/front/member/webgs/eccenter_old.aspx":
                    ViewModel.Status = "ログインが完了しました。";
                    ListAccounts();
                    break;
            }
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.UserName)
                || _lastLogin > DateTime.Now.AddMinutes(-1))
            {
                AccountSettingsMenuItem_Click(null, null);
                if (string.IsNullOrWhiteSpace(Settings.Default.UserName)) Close();
            }

            _lastLogin = DateTime.Now;

            var doc = (HTMLDocument)Browser.Document;
            doc.getElementById("MainContent_loginNameControl_txtLoginName").setAttribute("value", Settings.Default.UserName);
            doc.getElementById("MainContent_passwordControl_txtPassword").setAttribute("value", Settings.Default.Password);
            doc.getElementById("MainContent_btNext1").click();
        }

        private void ListAccounts()
        {
            var doc = (HTMLDocument)Browser.Document;
            var rootSpan = (HTMLSpanElement)doc.getElementById("labViewAttractionID");
            var tbody = rootSpan.getElementsByTagName("tbody").Cast<HTMLTableSection>().First();

            var accounts = ((IEnumerable)tbody.children).OfType<HTMLTableRow>().Skip(1).Select(t => new AccountViewModel(t)).ToList();
            accounts.Sort((a, b) => a.Name.CompareTo(b.Name));

            var items = NotifyIconMenu.Items;
            MenuItem item;
            items.Clear();
            foreach (var account in accounts)
            {
                item = new MenuItem { Header = account.Name };
                item.Click += (_, __) => StartAccount(account);
                items.Add(item);
            }

            items.Add(new Separator());
            item = new MenuItem { Header = "閉じる(_X)" };
            item.Click += ExitMenuItem_Click;
            items.Add(item);
        }

        private async void StartAccount(AccountViewModel account)
        {
            bool ismin = WindowState == WindowState.Minimized;
            if (ismin) WindowState = WindowState.Normal;

            account.StartElement.click();

            if (ismin)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                WindowState = WindowState.Minimized;
            }
        }
    }
}
