using System;
using System.Globalization;
using System.IO;
using CustomerApp.Settings;
using Xamarin.Forms;

namespace CustomerApp.Converters
{
    public class Base64ToImageSourceConverter : IValueConverter
    {
        ImageSource image;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && value is string)
            {
                image = null;
                try
                {
                    byte[] bytes = System.Convert.FromBase64String(value.ToString());
                    image = ImageSource.FromStream(() => new MemoryStream(bytes));
                }
                catch(Exception ex)
                {
                    string name = value.ToString();
                    return $"https://ui-avatars.com/api/?background=2196F3&rounded=false&color=ffffff&size=150&length=2&name={name}";
                }
                
                return image;
            }
            else
            {
                string name = string.IsNullOrWhiteSpace(UserLogged.ContactName) ? UserLogged.User : UserLogged.ContactName;
                return $"https://ui-avatars.com/api/?background=2196F3&rounded=false&color=ffffff&size=150&length=2&name={name}";
            }
            
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
