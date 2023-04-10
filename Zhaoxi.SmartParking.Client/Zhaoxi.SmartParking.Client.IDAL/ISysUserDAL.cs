using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface ISysUserDAL
    {
        public Task<string> Login(string userName, string password);

        public Task<string> All();

        public Task<string> GetUsers(int roleId);

        public Task<string> Save(object obj);

        public Task<string> SaveRole(object obj);

        public Task<string> ResetPwd(int userId);
    }
}
