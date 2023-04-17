using Prism.Ioc;
using Prism.Modularity;

namespace Zhaoxi.SmartParking.Client.ReportModule
{
    public class ReportModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.ReportView>();
        }
    }
}
