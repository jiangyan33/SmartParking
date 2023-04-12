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
    public class BaseInfoController
    {

        private readonly IBaseInfoService _baseInfoService;

        public BaseInfoController(IBaseInfoService baseInfoService)
        {
            _baseInfoService = baseInfoService;
        }


        [HttpPost]
        [Route("allAutoColor")]
        public async Task<Result<List<BaseAutoColorModel>>> GetAllAutoColor()
        {
            var result = await _baseInfoService.GetAllAutoColor();

            return new Result<List<BaseAutoColorModel>> { Data = result, IsSuccess = true };
        }

        [HttpPost]
        [Route("allLicenseColor")]
        public async Task<Result<List<BaseLicenseColorModel>>> GetAllLicenseColor()
        {
            var result = await _baseInfoService.GetAllLicenseColor();

            return new Result<List<BaseLicenseColorModel>> { Data = result, IsSuccess = true };
        }

        [HttpPost]
        [Route("allFeeModel")]
        public async Task<Result<List<BaseFeeModel>>> GetAllFeeModel()
        {
            var result = await _baseInfoService.GetAllFeeModel();

            return new Result<List<BaseFeeModel>> { Data = result, IsSuccess = true };
        }

        [HttpPost("saveAutoColor")]
        [Authorize]
        public async Task<Result<bool>> SaveAutoColor([FromBody] BaseAutoColorModel baseAutoColorModel)
        {
            try
            {
                if (string.IsNullOrEmpty(baseAutoColorModel.ColorName))
                {
                    throw new Exception($"车身颜色不能为空");
                }

                await _baseInfoService.SaveAutoColor(baseAutoColorModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }

        [HttpPost("saveLicenseColor")]
        [Authorize]
        public async Task<Result<bool>> SaveLicenseColor([FromBody] BaseLicenseColorModel baseLicenseColorModel)
        {
            try
            {
                if (string.IsNullOrEmpty(baseLicenseColorModel.ColorName))
                {
                    throw new Exception($"车牌颜色不能为空");
                }

                await _baseInfoService.SaveLicenseColor(baseLicenseColorModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }

        [HttpPost("saveFeeModel")]
        [Authorize]
        public async Task<Result<bool>> SaveFeeModel([FromBody] BaseFeeModel baseFeeModel)
        {
            try
            {
                if (string.IsNullOrEmpty(baseFeeModel.FeeModelName))
                {
                    throw new Exception($"计费模式不能为空");
                }

                await _baseInfoService.SavelFeeModel(baseFeeModel);

                return new Result<bool> { IsSuccess = true, Data = true };
            }
            catch (System.Exception ex)
            {
                return new Result<bool> { Data = false, Message = ex.Message };
            }
        }
    }
}
