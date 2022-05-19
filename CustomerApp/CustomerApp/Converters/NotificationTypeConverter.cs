using System;
using System.Globalization;
using CustomerApp.Models;
using Xamarin.Forms;

namespace CustomerApp.Converters
{
	public class NotificationTypeConverter : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            NotificationType type = (NotificationType)value;

            if (type == NotificationType.ConstructionProgess)
            {
                return "ConstructionProgess.png";
            }
            else if (type == NotificationType.HandOver)
            {
                return "newProject.png";
            }
            else if (type == NotificationType.NewPhaseLaunch)
            {
                return "newProject.png";
            }
            else if (type == NotificationType.NewProject)
            {
                return "newProject.png";
            }
            else if (type == NotificationType.PaymentReminder)
            {
                return "newProject.png";
            }
            else if (type == NotificationType.PaymentSuccess)
            {
                return "paymentSuccess.png";
            }
            else //QueueSuccess
            {
                return "newProject.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

