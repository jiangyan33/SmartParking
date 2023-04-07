using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class RolesDAL : IRolesDAL
    {
        public async Task<string> GetAll()
        {
            return await WebDataAccess.PostDatas("/roles/all", new { });
        }

        public Task<string> Save(object obj)
        {
            return WebDataAccess.PostDatas("/roles/save", obj);
        }

        public Task<string> SaveRelation(object obj)
        {
            return WebDataAccess.PostDatas("/roles/saveRelation", obj);
        }
    }
}
