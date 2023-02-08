using System.Windows;
using Zhaoxi.SmartParking.Client.Upgrade.ViewModels;

namespace Zhaoxi.SmartParking.Client.Upgrade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(App.FileModelList);
        }
    }
}
