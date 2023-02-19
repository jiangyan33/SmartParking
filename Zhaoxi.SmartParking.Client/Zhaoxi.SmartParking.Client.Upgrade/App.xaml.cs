using System;
using System.Collections.Generic;
using System.Windows;
using Zhaoxi.SmartParking.Client.Upgrade.DataAccess;
using Zhaoxi.SmartParking.Client.Upgrade.Models;

namespace Zhaoxi.SmartParking.Client.Upgrade
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            LocalDataAccess.InitTables();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // 程序运行需要的核心文件Zhaoxi.SmartParking.Client.Upgrade.runtimeconfig.json  Zhaoxi.SmartParking.Client.Upgrade.deps.json
            // 需要的文件参数:文件名称、文件大小、文件路径、文件MD5,每一个文件之间使用【;】进行分割，每个属性之间使用【|】进行分割
            var str = e.Args;
#if DEBUG
            var msg = "麻精药品智能管理柜.doc|3457842|http://localhost:5000/api/Files/download/麻精药品智能管理柜.doc|123456;WeGameMiniLoader.std.2.07.29.1736.exe|8124096|http://localhost:5000/api/Files/download/WeGameMiniLoader.std.2.07.29.1736.exe|12345678";

            str = new string[] { msg };
#endif

            if (str.Length == 0)
            {
                Current.Shutdown();

                return;
            }

            var fileArray = str[0].Split(";");

            for (var i = 0; i < fileArray.Length; i++)
            {
                var fileInfo = fileArray[i].Split("|");

                FileModelList.Add(new FileModel
                {
                    Index = i + 1,
                    FileName = fileInfo[0],
                    FileLen = Convert.ToInt32(fileInfo[1]),
                    FileUrl = fileInfo[2],
                    MD5 = fileInfo[3],
                    State = "待更新"
                });
            }

            // 这里可以接收参数
            base.OnStartup(e);
        }

        public static List<FileModel> FileModelList { get; private set; } = new List<FileModel>();
    }
}
