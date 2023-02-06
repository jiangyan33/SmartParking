using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Effects;

namespace Zhaoxi.SmartParking.Client.Controls.Converters
{
    public class Bool2BlurConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var blurEffect = new BlurEffect
            {
                Radius = 0
            };

            var result = false;

            if (value != null && bool.TryParse(value.ToString(), out result))
            {
                if (result) blurEffect.Radius = 10;
            }

            return blurEffect;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
