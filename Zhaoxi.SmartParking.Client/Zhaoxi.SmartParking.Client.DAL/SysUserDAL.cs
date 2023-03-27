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

        public Task<string> All()
        {
            return WebDataAccess.PostDatas("/users/all", new { });
        }

        public Task<string> Save(object obj)
        {
            return WebDataAccess.PostDatas("/users/save", obj);
        }

        public Task<string> ResetPwd(int userId)
        {
            return WebDataAccess.PostDatas("/users/resetPwd/" + userId, new { });
        }
    }
}
