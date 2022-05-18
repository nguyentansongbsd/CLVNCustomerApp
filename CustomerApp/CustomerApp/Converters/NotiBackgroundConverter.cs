using System;
using System.Globalization;
using Xamarin.Forms;

namespace CustomerApp.Converters
{
    public class NotiBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isRead = (bool)value;
            if (isRead)
            {
                return Color.White; 
            }
            else
            {
                return "#E6F2FF";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
