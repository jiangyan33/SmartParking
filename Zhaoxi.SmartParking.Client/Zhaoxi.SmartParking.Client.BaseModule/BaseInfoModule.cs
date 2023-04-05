using Prism.Ioc;
using Prism.Modularity;
using Zhaoxi.SmartParking.Client.BaseModule.Views;

namespace Zhaoxi.SmartParking.Client.BaseModule
{
    public class BaseInfoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<FileUploadView>();
            containerRegistry.RegisterForNavigation<UserManagementView>();
            containerRegistry.RegisterForNavigation<MenuManagementView>();

            containerRegistry.RegisterDialog<AddFileDialogView>();
            containerRegistry.RegisterDialog<AddUserDialogView>();
            containerRegistry.RegisterDialog<AddMenuDialogView>();
        }
    }
}