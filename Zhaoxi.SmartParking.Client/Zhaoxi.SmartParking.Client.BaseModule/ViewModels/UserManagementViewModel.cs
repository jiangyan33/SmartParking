using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.BaseModule.Models;
using Zhaoxi.SmartParking.Client.BaseModule.Views;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class UserManagementViewModel : PageViewModelBase
    {

        public ObservableCollection<UserInfoModel> Users { get; set; } = new ObservableCollection<UserInfoModel>();


        private readonly ISysUserBLL _sysUserBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public UserManagementViewModel(ISysUserBLL sysUserBLL, IDialogService dialogService)
        {
            PageTitle = "用户管理";

            AddButtonText = "新增";

            _sysUserBLL = sysUserBLL;

            _dialogService = dialogService;

            _dispatcher = Application.Current.Dispatcher;

            Refresh();
        }

        public ICommand DeleteCommand => new DelegateCommand<object>(Delete);

        private async void Delete(object obj)
        {
            var model = obj as UserInfoModel;

            var ret = MessageBox.Show($"是否要删除该条记录[{model.RealName}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (ret == MessageBoxResult.Cancel) return;

            try
            {
                var sysUserEntity = new SysUserEntity
                {
                    UserName = model.UserName,
                    RealName = model.RealName,
                    UserAge = model.Age,
                    UserIcon = model.UserIcon,
                    Password = model.Password,
                    Id = model.UserId,
                    UserState = 0
                };

                await _sysUserBLL.Save(sysUserEntity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Refresh();
        }

        public ICommand EditCommand => new DelegateCommand<object>(Edit);

        private void Edit(object obj)
        {
            var parameters = new DialogParameters();

            parameters.Add("model", obj);

            _dialogService.ShowDialog(nameof(AddUserDialogView), parameters, DialogResult);
        }

        public ICommand RoleCommand => new DelegateCommand<object>(Role);

        private void Role(object obj)
        {
            var userInfoModel = obj as UserInfoModel;

            var param = new DialogParameters();

            param.Add("userId", userInfoModel.UserId);

            param.Add("roleIdList", userInfoModel.Roles.Select(x => x.RoleId).ToList());

            _dialogService.ShowDialog(nameof(ModifyRoleDialogView), param, DialogResult);
        }

        public ICommand PwdCommand => new DelegateCommand<object>(Pwd);

        private async void Pwd(object obj)
        {
            var model = obj as UserInfoModel;

            try
            {
                await _sysUserBLL.ResetPwd(model.UserId);

                MessageBox.Show("重置密码成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void Add()
        {
            var parameters = new DialogParameters();

            parameters.Add("model", new UserInfoModel());

            _dialogService.ShowDialog(nameof(AddUserDialogView), parameters, DialogResult);
        }

        public override void Refresh()
        {
            Task.Run(async () =>
            {
                var userList = await _sysUserBLL.All();

                var tempList = new List<Models.UserInfoModel>();

                for (var i = 0; i < userList.Count; i++)
                {
                    if (!userList[i].UserName.Contains(SearchValue) && !userList[i].RealName.Contains(SearchValue)) continue;

                    var roles = userList[i].Roles.Select(x => new RoleModel { RoleId = x.RoleId, RoleName = x.RoleName }).ToList();
                    tempList.Add(new Models.UserInfoModel
                    {
                        Index = i + 1,
                        UserName = userList[i].UserName,
                        RealName = userList[i].RealName,
                        Age = userList[i].UserAge,
                        UserIcon = userList[i].UserIcon,
                        Password = userList[i].Password,
                        UserId = userList[i].Id,
                        Roles = new ObservableCollection<RoleModel>(roles)
                    });

                }

                _dispatcher.Invoke(() =>
                {
                    if (Users.Count > 0) Users.Clear();

                    Users.AddRange(tempList);
                });
            });
        }

        private void DialogResult(IDialogResult dialogResult)
        {
            if (dialogResult.Result != ButtonResult.OK) return;

            MessageBox.Show("操作完成", "提示");

            Refresh();
        }
    }
}
