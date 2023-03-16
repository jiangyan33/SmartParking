using Prism.Mvvm;
using System;

namespace Zhaoxi.SmartParking.Client.BaseModule.Models
{
    public class FileInfoModel : BindableBase
    {
        public int Index { get; set; }

        public string FileName { get; set; }

        public string UpdatePath { get; set; }

        public string FullPath { get; set; }

        public DateTime UploadTime { get; set; }

        private string _state;

        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }
    }
}
