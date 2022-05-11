using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusBox : RadBorder
    {
        // text
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(StatusBox), null, BindingMode.TwoWay, propertyChanged: TextChange);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        private static void TextChange(BindableObject bindable, object oldValue, object newValue)
        {
            StatusBox control = (StatusBox)bindable;
            if (newValue != null)
                control.lb_text.Text = (string)newValue;
        }
        // color
        public static readonly BindableProperty ColorStatusProperty = BindableProperty.Create(nameof(ColorStatus), typeof(Color), typeof(StatusBox), null, BindingMode.TwoWay, propertyChanged: ColorStatusChange);
        public Color ColorStatus { get => (Color)GetValue(ColorStatusProperty); set => SetValue(ColorStatusProperty, value); }
        private static void ColorStatusChange(BindableObject bindable, object oldValue, object newValue)
        {
            StatusBox control = (StatusBox)bindable;
            if (newValue != null)
                control.BackgroundColor = (Color)newValue;
            else
                control.BackgroundColor = (Color)App.Current.Resources["TextColor"];
        }
        // icon
        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(StatusBox), null, BindingMode.TwoWay, propertyChanged: IconChange);
        public string Icon { get => (string)GetValue(IconProperty); set => SetValue(IconProperty, value); }
        private static void IconChange(BindableObject bindable, object oldValue, object newValue)
        {
            StatusBox control = (StatusBox)bindable;
            if (newValue != null)
                control.icon.IsVisible = true;
            else
                control.icon.IsVisible = false;
        }
        // font
        public static readonly BindableProperty FontProperty = BindableProperty.Create(nameof(Font), typeof(Fonts), typeof(StatusBox), Fonts.Solid , BindingMode.TwoWay, propertyChanged: FontChange);
        public Fonts Font { get => (Fonts)GetValue(FontProperty); set => SetValue(FontProperty, value); }
        private static void FontChange(BindableObject bindable, object oldValue, object newValue)
        {
            StatusBox control = (StatusBox)bindable;
            control.SetFont();
        }
        public StatusBox()
        {
            InitializeComponent();
        }
        private void SetFont()
        {
            if (Font == Fonts.Solid)
                icon.FontFamily = "FontAwesomeSolid";
            else if (Font == Fonts.Regular)
                icon.FontFamily = "FontAwesomeRegular";
        }
    }
    public enum Fonts
    {
        Regular,
        Solid
    }
}