using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController
    {

        private readonly IRoleService _roleService;

        public RolesController(IRoleService menuService)
        {
            _roleService = menuService;
        }


        [HttpPost]
        [Route("all")]// 签权
        public async Task<Result<List<RoleModel>>> GetAllMenus()
        {
            var result = await _roleService.GetAllMenus();

            return new Result<List<RoleModel>> { Data = result, IsSuccess = true };
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<Result<bool>> Save([FromBody] RoleModel menuModel)
        {
            try
            {
                if (string.IsNullOrEmpty(menuModel.RoleName))
                {
                    throw new Exception($"菜单名称不能为空");
                }

                await _roleService.Save(menuModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }

        [HttpPost("saveRelation")]
        [Authorize]
        public async Task<Result<bool>> SaveRelation([FromBody] RoleModel menuModel)
        {
            try
            {
                await _roleService.SaveRelation(menuModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }
    }
}
