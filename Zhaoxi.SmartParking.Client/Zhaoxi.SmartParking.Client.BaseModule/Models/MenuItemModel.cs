using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Zhaoxi.SmartParking.Client.BaseModule.Models
{
    public class MenuItemModel : BindableBase
    {
        public int MenuId { get; set; }

        public int Index { get; set; }

        private int _parentId;

        public int Parent
        {
            get { return _parentId; }
            set { SetProperty(ref _parentId, value); }
        }

        private int _menuType;

        public int MenuType
        {
            get { return _menuType; }
            set { SetProperty(ref _menuType, value); }
        }

        private string _menuIcon;

        public string MenuIcon
        {
            get { return _menuIcon; }
            set { SetProperty(ref _menuIcon, value); }
        }

        private string _menuHeader;

        public string MenuHeader
        {
            get { return _menuHeader; }
            set { SetProperty(ref _menuHeader, value); }
        }

        private string _targetView;

        public string TargetView
        {
            get { return _targetView; }
            set { SetProperty(ref _targetView, value); }
        }

        public ObservableCollection<MenuItemModel> Children { get; set; } = new ObservableCollection<MenuItemModel>();

        private bool _isLastChild = false;

        public bool IsLastChild
        {
            get { return _isLastChild; }
            set { SetProperty(ref _isLastChild, value); }
        }

        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }
        }

        private int _overLocation = 0;
        public int OverLocation
        {
            get { return _overLocation; }
            set
            {
                SetProperty(ref _overLocation, value);
            }
        }

    }
}
