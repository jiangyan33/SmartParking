using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<int>> GetMenus(int roleId)
        {
            // 删除的时候需要删除关联关系
            var list = await _sqlSugarClient.Queryable<RoleMenuModel>().
                LeftJoin<MenuModel>((roleMenu, menu) => roleMenu.MenuId == menu.MenuId).
                Where((roleMenu, menu) => roleMenu.RoleId == roleId && menu.State == 1).Select((roleMenu, menu) => roleMenu.MenuId).ToListAsync();

            return list;
        }

        public async Task Save(MenuModel menuModel)
        {
            if (menuModel.MenuId == 0)
            {
                var list = await _sqlSugarClient.Queryable<MenuModel>().Where(x => x.MenuHeader == menuModel.MenuHeader && x.State == 1).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"菜单名称[{menuModel.MenuHeader}]已经存在");
                }

                menuModel.State = 1;

                await _sqlSugarClient.Insertable(menuModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<MenuModel>().Where(x => x.MenuId == menuModel.MenuId && x.State == 1).ToListAsync();

                if (list.Count == 0) return;

                list[0].TargetView = menuModel.TargetView;

                list[0].MenuHeader = menuModel.MenuHeader;

                list[0].ParentId = menuModel.ParentId;

                list[0].MenuIcon = menuModel.MenuIcon;

                list[0].Index = menuModel.Index;

                list[0].MenuType = menuModel.MenuType;

                list[0].State = menuModel.State;

                await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();
            }
        }
    }
}
