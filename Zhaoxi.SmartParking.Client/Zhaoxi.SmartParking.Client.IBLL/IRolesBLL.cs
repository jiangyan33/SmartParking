using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface IRolesBLL
    {
        public Task<List<RoleEntity>> GetAll();

        public Task<bool> Save(RoleEntity roleEntity);

        public Task<bool> SaveRelation(RoleEntity roleEntity);
    }
}
