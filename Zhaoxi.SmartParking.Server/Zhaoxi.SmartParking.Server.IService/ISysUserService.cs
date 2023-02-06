using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface ISysUserService
    {
        public Task<SysUserModel> Login(string userName, string password);
    }
}
