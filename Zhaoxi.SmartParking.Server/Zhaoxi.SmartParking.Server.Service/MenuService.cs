using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Core;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Service
{
    public class MenuService : BaseService, IMenuService
    {
        public MenuService(SqlSugarContext sqlSugarContext) : base(sqlSugarContext) { }


        public async Task<List<MenuModel>> GetAllMenus()
        {
            var list = await _sqlSugarClient.Queryable<MenuModel>().Where(x => x.State == 1).ToListAsync();

            var model = list.FirstOrDefault();

            return list;
        }
    }
}
