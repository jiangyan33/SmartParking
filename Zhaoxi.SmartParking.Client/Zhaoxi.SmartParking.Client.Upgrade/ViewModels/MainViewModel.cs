using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zhaoxi.SmartParking.Client.Upgrade.Models;

namespace Zhaoxi.SmartParking.Client.Upgrade.ViewModels
{
    public class MainViewModel
    {

        public ObservableCollection<FileModel> FileModelList { get; set; } = new ObservableCollection<FileModel>();

        public MainViewModel(List<FileModel> fileModels)
        {
            for (var i = 0; i < fileModels.Count; i++)
            {
                fileModels[i].Index = i + 1;

                fileModels[i].State = "待更新";

                FileModelList.Add(fileModels[i]);
            }
        }
    }
}
