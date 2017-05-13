using System.Windows;

namespace EcoLauncher.Views
{
    /// <summary>
    /// AccountSettingsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AccountSettingsWindow : Window
    {
        public AccountSettingsWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
