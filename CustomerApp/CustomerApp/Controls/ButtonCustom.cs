using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class ButtonCustom : Button
    {
        // text
        public static readonly BindableProperty TextBtnProperty = BindableProperty.Create(nameof(TextBtn), typeof(string), typeof(ButtonCustom), null, BindingMode.TwoWay, propertyChanged: TextBtnChange);
        public string TextBtn { get => (string)GetValue(TextBtnProperty); set => SetValue(TextBtnProperty, value); }
        private static void TextBtnChange(BindableObject bindable, object oldValue, object newValue)
        {
            ButtonCustom control = (ButtonCustom)bindable;
            if (newValue != null)
                control.ChangeTextBusy();
        }
        // style name
        public static readonly BindableProperty StyleNameProperty = BindableProperty.Create(nameof(StyleName), typeof(StyleName), typeof(ButtonCustom), StyleName.Blue, BindingMode.TwoWay, propertyChanged: StyleNameChange);
        public StyleName StyleName { get => (StyleName)GetValue(StyleNameProperty); set => SetValue(StyleNameProperty, value); }
        private static void StyleNameChange(BindableObject bindable, object oldValue, object newValue)
        {
            ButtonCustom control = (ButtonCustom)bindable;
            if (newValue != null)
                control.SetupStyle();
        }
        public static readonly BindableProperty isBusyProperty = BindableProperty.Create(nameof(isBusy), typeof(bool), typeof(ButtonCustom), false, BindingMode.TwoWay, propertyChanged: isBusyChange);
        public bool isBusy { get => (bool)GetValue(isBusyProperty); set => SetValue(isBusyProperty, value); }
        private static void isBusyChange(BindableObject bindable, object oldValue, object newValue)
        {
            ButtonCustom control = (ButtonCustom)bindable;
            if (newValue != null)
                control.ChangeTextBusy();
        }

        public ButtonCustom()
        {
            SetupStyle();
        }
        private void SetupStyle()
        {
            this.CornerRadius = 10;
            this.BorderWidth = 1;
            this.HeightRequest = 40;
            this.FontSize = 16;
            this.TextTransform = TextTransform.None;

            if (StyleName == StyleName.Blue)
            {
                this.BackgroundColor = (Color)App.Current.Resources["Primary"];
                this.TextColor = Color.White;
                this.BorderColor = (Color)App.Current.Resources["Primary"];
            }
            else if (StyleName == StyleName.White)
            {
                this.BackgroundColor = Color.White;
                this.TextColor = (Color)App.Current.Resources["Primary"];
                this.BorderColor = (Color)App.Current.Resources["Primary"];
            }
        }

        private void ChangeTextBusy()
        {
            if (isBusy)
            {
                this.Text = "Đang xử lý";
                this.IsEnabled = false;
            }
            else
            {
                this.Text = TextBtn;
                this.IsEnabled = true;
            }
        }
    }
    public enum StyleName
    {
        Blue,
        White
    }
}
