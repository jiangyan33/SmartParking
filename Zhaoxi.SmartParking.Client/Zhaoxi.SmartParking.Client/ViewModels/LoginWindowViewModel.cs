using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.SmartParking.Client.Common;
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

        public LoginWindowViewModel(ISysUserBLL sysUserBLL, IFilesBLL filesBLL, IEventAggregator eventAggregator)
        {
            _sysUserBLL = sysUserBLL;

            SysUserModel = new SysUserModel();

            LoginCommand = new DelegateCommand<Window>(Login);

            // 检查更新
            IsLoading = true;
            Task.Run(async () =>
            {
                try
                {
                    var files = new List<string>();

                    var ret = await filesBLL.List();

                    // 当服务器列表中文件存在，且md5和本地的不一样，执行更新下载操作
                    var localRet = await filesBLL.LocalList();

                    foreach (var serverFile in ret)
                    {
                        var model = localRet.Find(x => x.FileName == serverFile.FileName && x.FileMD5 == serverFile.FileMD5);

                        if (model == null)
                        {
                            //文件名称、文件大小、文件路径、文件MD5
                            files.Add(serverFile.FileName + "|" + serverFile.Length + "|" + serverFile.FilePath + "|" + serverFile.UpdatePath + "|" + serverFile.FileMD5);
                        }
                    }

                    if (files.Count > 0)
                    {
                        // 执行更新操作
                        var updateListJson = string.Join(";", files);

                        var process = Process.Start("Zhaoxi.SmartParking.Client.Upgrade.exe", updateListJson);

                        process.WaitForInputIdle();

                        // 操作关闭窗口,这里没有触发任何命令，是否事件总线的通知功能，通知到UI,然后关闭窗口
                        eventAggregator.GetEvent<CloseWindowEvent>().Publish();
                    }
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

        public void Login(Window window)
        {
            Task.Run(async () =>
            {
                try
                {
                    IsLoading = true;

                    if (string.IsNullOrEmpty(SysUserModel.UserName)) throw new System.Exception("请输入用户名");

                    if (string.IsNullOrEmpty(SysUserModel.Password)) throw new System.Exception("请输入密码");

                    //await Task.Delay(5000);

                    var model = await _sysUserBLL.Login(SysUserModel.UserName, SysUserModel.Password);

                    GlobalValue.UserInfo = model;

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
