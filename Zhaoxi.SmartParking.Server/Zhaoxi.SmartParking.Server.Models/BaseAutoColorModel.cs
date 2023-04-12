using SqlSugar;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("base_auto_color")]
    public class BaseAutoColorModel
    {
        [SugarColumn(ColumnName = "color_id", IsPrimaryKey = true)]
        public int ColorId { get; set; }

        [SugarColumn(ColumnName = "color_name")]
        public string ColorName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public int State { get; set; }
    }
}
