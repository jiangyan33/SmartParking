using Prism.Commands;
using Prism.Regions;
using System.Linq;
using System.Windows.Input;

namespace Zhaoxi.SmartParking.Client.ViewModels
{
    public class MainViewModel
    {

        private readonly IRegionManager _regionManager;


        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }


        public ICommand CloseCommand => new DelegateCommand<object>(Close);


        private void Close(object v)
        {
            var viewName = v.GetType().Name;

            var region = _regionManager.Regions["MainViewRegion"];

            var view = region.Views.FirstOrDefault(x => x.GetType().Name == viewName);

            if (view != null) region.Remove(view);
        }
    }
}
