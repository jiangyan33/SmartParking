using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface IAutoRegisterBLL
    {
        public Task<PageResult<List<AutoRegisterEntity>>> Pages(int pageNo, int pageSize, string search);
    }
}
