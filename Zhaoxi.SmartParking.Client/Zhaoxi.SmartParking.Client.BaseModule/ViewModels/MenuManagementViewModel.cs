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
    public class MenuManagementViewModel : PageViewModelBase
    {
        public ObservableCollection<MenuItemModel> Menus { get; set; } = new ObservableCollection<MenuItemModel>();

        private List<MenuEntity> _origMenus = new List<MenuEntity>();

        private readonly IMenusBLL _menusBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public MenuManagementViewModel(IMenusBLL menusBLL, IDialogService dialogService)
        {
            PageTitle = "菜单管理";

            AddButtonText = "新增";

            _menusBLL = menusBLL;

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

                //await _sysUserBLL.Save(sysUserEntity);
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
                _origMenus = await _menusBLL.GetMenus(0);

                // 先加一个根节点
                var root = new MenuItemModel
                {
                    MenuId = 0,
                    MenuHeader = "根节点",
                    IsExpanded = true,
                    IsCurrent = true,
                    MenuType = 1
                };
                FillMenu(root.Children, root.MenuId);

                _dispatcher.Invoke(() =>
                {
                    if (Menus.Count > 0) Menus.Clear();

                    Menus.Add(root);
                });
            });
        }

        private void DialogResult(IDialogResult dialogResult)
        {
            if (dialogResult.Result == ButtonResult.Cancel) return;

            Refresh();
        }

        private void FillMenu(ObservableCollection<MenuItemModel> menus, int parentId)
        {
            var sub = _origMenus.Where(x => x.ParentId == parentId).OrderBy(x => x.Index).ToList();

            if (sub.Count > 0)
            {
                foreach (var item in sub)
                {
                    var model = new MenuItemModel
                    {
                        MenuId = item.MenuId,
                        MenuHeader = item.MenuHeader,
                        MenuIcon = item.MenuIcon,
                        TargetView = item.TargetView,
                        IsExpanded = true,
                        MenuType = item.MenuType,
                        Parent = item.ParentId,
                        Index = item.Index,
                    };

                    Application.Current?.Dispatcher.Invoke(() => menus.Add(model));

                    FillMenu(model.Children, item.MenuId);
                }

                menus.Last().IsLastChild = true;
            }
        }
    }
}
