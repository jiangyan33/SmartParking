using System;
using System.Collections.Generic;

namespace Zhaoxi.SmartParking.Client.Entity
{
    public class SysUserEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string UserIcon { get; set; }

        public string RealName { get; set; }

        public int UserAge { get; set; }

        public int UserState { get; set; }

        public DateTime CreateTime { get; set; }

        public string Token { get; set; }

        public List<MenuEntity> Menus { get; set; } = new List<MenuEntity>();

        public List<RoleEntity> Roles { get; set; } = new List<RoleEntity>();
    }
}
