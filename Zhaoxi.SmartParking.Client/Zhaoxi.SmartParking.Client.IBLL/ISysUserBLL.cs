using System;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface ISysUserBLL
    {

        public Task<SysUserEntity> Login(string userName, string password);
    }
}
