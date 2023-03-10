using System;

namespace Zhaoxi.SmartParking.Client.Entity
{
    public class SysUserEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string UserIcon { get; set; }

        public string RealName { get; set; }

        public string UserAge { get; set; }

        public int UserState { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
