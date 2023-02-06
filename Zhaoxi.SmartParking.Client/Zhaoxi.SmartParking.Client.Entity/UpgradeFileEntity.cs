using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.Entity
{
    public class UpgradeFileEntity
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileMD5 { get; set; }

        public string FilePath { get; set; }

        public int Length { get; set; }

        public int State { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
