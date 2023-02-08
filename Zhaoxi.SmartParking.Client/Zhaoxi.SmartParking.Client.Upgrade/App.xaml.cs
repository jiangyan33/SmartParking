using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Zhaoxi.SmartParking.Client.Upgrade.Models;

namespace Zhaoxi.SmartParking.Client.Upgrade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 0)
            {
                Current.Shutdown();
            }

            FileModelList = System.Text.Json.JsonSerializer.Deserialize<List<FileModel>>(e.Args[0]);

            // 这里可以接收参数
            base.OnStartup(e);
        }

        public static List<FileModel> FileModelList { get; private set; }
    }
}
