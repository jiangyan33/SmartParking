using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.AutoModule.Models;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.Controls;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.AutoModule.ViewModels
{
    public class AutoRegisterViewModel : PageViewModelBase
    {
        public PaginationModel PaginationModel { get; set; } = new PaginationModel();

        private int _currentPage = 1;

        public ObservableCollection<AutoRegisterModel> AutoRegisterModelList { get; set; } = new ObservableCollection<AutoRegisterModel>();

        private readonly IAutoRegisterBLL _autoRegisterBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public AutoRegisterViewModel(IAutoRegisterBLL autoRegisterBLL, IDialogService dialogService)
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

        public ICommand DeleteCommand => new DelegateCommand<object>(Delete);

        private async void Delete(object obj)
        {
            //var model = obj as UserInfoModel;

            //var ret = MessageBox.Show($"是否要删除该条记录[{model.RealName}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            //if (ret == MessageBoxResult.Cancel) return;

            //try
            //{
            //    var sysUserEntity = new SysUserEntity
            //    {
            //        UserName = model.UserName,
            //        RealName = model.RealName,
            //        UserAge = model.Age,
            //        UserIcon = model.UserIcon,
            //        Password = model.Password,
            //        Id = model.UserId,
            //        UserState = 0
            //    };

            //    await _sysUserBLL.Save(sysUserEntity);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //Refresh();
        }

        public ICommand EditCommand => new DelegateCommand<object>(Edit);

        private void Edit(object obj)
        {
            var parameters = new DialogParameters();

            parameters.Add("model", obj);

            //_dialogService.ShowDialog(nameof(AddUserDialogView), parameters, DialogResult);
        }


        public override void Add()
        {
            var parameters = new DialogParameters();

            //parameters.Add("model", new UserInfoModel());

            //_dialogService.ShowDialog(nameof(AddUserDialogView), parameters, DialogResult);
        }

        public override void Refresh()
        {
            Task.Run(async () =>
            {
                var pageResult = await _autoRegisterBLL.Pages(_currentPage, PaginationModel.PageSize, SearchValue);

                var tempList = new List<AutoRegisterModel>();

                for (var i = 0; i < pageResult.Data.Count; i++)
                {
                    tempList.Add(new Models.AutoRegisterModel
                    {
                        Index = i + 1,
                        AutoId = pageResult.Data[i].AutoId,
                        AutoLicense = pageResult.Data[i].AutoLicense,
                        LicenseColorId = pageResult.Data[i].LicenseColorId,
                        LicenseColorName = pageResult.Data[i].LicenseColorName,
                        AutoColorId = pageResult.Data[i].AutoColorId,
                        AutoColorName = pageResult.Data[i].AutoColorName,
                        FeeModeId = pageResult.Data[i].FeeModeId,
                        FeeModeName = pageResult.Data[i].FeeModeName,
                        Description = pageResult.Data[i].Description,
                        State = pageResult.Data[i].State,
                        ValidCount = pageResult.Data[i].ValidCount,
                        ValidEndTime = pageResult.Data[i].ValidEndTime,
                        ValidStartTime = pageResult.Data[i].ValidStartTime,
                    });

                }

                _dispatcher.Invoke(() =>
                {
                    PaginationModel.FillPages(pageResult.Rows, _currentPage);

                    if (AutoRegisterModelList.Count > 0) AutoRegisterModelList.Clear();

                    AutoRegisterModelList.AddRange(tempList);
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
