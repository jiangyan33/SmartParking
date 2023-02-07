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
    }
}
