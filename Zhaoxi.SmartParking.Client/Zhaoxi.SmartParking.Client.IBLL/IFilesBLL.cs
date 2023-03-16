using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface IFilesBLL
    {
        public Task<List<UpgradeFileEntity>> List();

        public Task<bool> Delete(string fileName);

        public Task<List<UpgradeFileEntity>> LocalList();

        public void UploadFile(string file, string filePath, Action<int> progressChanged, Action completed);
    }
}
