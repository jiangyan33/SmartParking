using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.SmartParking.Client.BaseModule.Models;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class SelectUserDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "选择用户信息";

        private List<int> _userIdList = new List<int>();

        public ObservableCollection<UserInfoModel> Users { get; set; } = new ObservableCollection<UserInfoModel>();


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
            _userIdList = parameters.GetValue<List<int>>("userIdList");

            Refresh();
        }

        private string _roleName;

        public string RoleName
        {
            get => _roleName;
            set { SetProperty(ref _roleName, value); }
        }

        public ICommand CancelCommand => new DelegateCommand(Cancel);


        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public ICommand ConfirmCommand => new DelegateCommand(Confirm);


        private void Confirm()
        {
            try
            {
                var parameters = new DialogParameters();

                parameters.Add("users", Users.Where(x => x.IsSelected).ToList());

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Refresh()
        {
            var userList = await _sysUserBLL.All();

            var list = userList.Where(x => !_userIdList.Contains(x.Id)).Select(x => new UserInfoModel
            {
                UserId = x.Id,
                UserName = x.UserName,
                RealName = x.RealName
            }).ToList();

            Application.Current.Dispatcher.Invoke(() => Users.AddRange(list));
        }

        private readonly ISysUserBLL _sysUserBLL;

        public SelectUserDialogViewModel(ISysUserBLL sysUserBLL)
        {
            _sysUserBLL = sysUserBLL;
        }
    }
}
