using Prism.Mvvm;
using System.Collections.Generic;


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

        public List<MenuItemModel> Children { get; set; }
    }
}
