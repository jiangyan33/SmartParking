using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.BaseModule.Models;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class AddUserDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        private UserInfoModel _mainModel;

        public UserInfoModel MainModel
        {
            get { return _mainModel; }
            set { SetProperty(ref _mainModel, value); }
        }

        private bool _isReadOnlyUserName;

        public bool IsReadOnlyUserName
        {
            get { return _isReadOnlyUserName; }
            set { SetProperty(ref _isReadOnlyUserName, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            MainModel = parameters.GetValue<UserInfoModel>("model");

            if (MainModel.UserId == 0)
            {
                Title = "添加新用户";
            }
            else
            {
                Title = $"编辑用户[{MainModel.UserName}]";

                IsReadOnlyUserName = true;
            }
        }

        public List<string> UpdatePathList { get; set; } = new List<string>();

        public ObservableCollection<Models.FileInfoModel> FileList { get; set; } = new ObservableCollection<Models.FileInfoModel>();

        private readonly Dispatcher _dispatcher;

        private readonly ISysUserBLL _sysUserBLL;

        private readonly IFilesBLL _filesBLL;

        private readonly SignalTaskHelper _signalTaskHelper = new SignalTaskHelper();

        public AddUserDialogViewModel(ISysUserBLL sysUserBLL, IFilesBLL filesBLL)
        {
            _dispatcher = Application.Current.Dispatcher;

            _sysUserBLL = sysUserBLL;

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

        public ICommand CancelCommand => new DelegateCommand(Cancel);


        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public ICommand ConfirmCommand => new DelegateCommand(Confirm);


        private async void Confirm()
        {
            try
            {
                var sysUserEntity = new SysUserEntity
                {
                    UserName = MainModel.UserName,
                    RealName = MainModel.RealName,
                    UserAge = MainModel.Age,
                    UserIcon = MainModel.UserIcon,
                    Password = MainModel.Password,
                    Id = MainModel.UserId,
                    UserState = 1
                };

                await _sysUserBLL.Save(sysUserEntity);

                // 上传头像信息

                _filesBLL.UploadIcon(sysUserEntity.UserName, sysUserEntity.UserIcon);

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SelectIconCommand => new DelegateCommand(SelectIcon);


        private void SelectIcon()
        {
            var dialog = new OpenFileDialog();

            dialog.Multiselect = false;

            //dialog.Filter = "*.*|";

            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;

                MainModel.UserIcon = path;
            }
        }
    }
}
