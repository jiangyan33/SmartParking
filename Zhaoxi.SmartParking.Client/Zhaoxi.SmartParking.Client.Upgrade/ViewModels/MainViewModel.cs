using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Zhaoxi.SmartParking.Client.Upgrade.Models;

namespace Zhaoxi.SmartParking.Client.Upgrade.ViewModels
{
    public class MainViewModel : BindableBase
    {
        public ObservableCollection<FileModel> FileModelList { get; set; } = new ObservableCollection<FileModel>();

        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        private double _totalLen = 0.0;

        private int _value = 0;

        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        private string _state = "待更新";

        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private int _index = 0;

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        public MainViewModel(List<FileModel> fileModels)
        {
            foreach (var item in fileModels)
            {
                FileModelList.Add(item);

                _totalLen += item.FileLen;
            }

            StartCommand = new DelegateCommand(OnStart);
        }

        #region 命令

        public ICommand StartCommand { get; set; }

        private void OnStart()
        {
            if (FileModelList.Count == 0) return;

            State = "正在更新";

            Index = 1;

            Task.Run(() =>
            {
                foreach (var fileModel in FileModelList)
                {
                    fileModel.State = "正在更新";

                    var webDataAccess = new DataAccess.WebDataAccess();

                    webDataAccess.DownloadCompleted = new System.Action(() =>
                    {
                        // 更新本地数据库信息
                        var flag = DataAccess.LocalDataAccess.UpdateFile(fileModel.FileName, fileModel.MD5, fileModel.FileLen.ToString());

                        var brush = new SolidColorBrush(Colors.Green);

                        if (!flag)
                        {
                            brush = new SolidColorBrush(Colors.Red);

                            fileModel.State = "✖";
                        }
                        else
                        {
                            fileModel.State = "✔";
                        }

                        Index++;

                        _value += Convert.ToInt32(fileModel.FileLen / _totalLen * 100);

                        _autoResetEvent.Set();
                    });

                    webDataAccess.DownloadPrograssChanged = new System.Action<int>(progress =>
                    {
                        // 单个文件的下载速度变更
                        var value = Convert.ToInt32(progress * (fileModel.FileLen / _totalLen));

                        if (value + _value > 100)
                        {
                            Progress = 100;
                        }
                        else
                        {
                            Progress = _value + value;
                        }
                    });

                    webDataAccess.DownloadAsync(fileModel);

                    _autoResetEvent.WaitOne();
                }

                State = "更新完成";

                // 全部下载完成，提示更新成功，是否运行主程序

                var res = MessageBox.Show("更新操作已经完成，是否运行主程序？", "提示", MessageBoxButton.YesNo);

                if (res == MessageBoxResult.Yes)
                {
                    var process = Process.Start("Zhaoxi.SmartParking.Client.exe");

                    process.WaitForInputIdle();
                }

                Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
            });
        }

        #endregion
    }
}
