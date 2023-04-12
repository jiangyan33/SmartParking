using System;

namespace Zhaoxi.SmartParking.Client.AutoModule.Models
{
    public class AutoRegisterModel
    {
        public int Index { get; set; }

        public int AutoId { get; set; }

        public string AutoLicense { get; set; }

        public int LicenseColorId { get; set; }

        public string LicenseColorName { get; set; }

        public int AutoColorId { get; set; }

        public string AutoColorName { get; set; }

        public int FeeModeId { get; set; }

        public string FeeModeName { get; set; }

        public string Description { get; set; }

        public int State { get; set; }

        public int ValidCount { get; set; }

        public DateTime? ValidEndTime { get; set; }

        public DateTime? ValidStartTime { get; set; }
    }
}
