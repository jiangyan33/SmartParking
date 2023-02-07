using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.Models;

namespace Zhaoxi.SmartParking.Client.ViewModels
{
    public class LoginWindowViewModel : BindableBase
    {
        public SysUserModel SysUserModel { get; set; }

        public ICommand LoginCommand { get; set; }

        public readonly ISysUserBLL _sysUserBLL;

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public LoginWindowViewModel(ISysUserBLL sysUserBLL, IFilesBLL filesBLL)
        {
            _sysUserBLL = sysUserBLL;

            SysUserModel = new SysUserModel();

            LoginCommand = new DelegateCommand<Window>(Login);

            // 检查更新
            IsLoading = true;
            Task.Run(async () =>
            {
                await Task.Delay(5000);

                var ret = await filesBLL.List();

                var localRet = await filesBLL.LocalList();

                IsLoading = false;
            });
        }

        public void Login(Window window)
        {
            Task.Run(async () =>
            {
                try
                {
                    IsLoading = true;

                    if (string.IsNullOrEmpty(SysUserModel.UserName)) throw new System.Exception("请输入用户名");

                    if (string.IsNullOrEmpty(SysUserModel.Password)) throw new System.Exception("请输入密码");

                    await Task.Delay(5000);

                    var model = await _sysUserBLL.Login(SysUserModel.UserName, SysUserModel.Password);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        window.DialogResult = true;

                        window.Close();
                    });

                    return;
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message ?? "";
                }
                finally
                {
                    IsLoading = false;
                }
            });
        }
    }
}
