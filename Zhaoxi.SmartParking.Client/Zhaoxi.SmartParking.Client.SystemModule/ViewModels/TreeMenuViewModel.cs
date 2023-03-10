using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.SystemModule.Models;

namespace Zhaoxi.SmartParking.Client.SystemModule.ViewModels
{
    public class TreeMenuViewModel
    {
        public List<MenuItemModel> Menus { get; set; } = new List<MenuItemModel>();

        public TreeMenuViewModel(IMenusBLL menusBLL)
        {
            // 需要初始化的时候通过API接口获取

            Task.Run(async () =>
            {
                var res = await menusBLL.GetMenus(0);
            });
        }
    }
}
