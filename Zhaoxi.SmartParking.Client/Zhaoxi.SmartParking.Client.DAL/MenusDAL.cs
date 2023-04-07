using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class MenusDAL : IMenusDAL
    {
        public async Task<string> GetMenu(int id)
        {
            return await WebDataAccess.PostDatas("/menus/all", new { });
        }

        public Task<string> GetMenus(int roleId)
        {
            return WebDataAccess.PostDatas($"/menus/menus/{roleId}", new { });
        }

        public Task<string> Save(object obj)
        {
            return WebDataAccess.PostDatas("/menus/save", obj);
        }
    }
}
