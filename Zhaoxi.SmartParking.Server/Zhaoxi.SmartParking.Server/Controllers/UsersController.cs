using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("测试成功");
        }
    }
}
