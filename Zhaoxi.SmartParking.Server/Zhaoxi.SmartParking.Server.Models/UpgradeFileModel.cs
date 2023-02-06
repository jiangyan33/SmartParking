using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("sys_user")]
    public class UpgradeFileModel
    {
        [SugarColumn(ColumnName = "file_id")]
        public int Id { get; set; }

        [SugarColumn(ColumnName = "file_name")]
        public string FileName { get; set; }

        [SugarColumn(ColumnName = "file_md5")]
        public string FileMD5 { get; set; }

        [SugarColumn(ColumnName = "file_path")]
        public string FilePath { get; set; }

        [SugarColumn(ColumnName = "length")]
        public int Length { get; set; }

        [SugarColumn(ColumnName = "state")]
        public int State { get; set; }

        [SugarColumn(ColumnName = "create_time")]
        public DateTime CreateTime { get; set; }

        [SugarColumn(ColumnName = "update_time")]
        public DateTime UpdateTime { get; set; }
    }
}
