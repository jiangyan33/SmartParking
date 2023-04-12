using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Zhaoxi.SmartParking.Client.MainModule.Views;

namespace Zhaoxi.SmartParking.Client.MainModule
{
    public class MainInfoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("MainViewRegion", "DashboardView");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DashboardView>();
        }
    }
}
