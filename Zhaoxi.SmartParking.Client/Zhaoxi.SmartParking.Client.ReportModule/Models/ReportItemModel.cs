using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.ReportModule.Models
{
    public class ReportItemModel
    {
        public int Index { get; set; }

        public DateTime Date { get; set; }

        public int TotalCount { get; set; }

        public double ReceAmount { get; set; }

        public double CashPayment { get; set; }

        public double ElecPayment { get; set; }

        public double Subtotal { get; set; }

        public double DeduAmount { get; set; }
    }
}
