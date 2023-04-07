using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class AddRoleDialogViewModel : BindableBase, IDialogAware
    {
        public string Title => "添加角色信息";

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
                var model = new RoleEntity
                {
                    RoleName = RoleName,
                    State = 1
                };

                await _rolesBLL.Save(model);

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private readonly IRolesBLL _rolesBLL;

        public AddRoleDialogViewModel(IRolesBLL rolesBLL)
        {
            _rolesBLL = rolesBLL;
        }
    }
}
