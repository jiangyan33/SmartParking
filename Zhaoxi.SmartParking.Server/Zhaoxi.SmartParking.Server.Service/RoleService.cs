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
    public class RoleService : BaseService, IRoleService
    {

        public RoleService(SqlSugarContext sqlSugarContext) : base(sqlSugarContext) { }


        public async Task<List<RoleModel>> GetAllMenus()
        {
            var list = await _sqlSugarClient.Queryable<RoleModel>().Where(x => x.State == 1).ToListAsync();

            var model = list.FirstOrDefault();

            return list;
        }

        public async Task Save(RoleModel roleModel)
        {
            if (roleModel.RoleId == 0)
            {
                var list = await _sqlSugarClient.Queryable<RoleModel>().Where(x => x.RoleName == roleModel.RoleName && x.State == 1).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"菜单名称[{roleModel.RoleName}]已经存在");
                }

                roleModel.State = 1;

                await _sqlSugarClient.Insertable(roleModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<RoleModel>().Where(x => x.RoleId == roleModel.RoleId && x.State == 1).ToListAsync();

                if (list.Count == 0) return;

                list[0].RoleName = roleModel.RoleName;

                list[0].State = roleModel.State;

                await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();

                if (list[0].State == 0)
                {
                    // 删除关联关系

                    await _sqlSugarClient.Deleteable<UserRoleModel>(x => x.RoleId == list[0].RoleId).ExecuteCommandAsync();

                    await _sqlSugarClient.Deleteable<RoleMenuModel>(x => x.RoleId == list[0].RoleId).ExecuteCommandAsync();

                }
            }
        }

        public async Task SaveRelation(RoleModel roleModel)
        {
            try
            {
                _sqlSugarClient.BeginTran();

                // 移除用户角色关系
                await _sqlSugarClient.Deleteable<UserRoleModel>(x => x.RoleId == roleModel.RoleId).ExecuteCommandAsync();

                // 新增用户角色关系
                var userRoleModelList = roleModel.UserIdList.Select(x => new UserRoleModel { UserId = x, RoleId = roleModel.RoleId }).ToList();

                if (userRoleModelList.Count > 0)
                {
                    await _sqlSugarClient.Insertable(userRoleModelList).ExecuteCommandAsync();
                }

                // 移除角色菜单关系
                await _sqlSugarClient.Deleteable<RoleMenuModel>(x => x.RoleId == roleModel.RoleId).ExecuteCommandAsync();

                // 新增角色菜单关系
                var roleMenuModelList = roleModel.MenuIdList.Select(x => new RoleMenuModel { MenuId = x, RoleId = roleModel.RoleId }).ToList();

                if (roleMenuModelList.Count > 0)
                {
                    await _sqlSugarClient.Insertable(roleMenuModelList).ExecuteCommandAsync();
                }
                _sqlSugarClient.CommitTran();
            }
            catch (Exception ex)
            {
                _sqlSugarClient.RollbackTran();

                throw new Exception(ex.Message);
            }
        }
    }
}
