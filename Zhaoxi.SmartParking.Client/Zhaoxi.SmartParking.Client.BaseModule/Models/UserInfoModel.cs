﻿using Prism.Mvvm;
using System.Collections.ObjectModel;

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

        public bool IsSelected { get; set; }


        public ObservableCollection<RoleModel> Roles { get; set; } = new ObservableCollection<RoleModel>();
    }
}
