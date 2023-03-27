using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace Zhaoxi.SmartParking.Client.Common
{
    public class PageViewModelBase : BindableBase
    {

        public PageViewModelBase()
        {
            AddCommand = new DelegateCommand(Add);

            RefreshCommand = new DelegateCommand(Refresh);
        }

        #region 基本属性

        private string _pageTitle = "";

        /// <summary>
        /// 页面标题
        /// </summary>
        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetProperty(ref _pageTitle, value); }
        }

        private string _searchValue = "";

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string SearchValue
        {
            get { return _searchValue; }
            set { SetProperty(ref _searchValue, value); }
        }

        /// <summary>
        /// 是否可以关闭
        /// </summary>
        public bool IsCanClose { get; set; } = true;

        /// <summary>
        /// 新增按钮文本
        /// </summary>
        public string AddButtonText { get; set; } = "新增";

        public ICommand RefreshCommand { get; set; }

        public ICommand AddCommand { get; set; }

        #endregion

        #region 基本方法



        public virtual void Add()
        {
        }

        public virtual void Refresh()
        {
        }

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            Refresh();
        }
        #endregion
    }
}
