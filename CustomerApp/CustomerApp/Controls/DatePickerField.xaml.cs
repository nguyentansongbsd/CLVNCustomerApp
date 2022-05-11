using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerField : Grid
    {
        // title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(DatePickerField), null, BindingMode.TwoWay);
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        // value
        public static readonly BindableProperty DateProperty = BindableProperty.Create(nameof(Date), typeof(DateTime), typeof(DatePickerField), null, BindingMode.TwoWay, propertyChanged: DateChange);
        public DateTime Date { get => (DateTime)GetValue(DateProperty); set => SetValue(DateProperty, value); }
        private static void DateChange(BindableObject bindable, object oldValue, object newValue)
        {
            DatePickerField control = (DatePickerField)bindable;
            if (newValue != null)
                control.datepicker.Date = (DateTime)newValue;
        }
        // Placeholder
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(DatePickerField), null, BindingMode.TwoWay, propertyChanged: PlaceholderChange);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        private static void PlaceholderChange(BindableObject bindable, object oldValue, object newValue)
        {
            DatePickerField control = (DatePickerField)bindable;
            if (newValue != null)
                control.datepicker.Placeholder = (string)newValue;
        }
        // Required
        public static readonly BindableProperty isRequiredProperty = BindableProperty.Create(nameof(isRequired), typeof(bool), typeof(DatePickerField), false, BindingMode.TwoWay);
        public bool isRequired { get => (bool)GetValue(isRequiredProperty); set => SetValue(isRequiredProperty, value); }
        public DatePickerField()
        {
            InitializeComponent();
            this.title.BindingContext = this;
            this.required.BindingContext = this;
        }
    }
}