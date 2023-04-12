using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface IBaseInfoService
    {
        public Task<List<BaseAutoColorModel>> GetAllAutoColor();

        public Task<List<BaseLicenseColorModel>> GetAllLicenseColor();

        public Task<List<BaseFeeModel>> GetAllFeeModel();

        public Task SaveAutoColor(BaseAutoColorModel baseAutoColorModel);

        public Task SaveLicenseColor(BaseLicenseColorModel baseLicenseColorModel);

        public Task SavelFeeModel(BaseFeeModel baseFeeModel);
    }
}
