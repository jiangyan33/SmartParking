using Prism.Ioc;
using Prism.Modularity;
using Zhaoxi.SmartParking.Client.AutoModule.Views;

namespace Zhaoxi.SmartParking.Client.AutoModule
{
    public class AutoInfoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AutoRegisterView>();

            containerRegistry.RegisterDialog<AddAutoRegisterDialogView>();
        }
    }
}
