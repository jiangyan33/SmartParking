using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Windows;
using System.Windows.Input;
using Zhaoxi.SmartParking.Client.BaseModule.Models;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{

    /// <summary>
    /// 计费模式、车身颜色、车辆颜色编辑通用
    /// </summary>
    public class AddFeeModelDialogViewModel : BindableBase, IDialogAware
    {
        public string Title { get; set; }

        private string _operationType { get; set; }

        public event Action<IDialogResult> RequestClose;

        private DictModel _mainModel;

        public DictModel MainModel
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

        public void OnDialogOpened(IDialogParameters parameters)
        {
            MainModel = parameters.GetValue<DictModel>("model");

            _operationType = parameters.GetValue<string>("_operationType");

            if (MainModel.Value == 0)
            {
                Title = "添加" + _operationType;
            }
            else
            {
                Title = $"编辑" + _operationType;
            }
        }

        private readonly IBaseInfoBLL _baseInfoBLL;

        public AddFeeModelDialogViewModel(IBaseInfoBLL baseInfoBLL)
        {
            _baseInfoBLL = baseInfoBLL;
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
                if (_operationType == "计费模式")
                {
                    var baseFeeEntity = new BaseFeeEntity
                    {
                        FeeModelId = MainModel.Value,
                        FeeModelName = MainModel.Key,
                        State = 1
                    };

                    await _baseInfoBLL.SaveFeeModel(baseFeeEntity);
                }
                else if (_operationType == "车辆颜色")
                {
                    var baseAutoColorEntity = new BaseAutoColorEntity
                    {
                        ColorId = MainModel.Value,
                        ColorName = MainModel.Key,
                        State = 1
                    };

                    await _baseInfoBLL.SaveAutoColor(baseAutoColorEntity);
                }
                else if (_operationType == "车牌颜色")
                {
                    var baseLicenseColorEntity = new BaseLicenseColorEntity
                    {
                        ColorId = MainModel.Value,
                        ColorName = MainModel.Key,
                        State = 1
                    };

                    await _baseInfoBLL.SaveLicenseColor(baseLicenseColorEntity);
                }
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}