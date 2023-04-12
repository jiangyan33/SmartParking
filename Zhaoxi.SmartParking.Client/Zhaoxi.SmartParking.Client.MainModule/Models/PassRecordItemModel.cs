using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.MainModule.Models
{
    public class PassRecordItemModel
    {
        //public ICommand RecordItemCommand { get; set; } = new DelegateCommand(() =>
        //{

        //});
        //public ICommand MenuItemCommand { get; set; }
        public string PassDate { get; set; }

        public string Passageway { get; set; }

        public string TollCollector { get; set; }

        public string LiftingState { get; set; }
        public double Discount { get; set; }
        public string State { get; set; }
    }
}
