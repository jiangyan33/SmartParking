using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.MainModule.Models;

namespace Zhaoxi.SmartParking.Client.MainModule.ViewModels
{
    public class DashboardViewModel : PageViewModelBase
    {
        public string CurrentArea { get; set; }

        public DateTime CurrentDate { get; set; }

        public ObservableCollection<AreaModel> AreaList { get; set; } = new ObservableCollection<AreaModel>();

        public ObservableCollection<BoardModel> BoardList { get; set; } = new ObservableCollection<BoardModel>();

        public ObservableCollection<PassRecordItemModel> InfoList { get; set; } = new ObservableCollection<PassRecordItemModel>();

        public LiveCharts.SeriesCollection Series { get; set; } = new LiveCharts.SeriesCollection();

        private readonly ISysUserBLL _sysUserBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public DashboardViewModel(ISysUserBLL sysUserBLL, IDialogService dialogService)
        {
            PageTitle = "系统数据中心";
            IsCanClose = false;

            _sysUserBLL = sysUserBLL;

            _dialogService = dialogService;

            _dispatcher = Application.Current.Dispatcher;

            Refresh();
        }

        public override void Refresh()
        {
            _dispatcher.Invoke(() =>
            {
                AreaList.Clear();
                AreaList.Add(new AreaModel { AreaName = "湖北省" });
                AreaList.Add(new AreaModel { AreaName = "四川省" });

                BoardList.Clear();
                BoardList.Add(new BoardModel { Header = "总收入", Value = 568768 });
                BoardList.Add(new BoardModel { Header = "优惠卷(张)", Value = 24 });
                BoardList.Add(new BoardModel { Header = "会员累计人数", Value = 698 });
                BoardList.Add(new BoardModel { Header = "当前空车位", Value = 80 });
                BoardList.Add(new BoardModel { Header = "访客未处理(人)", Value = 5 });

                InfoList.Clear();
                InfoList.Add(new PassRecordItemModel() { PassDate = "2021-03-01 10:20:36", Passageway = "A1", TollCollector = "1024", LiftingState = "自动", Discount = 0, State = "正常" });
                InfoList.Add(new PassRecordItemModel() { PassDate = "2021-03-01 10:20:36", Passageway = "A1", TollCollector = "1024", LiftingState = "自动", Discount = 0, State = "正常" });
                InfoList.Add(new PassRecordItemModel() { PassDate = "2021-03-01 10:20:36", Passageway = "A1", TollCollector = "1024", LiftingState = "自动", Discount = 0, State = "正常" });
                InfoList.Add(new PassRecordItemModel() { PassDate = "2021-03-01 10:20:36", Passageway = "A1", TollCollector = "1024", LiftingState = "自动", Discount = 0, State = "正常" });
                InfoList.Add(new PassRecordItemModel() { PassDate = "2021-03-01 10:20:36", Passageway = "A1", TollCollector = "1024", LiftingState = "自动", Discount = 0, State = "正常" });
                InfoList.Add(new PassRecordItemModel() { PassDate = "2021-03-01 10:20:36", Passageway = "A1", TollCollector = "1024", LiftingState = "自动", Discount = 0, State = "正常" });

                Series.Clear();
                Series.Add(new PieSeries
                {
                    Title = "微信支付",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(16.0) }
                });
                Series.Add(new PieSeries
                {
                    Title = "支付宝支付",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(22.0) }
                });
                Series.Add(new PieSeries
                {
                    Title = "现金支付",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(11.0) }
                });
                Series.Add(new PieSeries
                {
                    Title = "优惠减免",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(39.0) }
                });
                Series.Add(new PieSeries
                {
                    Title = "会员",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(12.0) }
                });
            });
        }

        private void DialogResult(IDialogResult dialogResult)
        {
            if (dialogResult.Result != ButtonResult.OK) return;

            MessageBox.Show("操作完成", "提示");

            Refresh();
        }
    }
}
