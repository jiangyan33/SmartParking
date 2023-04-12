using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Zhaoxi.SmartParking.Client.Controls
{
    /// <summary>
    /// Pagination.xaml 的交互逻辑
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
        }
    }

    public class PaginationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<int> PageSizeList { get; set; } = new List<int> { 10, 20, 30, 50, 100 };

        public ICommand NavCommand { get; set; }

        public ICommand PageSizeChangeCommand { get; set; }

        public ObservableCollection<PageNumberItemModel> PageNumbers { get; set; } = new ObservableCollection<PageNumberItemModel>();


        private bool _isCanPrevious;

        public bool IsCanPrevious
        {
            get => _isCanPrevious;
            set
            {
                _isCanPrevious = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCanPrevious)));
            }
        }

        private bool _isCanNext;

        public bool IsCanNext
        {
            get => _isCanNext;
            set
            {
                _isCanNext = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCanNext)));
            }
        }

        private int _pageSize = 20;

        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PageSize)));
            }
        }

        private int _previousIndex;

        public int PreviousIndex
        {
            get => _previousIndex;
            set
            {
                _previousIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PreviousIndex)));
            }
        }

        private int _nextIndex;

        public int NextIndex
        {
            get => _nextIndex;
            set
            {
                _nextIndex = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextIndex)));
            }
        }

        public void FillPages(int rows, int currentPage)
        {
            IsCanNext = true;

            IsCanPrevious = true;

            PreviousIndex = currentPage - 1;

            NextIndex = currentPage + 1;

            var pages = (int)Math.Ceiling(rows * 1.0 / PageSize);

            // 如果当前页大于总页数，当前页面=1
            if (currentPage > pages) currentPage = 1;

            if (currentPage == 1)
            {
                IsCanPrevious = false;
            }

            if (currentPage == pages)
            {
                IsCanNext = false;
            }

            var temp = new List<string>();

            //[1]2 3 4 5 6 7 8 9 ... 15
            // 1 2 3 4 [5] 6 7 8 9 ... 15
            // 1 ... 3 4 5 [6] 7 8 9 ... 15
            // 1 ... 7 8 9 [10] 11 12 13 ... 15
            // 1 ... 8 9 10 [11] 12 13 14 15      10条    50条
            // 省略首页和尾页

            var min = currentPage - 4;

            min = min <= 1 ? 1 : currentPage - 3;

            var max = currentPage + 4;

            if (currentPage <= 5)
            {
                max = Math.Min(9, pages);
            }
            else
            {
                max = max >= pages ? pages : currentPage + 3;
            }

            if (currentPage > pages - 4)
            {
                min = Math.Max(1, pages - 8);
            }

            if (min > 1)
            {
                temp.Add("1");

                temp.Add("...");
            }

            for (var i = min; i <= max; i++)
            {
                temp.Add(i.ToString());
            }

            if (max < pages)
            {
                temp.Add("...");
                temp.Add(pages.ToString());
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                int index = 0;

                PageNumbers.Clear();

                foreach (var item in temp)
                {
                    var state = int.TryParse(item, out index);

                    PageNumbers.Add(new PageNumberItemModel
                    {
                        Index = state ? index.ToString() : item,
                        IsCurrent = index == currentPage,
                        IsEnable = state
                    });
                }
            });

        }
    }

    public class PageNumberItemModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Index { get; set; }

        public bool IsEnable { get; set; } = true;

        private bool _isCurrent;

        public bool IsCurrent
        {
            get => _isCurrent;
            set
            {
                _isCurrent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCurrent)));
            }
        }
    }
}
