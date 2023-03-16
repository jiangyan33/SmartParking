using SqlSugar;
using System;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("sys_user")]
    public class SysUserModel
    {
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true)]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "user_name")]
        public string UserName { get; set; }

        [SugarColumn(ColumnName = "password")]
        public string Password { get; set; }

        [SugarColumn(ColumnName = "user_icon")]
        public string UserIcon { get; set; }

        [SugarColumn(ColumnName = "real_name")]
        public string RealName { get; set; }

        [SugarColumn(ColumnName = "user_age")]
        public string UserAge { get; set; }

        [SugarColumn(ColumnName = "user_state")]
        public int UserState { get; set; }

        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string Token { get; set; }
    }
}
