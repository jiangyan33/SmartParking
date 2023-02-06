using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.BLL
{
    public class SysUserBLL : ISysUserBLL
    {

        private readonly ISysUserDAL _sysUserDAL;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public SysUserBLL(ISysUserDAL sysUserDAL)
        {
            _sysUserDAL = sysUserDAL;

            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<SysUserEntity> Login(string userName, string password)
        {
            var str = await _sysUserDAL.Login(userName, password);

            var result = JsonSerializer.Deserialize<ResultEntity<SysUserEntity>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }
    }
}
