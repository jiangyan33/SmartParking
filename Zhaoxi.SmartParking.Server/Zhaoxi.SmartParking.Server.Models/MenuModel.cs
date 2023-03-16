using SqlSugar;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("menus")]
    public class MenuModel
    {
        [SugarColumn(ColumnName = "menu_id", IsPrimaryKey = true)]
        public int MenuId { get; set; }

        [SugarColumn(ColumnName = "menu_header")]
        public string MenuHeader { get; set; }

        [SugarColumn(ColumnName = "target_view")]
        public string TargetView { get; set; }

        [SugarColumn(ColumnName = "parent_id")]
        public int ParentId { get; set; }

        [SugarColumn(ColumnName = "menu_icon")]
        public string MenuIcon { get; set; }

        [SugarColumn(ColumnName = "index")]
        public int Index { get; set; }

        [SugarColumn(ColumnName = "menu_type")]
        public int MenuType { get; set; }

        [SugarColumn(ColumnName = "state")]
        public int State { get; set; }
    }
}
