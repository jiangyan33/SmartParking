using SqlSugar;
using System.Collections.Generic;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("roles")]
    public class RoleModel
    {
        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true, IsIdentity = true)]
        public int RoleId { get; set; }

        [SugarColumn(ColumnName = "role_name")]
        public string RoleName { get; set; }

        [SugarColumn(ColumnName = "state")]
        public int State { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<int> UserIdList { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<int> MenuIdList { get; set; }

    }
}
