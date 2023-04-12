using SqlSugar;
using System;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("auto_register")]
    public class AutoRegisterModel
    {
        [SugarColumn(ColumnName = "auto_id", IsPrimaryKey = true)]
        public int AutoId { get; set; }

        [SugarColumn(ColumnName = "auto_license")]
        public string AutoLicense { get; set; }

        [SugarColumn(ColumnName = "license_color_id")]
        public int LicenseColorId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string LicenseColorName { get; set; }

        [SugarColumn(ColumnName = "auto_color_id")]
        public int AutoColorId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string AutoColorName { get; set; }

        [SugarColumn(ColumnName = "fee_mode_id")]
        public int FeeModeId { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string FeeModeName { get; set; }

        [SugarColumn(ColumnName = "description")]
        public string Description { get; set; }

        [SugarColumn(ColumnName = "state")]
        public int State { get; set; }

        [SugarColumn(ColumnName = "valid_count")]
        public int ValidCount { get; set; }

        [SugarColumn(ColumnName = "valid_end_time")]
        public DateTime? ValidEndTime { get; set; }

        [SugarColumn(ColumnName = "valid_start_time")]
        public DateTime? ValidStartTime { get; set; }
    }
}
