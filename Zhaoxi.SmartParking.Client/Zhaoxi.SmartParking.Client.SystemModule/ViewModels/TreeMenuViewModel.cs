using Prism.Commands;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.SystemModule.Models;

namespace Zhaoxi.SmartParking.Client.SystemModule.ViewModels
{
    public class TreeMenuViewModel
    {
        private List<MenuEntity> _origMenus;

        /// <summary>
        /// 这个地方有需要使用接口注入，使用实现注入不好用
        /// _regionManager.RequestNavigate("MainViewRegion", model.TargetView);
        /// </summary>
        private readonly IRegionManager _regionManager;

        public ObservableCollection<MenuItemModel> Menus { get; set; } = new ObservableCollection<MenuItemModel>();


        public TreeMenuViewModel(IMenusBLL menusBLL, IRegionManager regionManager)
        {
            _regionManager = regionManager;

            OpenViewCommand = new DelegateCommand<MenuItemModel>(DoOpenViewCommand);

            // 需要初始化的时候通过API接口获取
            Task.Run(async () =>
            {
                _origMenus = await menusBLL.GetMenus(0);

                FillMenu(Menus, 0);
            });
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
                        MenuHeader = item.MenuHeader,
                        MenuIcon = item.MenuIcon,
                        TargetView = item.TargetView
                    };

                    Application.Current?.Dispatcher.Invoke(() => menus.Add(model));

                    FillMenu(model.Children, item.MenuId);
                }
            }
        }

        public ICommand OpenViewCommand { get; set; }

        private void DoOpenViewCommand(MenuItemModel model)
        {
            if ((model.Children == null || model.Children.Count == 0) && !string.IsNullOrEmpty(model.TargetView))
            {
                _regionManager.RequestNavigate("MainViewRegion", model.TargetView);
            }
            else
            {
                model.IsExpanded = !model.IsExpanded;
            }
        }

    }
}
