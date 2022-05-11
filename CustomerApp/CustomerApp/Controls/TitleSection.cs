using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class TitleSection : RadExpander
    { // text
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(TitleSection), null, BindingMode.TwoWay, propertyChanged: TextChange);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        private static void TextChange(BindableObject bindable, object oldValue, object newValue)
        {
            TitleSection control = (TitleSection)bindable;
            if (newValue != null)
                control.label.Text = (string)newValue;
        }
        public Label label { get; set; }
        public TitleSection()
        {
            label = new Label();
            label.FontSize = 16;
            label.FontAttributes = FontAttributes.Bold;
            label.SetBinding(Label.TextProperty, "Text");
            label.TextColor = Color.FromHex("#444444");
            ExpanderHeader headerStyle = new ExpanderHeader();
            headerStyle.IndicatorText = "\uf105";
            headerStyle.IndicatorFontFamily = "FontAwesomeSolid";
            headerStyle.IndicatorLocation = ExpandCollapseIndicatorLocation.End;
            headerStyle.IndicatorFontSize = 18;
            headerStyle.IndicatorColor = Color.FromHex("#939393");
            headerStyle.BackgroundColor = Color.FromHex("#70F3F3F3");
            headerStyle.BorderColor = Color.Transparent;
            headerStyle.BorderThickness = 0;
            headerStyle.Padding = new Thickness(17, 8);
            headerStyle.Content = label;
            this.Header = headerStyle;
        }
    }
}
