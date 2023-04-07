using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Windows;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.BLL;
using Zhaoxi.SmartParking.Client.DAL;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;
using Zhaoxi.SmartParking.Client.Views;

namespace Zhaoxi.SmartParking.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        public App()
        {
            LocalDataAccess.InitTables();
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        protected override void InitializeShell(Window shell)
        {
            if (Container.Resolve<LoginWindow>().ShowDialog() == true)
            {
                base.InitializeShell(shell);
            }
            else Current.Shutdown();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 单例
            //containerRegistry.RegisterSingleton<Dispatcher>();


            // BLL
            containerRegistry.Register<ISysUserBLL, SysUserBLL>();
            containerRegistry.Register<IFilesBLL, FilesBLL>();
            containerRegistry.Register<IMenusBLL, MenusBLL>();
            containerRegistry.Register<IRolesBLL, RolesBLL>();

            // DAL
            containerRegistry.Register<ISysUserDAL, SysUserDAL>();
            containerRegistry.Register<IFilesDAL, FilesDAL>();
            containerRegistry.Register<IMenusDAL, MenusDAL>();
            containerRegistry.Register<IRolesDAL, RolesDAL>();

            // 注册弹出框窗口
            containerRegistry.RegisterDialogWindow<DialogWindow>();

        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            // 自动更新的时候    自动扫描目录
            return new DirectoryModuleCatalog() { ModulePath = Environment.CurrentDirectory + "\\Modules" };
        }

        // 
    }
}
