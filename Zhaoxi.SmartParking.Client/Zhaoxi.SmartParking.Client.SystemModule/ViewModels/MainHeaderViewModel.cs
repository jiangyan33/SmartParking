using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Common;

namespace Zhaoxi.SmartParking.Client.SystemModule.ViewModels
{
    public class MainHeaderViewModel : BindableBase
    {

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _userIcon;

        public string UserIcon
        {
            get { return _userIcon; }
            set { SetProperty(ref _userIcon, value); }
        }

        public MainHeaderViewModel()
        {
            UserName = GlobalValue.UserInfo?.UserName;

            UserIcon = GlobalValue.UserInfo?.UserIcon;
        }
    }
}
