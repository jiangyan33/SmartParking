using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Core;
using Zhaoxi.SmartParking.Server.IService;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.Service
{
    public class SysUserService : BaseService, ISysUserService
    {
        public SysUserService(SqlSugarContext sqlSugarContext) : base(sqlSugarContext) { }

        public async Task<SysUserModel> Login(string userName, string password)
        {
            var pwd = GetMD5Str(GetMD5Str(password) + "|" + userName);

            var list = await _sqlSugarClient.Queryable<SysUserModel>().Where(x => x.UserName == userName && x.Password == pwd).ToListAsync();

            var model = list.FirstOrDefault();

            if (model != null)
            {
                model.Token = AuthentationToken(model);
            }

            return model;
        }


        private string GetMD5Str(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            var result = Encoding.Default.GetBytes(str);

            var output = new MD5CryptoServiceProvider().ComputeHash(result);

            return BitConverter.ToString(output).Replace("-", "");
        }

        private string AuthentationToken(SysUserModel sysUserModel)
        {
            var token = "";

            if (sysUserModel == null) return token;

            var claims = new[] { new Claim(ClaimTypes.Sid, sysUserModel.Id.ToString()), new Claim(ClaimTypes.Name, sysUserModel.UserName) };

            // 大于16位
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456123456123456"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken("jiangyan.fun", "jiangyan", claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}
