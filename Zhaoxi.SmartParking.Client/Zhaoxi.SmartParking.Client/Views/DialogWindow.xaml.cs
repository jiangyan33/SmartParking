using Prism.Services.Dialogs;
using System.Windows;

namespace Zhaoxi.SmartParking.Client.Views
{
    /// <summary>
    /// DialogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DialogWindow : Window, IDialogWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public IDialogResult Result { get; set; }
    }
}
