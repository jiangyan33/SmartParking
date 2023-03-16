using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Zhaoxi.SmartParking.Client.SystemModule.Models
{
    public class MenuItemModel : BindableBase
    {
        public string MenuIcon { get; set; }

        public string MenuHeader { get; set; }

        public string TargetView { get; set; }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        public ObservableCollection<MenuItemModel> Children { get; set; } = new ObservableCollection<MenuItemModel>();
    }
}
