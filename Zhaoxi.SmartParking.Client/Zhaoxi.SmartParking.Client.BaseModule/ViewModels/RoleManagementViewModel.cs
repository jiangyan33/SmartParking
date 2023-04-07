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
    public class RoleManagementViewModel : PageViewModelBase
    {
        public ObservableCollection<RoleModel> Roles { get; set; } = new ObservableCollection<RoleModel>();

        public ObservableCollection<UserInfoModel> Users { get; set; } = new ObservableCollection<UserInfoModel>();

        public ObservableCollection<MenuItemModel> Menus { get; set; } = new ObservableCollection<MenuItemModel>();

        private RoleModel _currentRole;

        public RoleModel CurrentRole
        {
            get { return _currentRole; }
            set { SetProperty(ref _currentRole, value); }
        }

        private readonly IRolesBLL _rolesBLL;

        private readonly ISysUserBLL _sysUserBLL;

        private readonly IMenusBLL _menusBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;


        #region 通用方法

        public RoleManagementViewModel(IRolesBLL rolesBLL, ISysUserBLL sysUserBLL, IMenusBLL menusBLL, IDialogService dialogService)
        {
            PageTitle = "权限管理";

            AddButtonText = "新增";

            _rolesBLL = rolesBLL;

            _sysUserBLL = sysUserBLL;

            _menusBLL = menusBLL;

            _dialogService = dialogService;

            _dispatcher = Application.Current.Dispatcher;

            Refresh();
        }

        public ICommand MenuSelectedCommand => new DelegateCommand<object>(MenuSelected);

        private void MenuSelected(object obj)
        {
            if (obj is MenuItemModel menuItemModel)
            {
                var value = menuItemModel.IsSelected;

                // 选择的时候使用
                if (!value && menuItemModel.Children != null)
                {
                    menuItemModel.Children.ToList().ForEach(m => m.IsSelected = false);
                }

                var parent = GetMenuItemModel(Menus, menuItemModel.Parent);

                if (parent == null) return;

                if (value && parent.MenuId > 0)
                    parent.IsSelected = true;
                if (!value && parent != null && !parent.Children.ToList().Exists(c => c.IsSelected))
                {
                    parent.IsSelected = false;
                }
            }
        }

        public ICommand AddUserCommand => new DelegateCommand(AddUser);

        private void AddUser()
        {
            var parameters = new DialogParameters();

            parameters.Add("userIdList", Users.Select(x => x.UserId).ToList());

            _dialogService.ShowDialog(nameof(SelectUserDialogView), parameters, (dialogResult) =>
            {
                if (dialogResult.Result == ButtonResult.OK)
                {
                    var list = dialogResult.Parameters.GetValue<List<UserInfoModel>>("users");

                    if (list.Count > 0)
                    {
                        _dispatcher.Invoke(() => Users.AddRange(list));
                    }
                }
            });
        }

        public ICommand SaveCommand => new DelegateCommand(Save);

        private async void Save()
        {
            try
            {
                // 获取选择的用户信息
                var userIdList = Users.Select(x => x.UserId).ToList();

                // 获取选择的菜单信息
                var menuIdList = new List<int>();

                GetSelectedMenus(Menus, menuIdList);

                //保存
                var item = new RoleEntity
                {
                    RoleId = CurrentRole.RoleId,
                    UserIdList = userIdList,
                    MenuIdList = menuIdList,
                };
                await _rolesBLL.SaveRelation(item);

                MessageBox.Show("操作成功", "提示信息", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Refresh();
        }

        public ICommand RemoveUserCommand => new DelegateCommand<object>(RemoveUser);

        private void RemoveUser(object obj)
        {
            var model = obj as UserInfoModel;

            var ret = MessageBox.Show($"是否要删除该条记录[{model.UserName}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (ret == MessageBoxResult.Cancel) return;

            _dispatcher.Invoke(() => Users.Remove(model));
        }

        public ICommand DeleteCommand => new DelegateCommand<object>(Delete);

        private async void Delete(object obj)
        {
            var model = obj as RoleModel;

            var ret = MessageBox.Show($"是否要删除该条记录[{model.RoleName}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (ret == MessageBoxResult.Cancel) return;

            try
            {
                var item = new RoleEntity
                {
                    RoleId = model.RoleId,
                    RoleName = model.RoleName,
                    State = 0
                };
                await _rolesBLL.Save(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Refresh();
        }

        public ICommand ItemSelectedCommand => new DelegateCommand<object>(ItemSelected);

        private async void ItemSelected(object obj)
        {
            var model = obj as RoleModel;

            CurrentRole = model;

            await LoadUsersAndMenus();
        }

        public override void Add()
        {
            _dialogService.ShowDialog(nameof(AddRoleDialogView), DialogResult);
        }

        public override void Refresh()
        {
            CurrentRole = null;

            Task.Run(async () =>
            {
                var list = await _rolesBLL.GetAll();

                var tempList = list.Select(x => new RoleModel { RoleId = x.RoleId, RoleName = x.RoleName }).ToList();

                _dispatcher.Invoke(() =>
                  {
                      if (Roles.Count > 0) Roles.Clear();

                      Roles.AddRange(tempList);
                  });

                if (Roles.Count > 0)
                {
                    Roles[0].IsSelected = true;
                    CurrentRole = Roles[0];

                    await LoadUsersAndMenus();
                }
            });
        }

        private async Task LoadUsersAndMenus()
        {
            if (CurrentRole == null) return;

            var users = await _sysUserBLL.Users(CurrentRole.RoleId);

            var allMenus = await _menusBLL.GetAllMenus(0);

            var menus = await _menusBLL.GetMenus(CurrentRole.RoleId);

            _dispatcher.Invoke(() =>
            {
                if (Users.Count > 0) Users.Clear();

                Users.AddRange(users.Select(x => new UserInfoModel { UserId = x.Id, UserName = x.UserName }));

                if (Menus.Count > 0) Menus.Clear();

                FillMenus(allMenus, Menus, 0, menus);
            });
        }

        private void DialogResult(IDialogResult dialogResult)
        {
            if (dialogResult.Result == ButtonResult.Cancel) return;

            Refresh();
        }

        #endregion

        private void FillMenus(IList<MenuEntity> origMenus, IList<MenuItemModel> menus, int parentId, IList<int> role_menus)
        {
            var sub = origMenus.Where(m => m.ParentId == parentId).OrderBy(o => o.Index);

            if (sub.Count() > 0)
            {
                foreach (var item in sub)
                {
                    MenuItemModel mm = new MenuItemModel()
                    {
                        MenuId = item.MenuId,
                        MenuHeader = item.MenuHeader,
                        Parent = item.ParentId,
                        IsSelected = role_menus.Contains(item.MenuId),
                    };

                    FillMenus(origMenus, mm.Children, item.MenuId, role_menus);
                    menus.Add(mm);
                }
            }
        }

        private void GetSelectedMenus(ObservableCollection<MenuItemModel> menuItemModels, List<int> menuIdList)
        {
            foreach (var item in menuItemModels)
            {
                if (item.Children.Count > 0)
                {
                    GetSelectedMenus(item.Children, menuIdList);
                }

                if (item.IsSelected) menuIdList.Add(item.MenuId);
            }
        }

        /// <summary>
        /// 找到modelId对应节点的父节点
        /// </summary>
        /// <param name="children"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        private MenuItemModel GetMenuItemModel(ObservableCollection<MenuItemModel> children, int modelId)
        {
            foreach (var item in children)
            {
                if (item.MenuId == modelId)
                {
                    return item;
                }
                else
                {
                    return GetMenuItemModel(item.Children, modelId);
                }
            }

            return null;
        }
    }
}