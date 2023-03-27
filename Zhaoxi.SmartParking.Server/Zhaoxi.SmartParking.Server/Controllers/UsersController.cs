using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ISysUserService _sysUserService;

        private readonly IConfiguration _configuration;


        public UsersController(ISysUserService sysUserService, IConfiguration configuration)
        {
            _sysUserService = sysUserService;

            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<Result<SysUserModel>> Login([FromBody] SysUserModel sysUserModel)
        {
            var result = new Result<SysUserModel>
            {
                Data = await _sysUserService.Login(sysUserModel.UserName, sysUserModel.Password)
            };

            if (result.Data == null)
            {
                result.Message = "登录失败，用户名或者密码错误";
            }
            else
            {
                if (!string.IsNullOrEmpty(result.Data.UserIcon))
                {
                    var url = _configuration["Urls"] + "/api/Files/GetImage/";

                    result.Data.UserIcon = url + result.Data.UserIcon;

                }
                result.IsSuccess = true;
            }
            return result;
        }

        [HttpPost("all")]
        [Authorize]
        public async Task<Result<List<SysUserModel>>> All()
        {
            var list = await _sysUserService.All();

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.UserIcon))
                {
                    var url = _configuration["Urls"] + "/api/Files/GetImage/";

                    item.UserIcon = url + item.UserIcon;

                }
            }

            return new Result<List<SysUserModel>> { IsSuccess = true, Data = list };
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<Result<bool>> Save([FromBody] SysUserModel sysUserModel)
        {
            try
            {
                await _sysUserService.Save(sysUserModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }

        [HttpPost("resetPwd/{userId}")]
        [Authorize]
        public async Task<Result<bool>> ResetPwd(int userId)
        {
            try
            {
                await _sysUserService.ResetPwd(userId);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("测试成功");
        }
    }
}
