using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class AddFileDialogViewModel : IDialogAware
    {
        public string Title => "系统更新文件上传";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        public List<string> UpdatePathList { get; set; } = new List<string>();

        public ObservableCollection<Models.FileInfoModel> FileList { get; set; } = new ObservableCollection<Models.FileInfoModel>();

        private readonly Dispatcher _dispatcher;

        private readonly IFilesBLL _filesBLL;

        private readonly SignalTaskHelper _signalTaskHelper = new SignalTaskHelper();

        public AddFileDialogViewModel(IFilesBLL filesBLL)
        {
            _dispatcher = Application.Current.Dispatcher;

            _filesBLL = filesBLL;

            UpdatePathList.Add("");

            UpdatePathList.Add("Modules");
        }

        public ICommand SelectFileCommand => new DelegateCommand(SelectFile);

        private void SelectFile()
        {
            var dialog = new OpenFileDialog();

            dialog.Multiselect = true;

            if (dialog.ShowDialog() == true)
            {

                var tmpList = new List<Models.FileInfoModel>();

                for (var i = 0; i < dialog.FileNames.Length; i++)
                {
                    tmpList.Add(new Models.FileInfoModel
                    {
                        Index = i + 1,
                        FileName = new FileInfo(dialog.FileNames[i]).Name,
                        FullPath = dialog.FileNames[i],
                        UpdatePath = "",
                        State = "待上传"
                    });
                }

                _dispatcher.Invoke(() =>
                {
                    if (FileList.Count > 0)
                    {
                        FileList.Clear();
                    }

                    FileList.AddRange(tmpList);
                });

            }
        }

        public ICommand UploadCommand => new DelegateCommand(Upload);

        /// <summary>
        /// 上传过程的进度条问题
        /// </summary>
        private void Upload()
        {
            Task.Run(() =>
            {
                foreach (var item in FileList)
                {
                    _signalTaskHelper.Process(() =>
                    _filesBLL.UploadFile(item.FullPath, item.UpdatePath,
                        progress =>
                        {
                            // 进度值的变化
                            item.Progress = progress;
                        },
                        () =>
                        {
                            _signalTaskHelper.Set();

                            item.Progress = 100;

                            item.State = "✔";
                        }),
                    10);
                }
            });
        }
    }
}
