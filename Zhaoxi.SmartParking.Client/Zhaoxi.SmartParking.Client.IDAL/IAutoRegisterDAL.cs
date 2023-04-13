using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface IAutoRegisterDAL
    {
        public Task<string> Pages(int pageNo, int pageSize, string search);

        public Task<string> Save(object obj);

    }
}
