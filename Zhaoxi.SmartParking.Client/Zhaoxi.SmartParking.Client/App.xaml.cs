﻿using Prism.Ioc;
using Prism.Unity;
using System.Windows;
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
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
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
            // BLL
            containerRegistry.Register<ISysUserBLL, SysUserBLL>();


            // DAL
            containerRegistry.Register<ISysUserDAL, SysUserDAL>();
        }
    }
}
