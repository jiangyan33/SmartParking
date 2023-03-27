using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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

        public async Task<List<SysUserModel>> All()
        {
            var list = await _sqlSugarClient.Queryable<SysUserModel>().Where(x => x.UserState == 1).ToListAsync();

            return list;
        }

        public async Task Save(SysUserModel sysUserModel)
        {
            // 处理图片名称
            if (!string.IsNullOrEmpty(sysUserModel.UserIcon))
            {
                sysUserModel.UserIcon = System.IO.Path.GetFileName(sysUserModel.UserIcon);
            }
            else
            {
                sysUserModel.UserIcon = "";
            }

            if (sysUserModel.Id == 0)
            {
                var list = await _sqlSugarClient.Queryable<SysUserModel>().Where(x => x.UserName == sysUserModel.UserName && x.UserState == 1).ToListAsync();

                if (list.Count > 0)
                {
                    throw new Exception($"用户名[{sysUserModel.UserName}]已经存在");
                }

                sysUserModel.CreateTime = DateTime.Now;

                sysUserModel.UserState = 1;

                sysUserModel.Password = GetMD5Str(GetMD5Str("123456") + "|" + sysUserModel.UserName);

                await _sqlSugarClient.Insertable(sysUserModel).ExecuteCommandAsync();
            }
            else
            {
                var list = await _sqlSugarClient.Queryable<SysUserModel>().Where(x => x.Id == sysUserModel.Id && x.UserState == 1).ToListAsync();

                if (list.Count == 0) return;

                list[0].CreateTime = DateTime.Now;

                list[0].RealName = sysUserModel.RealName;

                list[0].UserAge = sysUserModel.UserAge;

                list[0].UserState = sysUserModel.UserState;

                await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();
            }
        }

        public async Task ResetPwd(int userId)
        {
            var list = await _sqlSugarClient.Queryable<SysUserModel>().Where(x => x.Id == userId && x.UserState == 1).ToListAsync();

            if (list.Count == 0) return;

            list[0].CreateTime = DateTime.Now;

            list[0].Password = GetMD5Str(GetMD5Str("123456") + "|" + list[0].UserName);

            await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();
        }

        public async Task UpdateUserIcon(string userName, string userIcon)
        {
            var list = await _sqlSugarClient.Queryable<SysUserModel>().Where(x => x.UserName == userName && x.UserState == 1).ToListAsync();

            if (list.Count == 0) return;

            list[0].UserIcon = userIcon;

            list[0].CreateTime = DateTime.Now;

            await _sqlSugarClient.Updateable(list[0]).ExecuteCommandAsync();
        }
    }
}
