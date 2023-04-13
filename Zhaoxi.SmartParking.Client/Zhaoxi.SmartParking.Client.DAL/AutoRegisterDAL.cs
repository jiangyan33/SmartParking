using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class AutoRegisterDAL : IAutoRegisterDAL
    {
        public Task<string> Pages(int pageNo, int pageSize, string search)
        {
            return WebDataAccess.PostDatas("/autoregister/pages", new { pageNo, pageSize, search });
        }

        public Task<string> Save(object obj)
        {
            return WebDataAccess.PostDatas("/autoregister/save", obj);
        }
    }
}
