using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.Controls;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;
using Zhaoxi.SmartParking.Client.ReportModule.Models;

namespace Zhaoxi.SmartParking.Client.ReportModule.ViewModels
{
    public class ReportViewModel : PageViewModelBase
    {
        public PaginationModel PaginationModel { get; set; } = new PaginationModel();

        private int _currentPage = 1;

        public ObservableCollection<ReportItemModel> ReportItemModelModelList { get; set; } = new ObservableCollection<ReportItemModel>();

        private readonly IAutoRegisterBLL _autoRegisterBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public ReportViewModel(IAutoRegisterBLL autoRegisterBLL, IDialogService dialogService)
        {
            PageTitle = "车辆登记管理";

            AddButtonText = "新增";

            _autoRegisterBLL = autoRegisterBLL;

            _dialogService = dialogService;

            _dispatcher = Application.Current.Dispatcher;

            PaginationModel.NavCommand = new DelegateCommand<object>((obj) =>
            {
                _currentPage = int.Parse(obj.ToString());

                Refresh();
            });

            PaginationModel.PageSizeChangeCommand = new DelegateCommand(() =>
            {
                Refresh();
            });

            Refresh();
        }

        public override void Refresh()
        {
            var tempList = new List<ReportItemModel>();

            for (int i = 0; i < 30; i++)
            {
                tempList.Add(new ReportItemModel
                {
                    Index = i + 1,
                    Date = new DateTime(2022, 02, 02),
                    TotalCount = 100,
                    ReceAmount = 100.04,
                    CashPayment = 50.01,
                    ElecPayment = 50.01,
                    Subtotal = 100.02,
                    DeduAmount = 0.02
                });
            }

            _dispatcher.Invoke(() =>
            {

                if (ReportItemModelModelList.Count > 0) ReportItemModelModelList.Clear();

                ReportItemModelModelList.AddRange(tempList);
            });

            Task.Run(async () =>
            {


                //var pageResult = await _autoRegisterBLL.Pages(_currentPage, PaginationModel.PageSize, SearchValue);

                //var tempList = new List<ReportItemModel>();

                //for (var i = 0; i < pageResult.Data.Count; i++)
                //{
                //    tempList.Add(new Models.ReportItemModel
                //    {
                //        Index = i + 1,
                //        AutoId = pageResult.Data[i].AutoId,
                //        AutoLicense = pageResult.Data[i].AutoLicense,
                //        LicenseColorId = pageResult.Data[i].LicenseColorId,
                //        LicenseColorName = pageResult.Data[i].LicenseColorName,
                //        AutoColorId = pageResult.Data[i].AutoColorId,
                //        AutoColorName = pageResult.Data[i].AutoColorName,
                //        FeeModeId = pageResult.Data[i].FeeModeId,
                //        FeeModeName = pageResult.Data[i].FeeModeName,
                //        Description = pageResult.Data[i].Description,
                //        State = pageResult.Data[i].State,
                //        ValidCount = pageResult.Data[i].ValidCount,
                //        ValidEndTime = pageResult.Data[i].ValidEndTime,
                //        ValidStartTime = pageResult.Data[i].ValidStartTime,
                //    });

                //}

                _dispatcher.Invoke(() =>
                {
                    //PaginationModel.FillPages(pageResult.Rows, _currentPage);

                    //if (AutoRegisterModelList.Count > 0) AutoRegisterModelList.Clear();

                    //AutoRegisterModelList.AddRange(tempList);
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
