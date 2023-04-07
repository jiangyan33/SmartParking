using System.Collections.Generic;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;

namespace Zhaoxi.SmartParking.Client.IBLL
{
    public interface IMenusBLL
    {
        public Task<List<MenuEntity>> GetAllMenus(int id);

        public Task<List<int>> GetMenus(int roleId);

        public Task<bool> Save(MenuEntity menuEntity);
    }
}
