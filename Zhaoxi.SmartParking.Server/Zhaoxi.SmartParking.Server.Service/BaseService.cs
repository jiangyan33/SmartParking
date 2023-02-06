using SqlSugar;
using Zhaoxi.SmartParking.Server.Core;

namespace Zhaoxi.SmartParking.Server.Service
{
    public class BaseService
    {
        protected readonly SqlSugarClient _sqlSugarClient;

        public BaseService(SqlSugarContext sqlSugarContext)
        {
            _sqlSugarClient = sqlSugarContext.GetInstance;
        }
    }
}
