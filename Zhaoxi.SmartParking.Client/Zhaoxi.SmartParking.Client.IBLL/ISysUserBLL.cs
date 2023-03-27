using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface ISysUserBLL
    {
        public Task<SysUserEntity> Login(string userName, string password);

        public Task<List<SysUserEntity>> All();

        public Task<bool> Save(SysUserEntity sysUserEntity);

        public Task<bool> ResetPwd(int userId);
    }
}
