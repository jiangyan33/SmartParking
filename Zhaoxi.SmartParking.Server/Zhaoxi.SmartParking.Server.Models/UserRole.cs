using SqlSugar;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("user_role")]
    public class UserRoleModel
    {
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true)]
        public int UserId { get; set; }

        [SugarColumn(ColumnName = "role_id", IsPrimaryKey = true)]
        public int RoleId { get; set; }
    }
}
