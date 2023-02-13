using System.Collections.Generic;
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
            //[{ "Index":0,"FileName":"\u6D4B\u8BD5001","FileUrl":null,"FileLen":0,"State":null,"ErrorMsg":null},{ "Index":0,"FileName":"\u6D4B\u8BD5002","FileUrl":null,"FileLen":0,"State":null,"ErrorMsg":null},{ "Index":0,"FileName":"\u6D4B\u8BD5003","FileUrl":null,"FileLen":0,"State":null,"ErrorMsg":null},{ "Index":0,"FileName":"\u6D4B\u8BD5004","FileUrl":null,"FileLen":0,"State":null,"ErrorMsg":null}]

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
