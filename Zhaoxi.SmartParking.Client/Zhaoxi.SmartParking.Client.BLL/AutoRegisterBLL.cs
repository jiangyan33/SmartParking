using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.BLL
{
    public class AutoRegisterBLL : IAutoRegisterBLL
    {
        private readonly IAutoRegisterDAL _autoRegisterDAL;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AutoRegisterBLL(IAutoRegisterDAL autoRegisterDAL)
        {
            _autoRegisterDAL = autoRegisterDAL;

            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }
        public async Task<PageResult<List<AutoRegisterEntity>>> Pages(int pageNo, int pageSize, string search)
        {
            var str = await _autoRegisterDAL.Pages(pageNo, pageSize, search);

            var result = JsonSerializer.Deserialize<PageResult<List<AutoRegisterEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result;
        }
    }
}
