using System;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class SysUserDAL : ISysUserDAL
    {
        public Task<string> Login(string userName, string password)
        {
            return WebDataAccess.PostDatas("/users/login", new { userName, password });
        }
    }
}
