using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.AutoModule.Models;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.AutoModule.ViewModels
{
    public class AddAutoRegisterDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        private AutoRegisterModel _mainModel;

        public AutoRegisterModel MainModel
        {
            get { return _mainModel; }
            set { SetProperty(ref _mainModel, value); }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            MainModel = parameters.GetValue<AutoRegisterModel>("model");

            var autoColorList = await _baseInfoBLL.GetAllAutoColor();

            var licenseColorList = await _baseInfoBLL.GetAllLicenseColor();

            var feeModelList = await _baseInfoBLL.GetAllFeeModel();

            _dispatcher.Invoke(() =>
            {
                AutoColorList.AddRange(autoColorList.Select(x => new DictModel { Key = x.ColorName, Value = x.ColorId }));
                LicenseColorList.AddRange(licenseColorList.Select(x => new DictModel { Key = x.ColorName, Value = x.ColorId }));
                FeeModelList.AddRange(feeModelList.Select(x => new DictModel { Key = x.FeeModelName, Value = x.FeeModelId }));
            });

            if (MainModel.AutoId == 0)
            {
                Title = "添加车辆信息";
            }
            else
            {
                Title = $"编辑车辆信息";

                //CurrentAutoColor.Key = MainModel.AutoColorName;

                //CurrentAutoColor.Value = MainModel.AutoColorId;

                //CurrentLicenseColor.Key = MainModel.LicenseColorName;

                //CurrentLicenseColor.Value = MainModel.LicenseColorId;

                //CurrentFeeModel.Key = MainModel.FeeModeName;

                //CurrentFeeModel.Value = MainModel.FeeModeId;
            }
        }

        public ObservableCollection<DictModel> LicenseColorList { get; set; } = new ObservableCollection<Models.DictModel>();

        public ObservableCollection<DictModel> AutoColorList { get; set; } = new ObservableCollection<Models.DictModel>();

        public ObservableCollection<DictModel> FeeModelList { get; set; } = new ObservableCollection<Models.DictModel>();


        //private DictModel _currentLicenseColor = new DictModel();

        //public DictModel CurrentLicenseColor
        //{
        //    get { return _currentLicenseColor; }
        //    set { SetProperty(ref _currentLicenseColor, value); }
        //}

        //private DictModel _currentAutoColor = new DictModel();

        //public DictModel CurrentAutoColor
        //{
        //    get { return _currentAutoColor; }
        //    set { SetProperty(ref _currentAutoColor, value); }
        //}

        //private DictModel _currentFeeModel = new DictModel();

        //public DictModel CurrentFeeModel
        //{
        //    get { return _currentFeeModel; }
        //    set { SetProperty(ref _currentFeeModel, value); }
        //}

        private readonly Dispatcher _dispatcher;

        private readonly IBaseInfoBLL _baseInfoBLL;

        private readonly IAutoRegisterBLL _autoRegisterBLL;

        public AddAutoRegisterDialogViewModel(IBaseInfoBLL baseInfoBLL, IAutoRegisterBLL autoRegisterBLL)
        {
            _dispatcher = Application.Current.Dispatcher;

            _baseInfoBLL = baseInfoBLL;

            _autoRegisterBLL = autoRegisterBLL;
        }

        public ICommand CancelCommand => new DelegateCommand(Cancel);

        private void Cancel()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        public ICommand ConfirmCommand => new DelegateCommand(Confirm);

        private async void Confirm()
        {
            try
            {
                var autoRegisterEntity = new AutoRegisterEntity
                {
                    AutoId = MainModel.AutoId,
                    AutoLicense = MainModel.AutoLicense,
                    LicenseColorId = MainModel.LicenseColorId,
                    AutoColorId = MainModel.AutoColorId,
                    FeeModeId = MainModel.FeeModeId,
                    Description = MainModel.Description,
                    State = MainModel.State,
                    ValidCount = MainModel.ValidCount,
                    ValidEndTime = MainModel.ValidEndTime,
                };

                await _autoRegisterBLL.Save(autoRegisterEntity);

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
