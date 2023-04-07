using SqlSugar;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("role_menu")]
    public class RoleMenuModel
    {
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true)]
        public int RoleId { get; set; }

        [SugarColumn(ColumnName = "menu_id", IsPrimaryKey = true)]
        public int MenuId { get; set; }
    }
}
