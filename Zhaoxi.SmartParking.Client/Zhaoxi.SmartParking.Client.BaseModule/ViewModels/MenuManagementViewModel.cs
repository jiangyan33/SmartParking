using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            var model = obj as MenuItemModel;

            var ret = MessageBox.Show($"是否要删除该条记录[{model.MenuHeader}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (ret == MessageBoxResult.Cancel) return;

            try
            {
                var item = new MenuEntity
                {
                    MenuId = model.MenuId,
                    MenuHeader = model.MenuHeader,
                    Index = model.Index,
                    MenuIcon = model.MenuIcon,
                    TargetView = model.TargetView,
                    ParentId = model.Parent,
                    MenuType = model.MenuType,
                    State = 0
                };
                await _menusBLL.Save(item);
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

            parameters.Add("parentList", _origMenus);

            parameters.Add("model", obj);

            _dialogService.ShowDialog(nameof(AddMenuDialogView), parameters, DialogResult);
        }


        #region 拖动命令

        public ICommand MouseMoveCommand => new DelegateCommand<object>(MouseMove);

        private void MouseMove(object obj)
        {
            MouseEventArgs ea = obj as MouseEventArgs;

            if (ea == null) return;

            if (ea.LeftButton != MouseButtonState.Pressed) return;

            if (ea.OriginalSource is FrameworkElement frameworkElement)
            {

                if ((frameworkElement.Parent as FrameworkElement).Tag is MenuItemModel model)
                {
                    //创建一个拖动对象，传一个节点过去
                    DragDrop.DoDragDrop(frameworkElement, model, DragDropEffects.Move);
                }
            }
        }

        public ICommand DropCommand => new DelegateCommand<object>(Drop);

        private void Drop(object obj)
        {
            DragEventArgs ea = obj as DragEventArgs;
            ea.Handled = true;
            // 被拖动的Model
            var model = ea.Data.GetData(typeof(MenuItemModel)) as MenuItemModel;

            var frameworkElement = ea.OriginalSource as FrameworkElement;

            if ((frameworkElement.Parent as FrameworkElement).Tag is MenuItemModel targetModel)
            {
                var list = GetMenuItemModel(Menus, targetModel.Parent);

                // 当前拖动的对象放到目标对象的前面
                if (targetModel.OverLocation == 1)
                {
                    Move(list, model, targetModel.MenuId, 1);
                }
                else if (targetModel.OverLocation == 3)// 当前拖动对象放置到目标对象的后面
                {
                    Move(list, model, targetModel.MenuId, -1);
                }
                else if (targetModel.OverLocation == 2)// 当前拖动对象放置到目标对象的子项
                {
                    Move(targetModel, model, -1, -1);
                }
                targetModel.OverLocation = 0;

                // 处理IsLastChild
                //if (list.Count > 0)
                //    list.Last().IsLastChild = true;

                //var count = targetModel.Children.Count;
                //for (int i = 0; i < count; i++)
                //{
                //    targetModel.Children[i].Index = i;
                //    targetModel.Children[i].IsLastChild = false;
                //}
                //if (count > 0)
                //    targetModel.Children.Last().IsLastChild = true;
            }
        }

        public ICommand DragOverCommand => new DelegateCommand<object>(DragOver);

        private void DragOver(object obj)
        {
            DragEventArgs ea = obj as DragEventArgs;// 实际的页面对象   不是Model
            ea.Handled = true;

            var frameworkElement = ea.OriginalSource as FrameworkElement;

            if ((frameworkElement.Parent as FrameworkElement).Tag is MenuItemModel targetModel)
            {
                targetModel.OverLocation = 0;// 光标移动的位置
                var model = ea.Data.GetData(typeof(MenuItemModel)) as MenuItemModel;
                if (model == null || model.MenuId == targetModel.MenuId) return;

                TreeViewItem tvi = this.FindParent<TreeViewItem>(ea.Source as DependencyObject);

                Point point = ea.GetPosition(tvi);

                if (point.Y <= 15)
                {
                    targetModel.OverLocation = 1;
                }
                else if (targetModel.MenuType == 1 && point.Y > 15 && point.Y < tvi.ActualHeight - 15)
                {
                    targetModel.OverLocation = 2;
                }
                else if ((!targetModel.IsExpanded || targetModel.MenuType == 0) && targetModel.Children.Count == 0 && point.Y >= tvi.ActualHeight - 15)
                {
                    targetModel.OverLocation = 3;
                }
                else
                {
                    targetModel.OverLocation = 0;
                }
            }
        }

        public ICommand DragLeaveCommand => new DelegateCommand<object>(DragLeave);

        private void DragLeave(object obj)
        {
            //Debug.WriteLine("DragLeave----" + obj.GetType().FullName);
            DragEventArgs ea = obj as DragEventArgs;// 实际的页面对象   不是Model

            var frameworkElement = ea.OriginalSource as FrameworkElement;

            if ((frameworkElement.Parent as FrameworkElement).Tag is MenuItemModel targetModel)
            {
                targetModel.OverLocation = 0;
            }

        }
        #endregion

        public override void Add()
        {
            var parameters = new DialogParameters();

            parameters.Add("parentList", _origMenus);

            parameters.Add("model", new MenuItemModel());

            _dialogService.ShowDialog(nameof(AddMenuDialogView), parameters, DialogResult);
        }

        public override void Refresh()
        {
            Task.Run(async () =>
            {
                _origMenus = await _menusBLL.GetAllMenus(0);

                _origMenus.Add(new MenuEntity
                {
                    MenuId = 0,
                    MenuHeader = "根节点",
                    MenuType = 1,
                    ParentId = -1
                });

                // 先加一个根节点
                var root = new MenuItemModel
                {
                    MenuId = 0,
                    MenuHeader = "根节点",
                    IsExpanded = true,
                    //IsCurrent = true,
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

        private T FindParent<T>(DependencyObject i_dp) where T : DependencyObject
        {
            DependencyObject dobj = VisualTreeHelper.GetParent(i_dp);
            if (dobj != null)
            {
                if (dobj is T)
                {
                    return (T)dobj;
                }
                else
                {
                    dobj = FindParent<T>(dobj);
                    if (dobj != null && dobj is T)
                    {
                        return (T)dobj;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 将model从容器菜单容器中移除，放入到parentMenuItemModel的children中，放入位置为前、后、最后,当menuId为-1时放最后，当type=1时放前面,type为-1时放后面
        /// </summary>
        /// <param name="parentMenuItemModel"></param>
        /// <param name="model"></param>
        /// <param name="menuId"></param>
        /// <param name="type"></param>
        private void Move(MenuItemModel parentMenuItemModel, MenuItemModel model, int menuId, int type)
        {
            // 将model从原来的容器中移除，放到新的parent中，并且指定放入的位置
            _dispatcher.Invoke(() =>
            {
                // 递归
                var modelParentModel = GetMenuItemModel(Menus, model.Parent);

                // 触发移除操作
                Debug.WriteLine($"触发移除操作，父容器数量:{parentMenuItemModel.Children.Count}");

                modelParentModel.Children.Remove(model);

                if (modelParentModel.Children.Count > 0)
                    modelParentModel.Children.Last().IsLastChild = true;

                // 变更到数据库中的顺序索引
                var menuIndex = 0;

                if (menuId == -1)
                {
                    // 放入到最后
                    parentMenuItemModel.Children.Add(model);

                    if (parentMenuItemModel.Children.Count == 0)
                    {
                        menuIndex = 0;
                    }
                    else
                    {
                        menuIndex = parentMenuItemModel.Children.LastOrDefault().Index + 1000;
                    }
                }
                else
                {
                    var index = parentMenuItemModel.Children.ToList().FindIndex(x => x.MenuId == menuId);

                    // 变更的子容器中的插入位置
                    var insertId = 0;

                    if (type == 1)
                    {
                        insertId = index;

                        menuIndex = parentMenuItemModel.Children[index].Index - 1;
                    }
                    else
                    {
                        insertId = index + 1;

                        // 如果menuId后面没有数据
                        if (parentMenuItemModel.Children.Count - 1 == index)
                        {
                            menuIndex = parentMenuItemModel.Children[index].Index + 1000;
                        }
                        else
                        {
                            menuIndex = (parentMenuItemModel.Children[index + 1].Index + parentMenuItemModel.Children[index].Index) / 2;
                        }
                    }

                    parentMenuItemModel.Children.Insert(insertId, model);
                }

                foreach (var item in parentMenuItemModel.Children)
                {
                    item.IsLastChild = false;
                }

                parentMenuItemModel.Children.LastOrDefault().IsLastChild = true;
                // todo：更新到数据库中
                Debug.WriteLine("更新到数据库中。。" + menuIndex + "----" + model.MenuHeader);
            });
        }


        /// <summary>
        /// 找到modelId对应节点的父节点的所有字节点
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
