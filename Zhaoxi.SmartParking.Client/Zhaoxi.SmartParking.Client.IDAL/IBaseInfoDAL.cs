using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface IBaseInfoDAL
    {
        public Task<string> GetAllAutoColor();

        public Task<string> GetAllLicenseColor();

        public Task<string> GetAllFeeModel();

        public Task<string> SaveAutoColor(object obj);

        public Task<string> SaveLicenseColor(object obj);

        public Task<string> SaveFeeModel(object obj);

    }
}
