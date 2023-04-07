using System.Collections.Generic;

namespace Zhaoxi.SmartParking.Client.Entity
{
    public class RoleEntity
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public int State { get; set; }

        public List<int> UserIdList { get; set; }

        public List<int> MenuIdList { get; set; }
    }
}
