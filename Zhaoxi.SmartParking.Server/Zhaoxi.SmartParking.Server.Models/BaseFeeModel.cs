using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Server.Models
{
    [SugarTable("base_fee_model")]
    public class BaseFeeModel
    {
        [SugarColumn(ColumnName = "fee_model_id", IsPrimaryKey = true)]
        public int FeeModelId { get; set; }

        [SugarColumn(ColumnName = "fee_model_name")]
        public string FeeModelName { get; set; }

        [SugarColumn(IsIgnore = true)]
        public int State { get; set; }
    }
}