using EcoLauncher.Properties;
using EcoLauncher.ViewModels;
using mshtml;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace EcoLauncher.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _lastUrl;

        private MainViewModel ViewModel => (MainViewModel)DataContext;

        public MainWindow()
        {
            InitializeComponent();

            Width = Settings.Default.Width;
            Height = Settings.Default.Height;

            DataContext = new MainViewModel();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Settings.Default.Width = Width;
            Settings.Default.Height = Height;
            Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.Navigate("https://member.gungho.jp/front/member/webgs/eccenter_old.aspx");
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
            }

            _lastUrl = e.Uri.LocalPath;
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Settings.Default.UserName)
                || _lastUrl == "/front/guest/login.aspx")
            {
                AccountSettingsMenuItem_Click(null, null);
                if (string.IsNullOrWhiteSpace(Settings.Default.UserName)) Close();
            }

            var doc = (HTMLDocument)Browser.Document;
            doc.getElementById("MainContent_loginNameControl_txtLoginName").setAttribute("value", Settings.Default.UserName);
            doc.getElementById("MainContent_passwordControl_txtPassword").setAttribute("value", Settings.Default.Password);
            doc.getElementById("MainContent_btNext1").click();
        }
    }
}
