using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.IDAL;

namespace Zhaoxi.SmartParking.Client.BLL
{
    public class MenusBLL : IMenusBLL
    {

        private readonly IMenusDAL _menusDAL;

        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MenusBLL(IMenusDAL menusDAL)
        {
            _menusDAL = menusDAL;

            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<bool> Save(MenuEntity menuEntity)
        {
            var str = await _menusDAL.Save(menuEntity);

            var result = JsonSerializer.Deserialize<ResultEntity<bool>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }
            return result.Data;
        }

        public async Task<List<MenuEntity>> GetMenus(int id)
        {
            var str = await _menusDAL.GetMenu(id);

            var result = JsonSerializer.Deserialize<ResultEntity<List<MenuEntity>>>(str, _jsonSerializerOptions);

            if (!result.IsSuccess)
            {
                throw new System.Exception(result.Message);
            }

            foreach (var item in result.Data)
            {
                if (!string.IsNullOrEmpty(item.MenuIcon))
                {
                    item.MenuIcon = ((char)int.Parse(item.MenuIcon, NumberStyles.HexNumber)).ToString();
                }
            }

            return result.Data;
        }

    }
}
