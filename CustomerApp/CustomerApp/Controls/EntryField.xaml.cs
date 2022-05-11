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
    public partial class EntryField : Grid
    {
        // title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(EntryField), null, BindingMode.TwoWay);
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        // value
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryField), null, BindingMode.TwoWay, propertyChanged: TextChange);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        private static void TextChange(BindableObject bindable, object oldValue, object newValue)
        {
            EntryField control = (EntryField)bindable;
            if (newValue != null)
                control.entry.Text = control.label.Text = (string)newValue;
        }
        // Placeholder
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(EntryField), null, BindingMode.TwoWay, propertyChanged: PlaceholderChange);
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }
        private static void PlaceholderChange(BindableObject bindable, object oldValue, object newValue)
        {
            EntryField control = (EntryField)bindable;
            if (newValue != null)
                control.entry.Placeholder = (string)newValue;
        }
        // Required
        public static readonly BindableProperty isRequiredProperty = BindableProperty.Create(nameof(isRequired), typeof(bool), typeof(EntryField), false, BindingMode.TwoWay);
        public bool isRequired { get => (bool)GetValue(isRequiredProperty); set => SetValue(isRequiredProperty, value); }
        // EnabledField
        public static readonly BindableProperty isEnabledFieldProperty = BindableProperty.Create(nameof(isEnabledField), typeof(bool), typeof(EntryField), null, BindingMode.TwoWay, propertyChanged: isEnabledFieldChange);
        public bool isEnabledField { get => (bool)GetValue(isEnabledFieldProperty); set => SetValue(isEnabledFieldProperty, value); }
        private static void isEnabledFieldChange(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue == null) return;
            EntryField control = (EntryField)bindable;
            if ((bool)newValue == true)
            {
                control.entry.IsEnabled = false;
                control.enable.IsVisible = true;
            }
            else
            {
                control.entry.IsEnabled = true;
                control.enable.IsVisible = false;
            }
        }
        // Keyboard
        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(bool), typeof(Keyboard), null, BindingMode.TwoWay, propertyChanged: KeyboardChange);
        public Keyboard Keyboard { get => (Keyboard)GetValue(KeyboardProperty); set => SetValue(KeyboardProperty, value); }
        private static void KeyboardChange(BindableObject bindable, object oldValue, object newValue)
        {
            EntryField control = (EntryField)bindable;
            if (newValue != null)
                control.entry.Keyboard = (Keyboard)newValue;
        }
        public EntryField()
        {
            InitializeComponent();
            this.title.BindingContext = this;
            this.required.BindingContext = this;
            this.label.BindingContext = this;
            // không binding với entry để nhận giá trị binding nhập vào
        }
    }
}