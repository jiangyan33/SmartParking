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
    public class AddMenuDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; }

        /// <summary>
        /// 选择的父节点
        /// </summary>
        public MenuEntity CurrentParentMenu { get; set; }

        private List<MenuEntity> _origMenus;

        public event Action<IDialogResult> RequestClose;

        private MenuItemModel _mainModel;

        public MenuItemModel MenuModel
        {
            get { return _mainModel; }
            set { SetProperty(ref _mainModel, value); }
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
            MenuModel = parameters.GetValue<MenuItemModel>("model");

            _origMenus = parameters.GetValue<List<MenuEntity>>("parentList");

            var parentNodes = _origMenus.Where(x => x.MenuType == 1).ToList();

            ParentNodes.AddRange(parentNodes);

            if (MenuModel.MenuId == 0)
            {
                Title = "添加菜单";

                CurrentParentMenu = parentNodes[0];
            }
            else
            {
                Title = $"编辑菜单[{MenuModel.MenuHeader}]";

                CurrentParentMenu = parentNodes.Find(x => x.MenuId == MenuModel.Parent);
            }
        }

        public List<string> IconList { get; set; } = new List<string>();

        private readonly IMenusBLL _menusBLL;

        public ObservableCollection<MenuEntity> ParentNodes { get; set; } = new ObservableCollection<MenuEntity>();

        public AddMenuDialogViewModel(IMenusBLL menusBLL)
        {
            IconList.AddRange(new List<string> { "\ue618", "\ue6b8", "\ue600", "\ue66d", "\ue624", "\ue604", "\ue68f", "\ue635", "\ue609", "\ue605", "\ue62a", "\ue658" });

            _menusBLL = menusBLL;
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
                var model = new MenuEntity
                {
                    MenuId = MenuModel.MenuId,
                    MenuHeader = MenuModel.MenuHeader,
                    TargetView = MenuModel.TargetView,
                    ParentId = CurrentParentMenu.MenuId,
                    MenuIcon = MenuModel.MenuIcon,
                    Index = MenuModel.Index,
                    MenuType = MenuModel.MenuType,
                    State = 1
                };

                if (!string.IsNullOrEmpty(model.MenuIcon))
                {
                    model.MenuIcon = ((int)model.MenuIcon.ToArray()[0]).ToString("x");
                }

                if (model.MenuId == 0)
                {
                    model.Index = _origMenus.ToList().Count(x => x.ParentId == CurrentParentMenu.MenuId) + 1000;
                }

                await _menusBLL.Save(model);

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}