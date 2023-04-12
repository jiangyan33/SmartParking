using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;
using Zhaoxi.SmartParking.Server.Models.Dto;

namespace Zhaoxi.SmartParking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoRegisterController
    {

        private readonly IAutoRegisterService _autoRegisterService;

        public AutoRegisterController(IAutoRegisterService autoRegisterService)
        {
            _autoRegisterService = autoRegisterService;
        }


        [HttpPost]
        [Route("pages")]// 签权
        public async Task<PageResult<List<AutoRegisterModel>>> Pages(PageQuery pageQuery)
        {
            return await _autoRegisterService.Pages(pageQuery);
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<Result<bool>> Save([FromBody] AutoRegisterModel autoRegisterModel)
        {
            try
            {
                if (string.IsNullOrEmpty(autoRegisterModel.AutoLicense))
                {
                    throw new Exception($"车牌号不能为空");
                }

                await _autoRegisterService.Save(autoRegisterModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }
    }

}
