using Prism.Events;
using System.Windows;
using Zhaoxi.SmartParking.Client.Common;

namespace Zhaoxi.SmartParking.Client.Views
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<CloseWindowEvent>().Subscribe(MessageReceived, ThreadOption.UIThread);
        }

        private void MessageReceived()
        {
            this.Close();
        }
    }
}
