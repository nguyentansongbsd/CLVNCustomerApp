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
    public partial class LabelListViewItem : Grid
    {
        // title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(LabelListViewItem), null, BindingMode.TwoWay);
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        // value
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelListViewItem), null, BindingMode.TwoWay, propertyChanged: TextChange);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        private static void TextChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelListViewItem control = (LabelListViewItem)bindable;
            if (newValue != null)
                control.lb_text.Text = (string)newValue;
        }
        // color
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LabelListViewItem), (Color)App.Current.Resources["TextColor"], BindingMode.TwoWay, propertyChanged: TextColorChange);
        public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
        private static void TextColorChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelListViewItem control = (LabelListViewItem)bindable;
            if (newValue != null)
                control.lb_text.TextColor = (Color)newValue;
            else
                control.lb_text.TextColor = (Color)App.Current.Resources["TextColor"];
        }
        // FontAttributes
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(LabelListViewItem), null, BindingMode.TwoWay, propertyChanged: FontAttributesChange);
        public FontAttributes FontAttributes { get => (FontAttributes)GetValue(FontAttributesProperty); set => SetValue(FontAttributesProperty, value); }
        private static void FontAttributesChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelListViewItem control = (LabelListViewItem)bindable;
            if (newValue != null)
                control.lb_text.FontAttributes = (FontAttributes)newValue;
            else
                control.lb_text.FontAttributes = FontAttributes.None;
        }
        // LineBreakMode
        public static readonly BindableProperty isWordWrapProperty = BindableProperty.Create(nameof(isWordWrap), typeof(bool), typeof(LabelListViewItem), false, BindingMode.TwoWay, propertyChanged: isWordWrapChange);
        public bool isWordWrap { get => (bool)GetValue(isWordWrapProperty); set => SetValue(isWordWrapProperty, value); }
        private static void isWordWrapChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelListViewItem control = (LabelListViewItem)bindable;
            if ((bool)newValue == true)
                control.lb_text.LineBreakMode = LineBreakMode.WordWrap;
            else
                control.lb_text.LineBreakMode = LineBreakMode.TailTruncation;
        }
        public LabelListViewItem()
        {
            InitializeComponent();
            lb_title.BindingContext = this;
        }
    }
}