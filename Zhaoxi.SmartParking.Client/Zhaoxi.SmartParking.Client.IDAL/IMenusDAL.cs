using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface IMenusDAL
    {
        public Task<string> GetMenu(int id);

        public Task<string> Save(object obj);
    }
}
