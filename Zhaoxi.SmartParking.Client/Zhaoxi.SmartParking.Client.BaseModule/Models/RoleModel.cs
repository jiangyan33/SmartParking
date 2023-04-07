using Prism.Mvvm;

namespace Zhaoxi.SmartParking.Client.BaseModule.Models
{
    public class RoleModel : BindableBase
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { SetProperty(ref _isSelected, value); }
        }
    }
}
