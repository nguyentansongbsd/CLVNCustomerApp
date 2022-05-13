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
                return "#F2F2F2"; 
            }
            else
            {
                return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
