using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.IDAL
{
    public interface IRolesDAL
    {
        public Task<string> GetAll();

        public Task<string> Save(object obj);

        public Task<string> SaveRelation(object obj);
    }
}
