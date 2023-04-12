using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface IBaseInfoBLL
    {
        public Task<List<BaseAutoColorEntity>> GetAllAutoColor();
        public Task<List<BaseLicenseColorEntity>> GetAllLicenseColor();
        public Task<List<BaseFeeEntity>> GetAllFeeModel();

        public Task<bool> SaveAutoColor(BaseAutoColorEntity baseAutoColorEntity);

        public Task<bool> SaveLicenseColor(BaseLicenseColorEntity baseLicenseColorEntity);

        public Task<bool> SaveFeeModel(BaseFeeEntity baseFeeEntity);
    }
}
