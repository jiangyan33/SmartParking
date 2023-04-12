using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class BaseInfoDAL : IBaseInfoDAL
    {
        public async Task<string> GetAllAutoColor()
        {
            return await WebDataAccess.PostDatas("/baseInfo/allAutoColor", new { });
        }

        public async Task<string> GetAllLicenseColor()
        {
            return await WebDataAccess.PostDatas("/baseInfo/allLicenseColor", new { });
        }

        public async Task<string> GetAllFeeModel()
        {
            return await WebDataAccess.PostDatas("/baseInfo/allFeeModel", new { });
        }

        public Task<string> SaveAutoColor(object obj)
        {
            return WebDataAccess.PostDatas("/baseInfo/saveAutoColor", obj);
        }

        public Task<string> SaveLicenseColor(object obj)
        {
            return WebDataAccess.PostDatas("/baseInfo/saveLicenseColor", obj);
        }

        public Task<string> SaveFeeModel(object obj)
        {
            return WebDataAccess.PostDatas("/baseInfo/saveFeeModel", obj);
        }
    }
}
