using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Zhaoxi.SmartParking.Client.Upgrade.Converters
{
    public class FileSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filesize = System.Convert.ToInt64(value);

            return GetFileSize(filesize);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        /// <summary>
        /// 格式化文件大小的C#方法
        /// </summary>
        /// <param name="filesize">文件的大小,传入的是一个bytes为单位的参数</param>
        /// <returns>格式化后的值</returns>
        private static string GetFileSize(long filesize)
        {
            if (filesize < 0)
            {
                return "0";
            }
            else if (filesize >= 1024 * 1024 * 1024) //文件大小大于或等于1024MB
            {
                return string.Format("{0:0.00} GB", (double)filesize / (1024 * 1024 * 1024));
            }
            else if (filesize >= 1024 * 1024) //文件大小大于或等于1024KB
            {
                return string.Format("{0:0.00} MB", (double)filesize / (1024 * 1024));
            }
            else if (filesize >= 1024) //文件大小大于等于1024bytes
            {
                return string.Format("{0:0.00} KB", (double)filesize / 1024);
            }
            else
            {
                return string.Format("{0:0.00} bytes", filesize);
            }
        }
    }
}