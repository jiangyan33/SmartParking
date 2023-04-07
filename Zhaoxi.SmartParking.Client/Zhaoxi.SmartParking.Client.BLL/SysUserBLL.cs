using System.Collections.Generic;
using System.Globalization;
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

        public async Task<List<SysUserEntity>> All()
        {
            var str = await _sysUserDAL.All();

            var result = JsonSerializer.Deserialize<ResultEntity<List<SysUserEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<List<SysUserEntity>> Users(int roleId)
        {
            var str = await _sysUserDAL.GetUsers(roleId);

            var result = JsonSerializer.Deserialize<ResultEntity<List<SysUserEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> Save(SysUserEntity sysUserEntity)
        {
            var str = await _sysUserDAL.Save(sysUserEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<bool> ResetPwd(int userId)
        {
            var str = await _sysUserDAL.ResetPwd(userId);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }


        public async Task<SysUserEntity> Login(string userName, string password)
        {
            var str = await _sysUserDAL.Login(userName, password);

            var result = JsonSerializer.Deserialize<ResultEntity<SysUserEntity>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }

            foreach (var item in result.Data.Menus)
            {
                if (!string.IsNullOrEmpty(item.MenuIcon))
                {
                    item.MenuIcon = ((char)int.Parse(item.MenuIcon, NumberStyles.HexNumber)).ToString();
                }
            }
            return result.Data;
        }
    }
}
