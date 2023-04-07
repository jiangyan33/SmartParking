using SqlSugar;
using System;
using System.Collections.Generic;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("sys_user")]
    public class SysUserModel
    {
        [SugarColumn(ColumnName = "user_id", IsPrimaryKey = true, IsIdentity = true)]
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
        public int UserAge { get; set; }

        [SugarColumn(ColumnName = "user_state")]
        public int UserState { get; set; }

        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string Token { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<MenuModel> Menus { get; set; } = new List<MenuModel>();

        [SugarColumn(IsIgnore = true)]
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }
}
