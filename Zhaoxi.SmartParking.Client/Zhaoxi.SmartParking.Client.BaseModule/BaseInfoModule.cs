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
            containerRegistry.RegisterForNavigation<RoleManagementView>();
            containerRegistry.RegisterForNavigation<AutoColorView>();
            containerRegistry.RegisterForNavigation<LicenseColorView>();
            containerRegistry.RegisterForNavigation<FeeModelView>();

            containerRegistry.RegisterDialog<AddFileDialogView>();
            containerRegistry.RegisterDialog<AddUserDialogView>();
            containerRegistry.RegisterDialog<AddMenuDialogView>();
            containerRegistry.RegisterDialog<AddRoleDialogView>();
            containerRegistry.RegisterDialog<SelectUserDialogView>();
            containerRegistry.RegisterDialog<ModifyRoleDialogView>();
            containerRegistry.RegisterDialog<AddFeeModelDialogView>();
        }
    }
}