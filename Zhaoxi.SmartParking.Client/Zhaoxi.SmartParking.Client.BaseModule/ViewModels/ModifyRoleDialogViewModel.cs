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
    public class ModifyRoleDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "添加角色信息";

        public ObservableCollection<RoleModel> Roles { get; set; } = new ObservableCollection<RoleModel>();


        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        private int _userId;

        private List<int> _roleIdList;

        public void OnDialogOpened(IDialogParameters parameters)
        {
            _roleIdList = parameters.GetValue<List<int>>("roleIdList");

            _userId = parameters.GetValue<int>("userId");

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


        private async void Confirm()
        {
            try
            {
                var roleIdList = Roles.Where(x => x.IsSelected).Select(x => new RoleEntity { RoleId = x.RoleId }).ToList();

                var model = new SysUserEntity
                {
                    Id = _userId,

                    Roles = roleIdList
                };

                await _sysUserBLL.SaveRole(model);

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Refresh()
        {
            var roles = await _rolesBLL.GetAll();

            Application.Current.Dispatcher.Invoke(() =>
            {
                var temp = roles.Select(x => new RoleModel { RoleId = x.RoleId, RoleName = x.RoleName, IsSelected = _roleIdList.Contains(x.RoleId) }).ToList();

                Roles.AddRange(temp);
            });
        }

        private readonly IRolesBLL _rolesBLL;

        private readonly ISysUserBLL _sysUserBLL;


        public ModifyRoleDialogViewModel(IRolesBLL rolesBLL, ISysUserBLL sysUserBLL)
        {
            _rolesBLL = rolesBLL;

            _sysUserBLL = sysUserBLL;
        }
    }
}