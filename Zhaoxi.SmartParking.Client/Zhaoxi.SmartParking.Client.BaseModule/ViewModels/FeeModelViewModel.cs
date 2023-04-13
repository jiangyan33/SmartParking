using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.BaseModule.Models;
using Zhaoxi.SmartParking.Client.BaseModule.Views;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.Entity;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class FeeModelViewModel : PageViewModelBase
    {
        public ObservableCollection<DictModel> DictModelList { get; set; } = new ObservableCollection<DictModel>();

        private readonly IBaseInfoBLL _baseInfoBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public FeeModelViewModel(IBaseInfoBLL baseInfoBLL, IDialogService dialogService)
        {
            PageTitle = "计费模式";

            AddButtonText = "新增";

            _baseInfoBLL = baseInfoBLL;

            _dialogService = dialogService;

            _dispatcher = Application.Current.Dispatcher;

            Refresh();
        }

        public ICommand DeleteCommand => new DelegateCommand<object>(Delete);

        private async void Delete(object obj)
        {
            var model = obj as DictModel;

            var ret = MessageBox.Show($"是否要删除该条记录[{model.Key}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (ret == MessageBoxResult.Cancel) return;

            try
            {
                var baseFeeEntity = new BaseFeeEntity
                {
                    FeeModelId = model.Value,
                    FeeModelName = model.Key,
                    State = 0
                };

                await _baseInfoBLL.SaveFeeModel(baseFeeEntity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误信息", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Refresh();
        }

        public ICommand EditCommand => new DelegateCommand<object>(Edit);

        private void Edit(object obj)
        {
            var parameters = new DialogParameters();

            parameters.Add("model", ModelHelper.CopyModel(obj as DictModel));

            parameters.Add("_operationType", "计费模式");

            _dialogService.ShowDialog(nameof(AddFeeModelDialogView), parameters, DialogResult);
        }

        public override void Add()
        {
            var parameters = new DialogParameters();

            parameters.Add("model", new DictModel());

            parameters.Add("_operationType", "计费模式");

            _dialogService.ShowDialog(nameof(AddFeeModelDialogView), parameters, DialogResult);
        }

        public override void Refresh()
        {
            Task.Run(async () =>
            {
                var baseModeList = await _baseInfoBLL.GetAllFeeModel();

                var tempList = new List<Models.DictModel>();

                for (var i = 0; i < baseModeList.Count; i++)
                {
                    if (!baseModeList[i].FeeModelName.Contains(SearchValue)) continue;

                    tempList.Add(new Models.DictModel
                    {
                        Index = i + 1,
                        Key = baseModeList[i].FeeModelName,
                        Value = baseModeList[i].FeeModelId,
                    });
                }

                _dispatcher.Invoke(() =>
                {
                    if (DictModelList.Count > 0) DictModelList.Clear();

                    DictModelList.AddRange(tempList);
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

