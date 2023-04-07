using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Server.Models;

namespace Zhaoxi.SmartParking.Server.IService
{
    public interface IRoleService
    {
        public Task<List<RoleModel>> GetAllMenus();

        public Task Save(RoleModel menuModel);

        public Task SaveRelation(RoleModel roleModel);
    }
}
