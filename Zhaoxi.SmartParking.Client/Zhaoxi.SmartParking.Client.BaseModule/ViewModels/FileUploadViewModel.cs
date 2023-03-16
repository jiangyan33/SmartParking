using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Zhaoxi.SmartParking.Client.BaseModule.Views;
using Zhaoxi.SmartParking.Client.IBLL;

namespace Zhaoxi.SmartParking.Client.BaseModule.ViewModels
{
    public class FileUploadViewModel : BindableBase
    {

        public string PageTitle => "升级文件上传";

        public bool IsCanClose => true;

        public ObservableCollection<Models.FileInfoModel> Files { get; set; } = new ObservableCollection<Models.FileInfoModel>();


        private readonly IFilesBLL _filesBLL;

        private readonly Dispatcher _dispatcher;

        private readonly IDialogService _dialogService;

        public FileUploadViewModel(IFilesBLL filesBLL, IDialogService dialogService)
        {
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

        public ICommand AddCommand => new DelegateCommand(Add);

        private void Add()
        {
            _dialogService.ShowDialog(nameof(AddFileDialogView), (res) => Refresh());
        }

        public ICommand RefreshCommand => new DelegateCommand(Refresh);

        private void Refresh()
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


        private string _searchValue = "";

        public string SearchValue
        {
            get { return _searchValue; }
            set { SetProperty(ref _searchValue, value); }
        }

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            Refresh();
        }

    }
}
