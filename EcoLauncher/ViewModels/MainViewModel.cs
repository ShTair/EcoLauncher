using System.ComponentModel;

namespace EcoLauncher.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Status
        {
            get { return _Status; }
            set
            {
                if (_Status == value) return;
                _Status = value;
                PropertyChanged?.Invoke(this, _StatusChangedEventArgs);
            }
        }
        private string _Status;
        private PropertyChangedEventArgs _StatusChangedEventArgs = new PropertyChangedEventArgs(nameof(Status));

        public MainViewModel()
        {
            Status = "準備完了";
        }
    }
}
