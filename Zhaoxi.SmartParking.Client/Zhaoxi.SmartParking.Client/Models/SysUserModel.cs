using Prism.Mvvm;

namespace Zhaoxi.SmartParking.Client.Models
{
    public class SysUserModel : BindableBase
    {

        private string _userName = "admin";

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private string _password = "123456";

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

    }
}
