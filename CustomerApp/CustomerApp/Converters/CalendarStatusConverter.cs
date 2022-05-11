using System;
using System.Globalization;
using CustomerApp.Datas;
using CustomerApp.Models;
using Xamarin.Forms;

namespace CustomerApp.Converters
{
    public class CalendarStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string Status = string.Empty;
                CalendarModel model = value as CalendarModel;
                if (model.Type == CalendarType.Meeting)
                {
                    Status = StatusCodeActivityData.GetStatusCodeById(model.State).Name;
                    return Status;
                }
                else if (model.Type == CalendarType.Booking)
                {
                    Status = Data.GetQueuesById(model.State).Name;
                    return Status;
                }
                else if (model.Type == CalendarType.Deposit)
                {
                    Status = Data.GetQuoteStatusCodeById(model.State).Name;
                    return Status;
                }
                else if (model.Type == CalendarType.Installment)
                {
                    Status = InstallmentsStatusCodeData.GetInstallmentsStatusCodeById(model.State).Name;
                    return Status;
                }
                else if (model.Type == CalendarType.Contract)
                {
                    Status = Data.GetContractStatusCodeById(model.State).Name;
                    return Status;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
