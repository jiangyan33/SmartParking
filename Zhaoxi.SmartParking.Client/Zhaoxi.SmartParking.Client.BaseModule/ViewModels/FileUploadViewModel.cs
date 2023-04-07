using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.BaseModule.Views;
using Zhaoxi.SmartParking.Client.Common;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class FileUploadViewModel : PageViewModelBase
    {
        public ObservableCollection<Models.FileInfoModel> Files { get; set; } = new ObservableCollection<Models.FileInfoModel>();

        private readonly IFilesBLL _filesBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public FileUploadViewModel(IFilesBLL filesBLL, IDialogService dialogService)
        {
            PageTitle = "升级文件上传";

            AddButtonText = "上传";

            _filesBLL = filesBLL;

            _dialogService = dialogService;

            _dispatcher = Application.Current.Dispatcher;

            Refresh();
        }

        public ICommand DeleteCommand => new DelegateCommand<string>(Delete);

        private async void Delete(string fileName)
        {
            var ret = MessageBox.Show($"是否要删除该条记录[{fileName}]?", "删除提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (ret == MessageBoxResult.Cancel) return;

            await _filesBLL.Delete(fileName);

            Refresh();
        }

        public override void Add()
        {
            _dialogService.ShowDialog(nameof(AddFileDialogView), (res) => Refresh());
        }

        public override void Refresh()
        {
            Task.Run(async () =>
            {
                var files = await _filesBLL.List();

                var tempList = new List<Models.FileInfoModel>();

                for (var i = 0; i < files.Count; i++)
                {
                    if (!files[i].FileName.Contains(SearchValue)) continue;

                    tempList.Add(new Models.FileInfoModel
                    {
                        Index = i + 1,
                        FileName = files[i].FileName,
                        UpdatePath = files[i].UpdatePath,
                        UploadTime = files[i].UpdateTime,
                    });

                }

                _dispatcher.Invoke(() =>
                {
                    if (Files.Count > 0) Files.Clear();

                    Files.AddRange(tempList);
                });
            });
        }
    }
}
