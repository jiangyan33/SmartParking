using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class MenusDAL : IMenusDAL
    {
        public async Task<string> GetMenu(int id)
        {
            return await WebDataAccess.PostDatas("/menus/all", new { });
        }
    }
}
