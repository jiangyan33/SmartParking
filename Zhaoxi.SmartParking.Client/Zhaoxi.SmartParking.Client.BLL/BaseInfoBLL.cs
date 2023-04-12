using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.BLL
{
    public class BaseInfoBLL : IBaseInfoBLL
    {
        private readonly IBaseInfoDAL _baseInfoDAL;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BaseInfoBLL(IBaseInfoDAL baseInfoDAL)
        {
            _baseInfoDAL = baseInfoDAL;

            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<List<BaseAutoColorEntity>> GetAllAutoColor()
        {
            var str = await _baseInfoDAL.GetAllAutoColor();

            var result = JsonSerializer.Deserialize<ResultEntity<List<BaseAutoColorEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<List<BaseLicenseColorEntity>> GetAllLicenseColor()
        {
            var str = await _baseInfoDAL.GetAllLicenseColor();

            var result = JsonSerializer.Deserialize<ResultEntity<List<BaseLicenseColorEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<List<BaseFeeEntity>> GetAllFeeModel()
        {
            var str = await _baseInfoDAL.GetAllFeeModel();

            var result = JsonSerializer.Deserialize<ResultEntity<List<BaseFeeEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> SaveAutoColor(BaseAutoColorEntity baseAutoColorEntity)
        {
            var str = await _baseInfoDAL.SaveAutoColor(baseAutoColorEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> SaveLicenseColor(BaseLicenseColorEntity baseLicenseColorEntity)
        {
            var str = await _baseInfoDAL.SaveLicenseColor(baseLicenseColorEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> SaveFeeModel(BaseFeeEntity baseFeeEntity)
        {
            var str = await _baseInfoDAL.SaveFeeModel(baseFeeEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }


    }
}
