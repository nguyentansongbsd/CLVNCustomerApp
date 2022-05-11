using System;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace CustomerApp.Models
{
    public class CalendarModel : Appointment
    {
        public Guid Id { get; set; }
        public CalendarType Type { get; set; }
        public string State { get; set; }
        public Color BackgroundColor { get; set; }

        public DateTime? dateForGroupingWeek { get; set; }
    }
}
