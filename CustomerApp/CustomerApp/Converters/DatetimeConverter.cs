using System;
using System.Globalization;
using CustomerApp.IServices;
using Xamarin.Forms;

namespace CustomerApp.Converters
{
    public class DatetimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            DateTime date = (DateTime)value;
            var timeago = DependencyService.Get<IDatetimeService>().TimeAgo(date);
            return timeago;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
