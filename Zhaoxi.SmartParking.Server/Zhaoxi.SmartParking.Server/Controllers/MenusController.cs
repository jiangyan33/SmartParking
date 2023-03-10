using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }


        [HttpGet]
        [Route("all")]// 签权
        public async Task<Result<List<MenuModel>>> GetAllMenus()
        {
            var result = await _menuService.GetAllMenus();

            return new Result<List<MenuModel>> { Data = result };
        }

    }
}
