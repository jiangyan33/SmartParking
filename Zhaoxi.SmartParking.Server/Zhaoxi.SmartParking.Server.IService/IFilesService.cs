using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface IFilesService
    {
        public Task<List<UpgradeFileModel>> List();

        public Task Save(UpgradeFileModel upgradeFileModel);

        public Task<bool> Delete(string fileName);
    }
}
