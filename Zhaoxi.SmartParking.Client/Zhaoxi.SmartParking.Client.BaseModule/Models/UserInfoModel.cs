using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zhaoxi.SmartParking.Client.BaseModule.Models
{
    public class UserInfoModel : BindableBase
    {
        public int Index { get; set; }

        public int UserId { get; set; }

        private string _userIcon;

        public string UserIcon
        {
            get { return _userIcon; }
            set { SetProperty(ref _userIcon, value); }
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public string RealName { get; set; }

        //public ObservableCollection<RoleModel> Roles { get; set; } = new ObservableCollection<RoleModel>();
    }
}
