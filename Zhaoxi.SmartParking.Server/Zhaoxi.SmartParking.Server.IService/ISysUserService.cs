using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface ISysUserService
    {
        public Task<SysUserModel> Login(string userName, string password);

        public Task<List<SysUserModel>> All();

        public Task<List<SysUserModel>> GetUsers(int roleId);

        public Task UpdateUserIcon(string userName, string userIcon);

        public Task Save(SysUserModel sysUserModel);

        public Task ResetPwd(int userId);

        public Task SaveRole(SysUserModel sysUserModel);
    }
}
