using Prism.Mvvm;
using System.Windows;
using System.Windows.Media;

namespace Zhaoxi.SmartParking.Client.Upgrade.Models
{
    public class FileModel : BindableBase
    {

        public int Index { get; set; }

        public string FileName { get; set; }

        public string MD5 { get; set; }

        public string FileUrl { get; set; }

        public string UpdatePath { get; set; }

        public int FileLen { get; set; }

        private string _state;

        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        //private SolidColorBrush _errorMsg = new SolidColorBrush(Color.FromRgb(85, 85, 85));

        //public SolidColorBrush ErrorMsg
        //{
        //    get { return _errorMsg; }
        //    set { Application.Current.Dispatcher.Invoke(() => SetProperty(ref _errorMsg, value)); }
        //}

        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
    }
}
