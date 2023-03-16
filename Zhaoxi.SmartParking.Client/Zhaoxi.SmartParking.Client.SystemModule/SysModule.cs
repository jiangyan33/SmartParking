using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Zhaoxi.SmartParking.Client.SystemModule.Views;

namespace Zhaoxi.SmartParking.Client.SystemModule
{
    public class SysModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // 把默认显示

            // RegionManager
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("LeftMenuTreeRegion", nameof(TreeMenuView));
            regionManager.RegisterViewWithRegion("MainHeaderRegion", nameof(MainHeaderView));

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<TreeMenuView>();
            containerRegistry.Register<MainHeaderView>();
        }
    }
}
