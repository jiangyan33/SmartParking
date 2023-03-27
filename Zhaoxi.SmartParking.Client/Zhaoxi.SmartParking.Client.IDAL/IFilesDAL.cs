using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface IFilesDAL
    {
        public Task<string> List();

        public Task<string> LocalList();

        public void UploadFile(string file, string updatePath, Action<int> progressChanged, Action completed);

        public void UploadIcon(string userName, string file);

        public Task<string> Delete(string fileName);
    }
}
