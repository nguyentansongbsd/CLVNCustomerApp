using CustomerApp.Helpers;
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
    public partial class LabelField : Grid
    {
        // title
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(LabelField), null, BindingMode.TwoWay);
        public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }
        // value
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelField), null, BindingMode.TwoWay, propertyChanged: TextChange);
        public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
        private static void TextChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelField control = (LabelField)bindable;
            if (newValue != null)
                control.lb_text.Text = (string)newValue;
        }
        //// color
        //public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LabelField), (Color)App.Current.Resources["TextColor"], BindingMode.TwoWay, propertyChanged: TextColorChange);
        //public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
        //private static void TextColorChange(BindableObject bindable, object oldValue, object newValue)
        //{
        //    LabelField control = (LabelField)bindable;
        //    if (newValue != null)
        //        control.lb_text.TextColor = (Color)newValue;
        //    else
        //        control.lb_text.TextColor = (Color)App.Current.Resources["TextColor"];
        //}
        //// FontAttributes
        //public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(LabelField), null, BindingMode.TwoWay, propertyChanged: FontAttributesChange);
        //public FontAttributes FontAttributes { get => (FontAttributes)GetValue(FontAttributesProperty); set => SetValue(FontAttributesProperty, value); }
        //private static void FontAttributesChange(BindableObject bindable, object oldValue, object newValue)
        //{
        //    LabelField control = (LabelField)bindable;
        //    if (newValue != null)
        //        control.lb_text.FontAttributes = (FontAttributes)newValue;
        //    else
        //        control.lb_text.FontAttributes = FontAttributes.None;
        //}

        // Event Tap
        public event EventHandler TextTapped;
        // FormatText theo 3 loại 
        public static readonly BindableProperty FormatTextProperty = BindableProperty.Create(nameof(FormatText), typeof(FormatTexts), typeof(LabelField), FormatTexts.None, BindingMode.TwoWay, propertyChanged: FormatTextChange);
        public FormatTexts FormatText { get => (FormatTexts)GetValue(FormatTextProperty); set => SetValue(FormatTextProperty, value); }
        private static void FormatTextChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelField control = (LabelField)bindable;
            if (newValue != null)
                control.FormatTextFiled();
        }
        // FormatText theo 3 loại 
        public static readonly BindableProperty FormatStyleProperty = BindableProperty.Create(nameof(FormatStyle), typeof(FormatStyles), typeof(LabelField), FormatStyles.None, BindingMode.TwoWay, propertyChanged: FormatStyleChange);
        public FormatStyles FormatStyle { get => (FormatStyles)GetValue(FormatStyleProperty); set => SetValue(FormatStyleProperty, value); }
        private static void FormatStyleChange(BindableObject bindable, object oldValue, object newValue)
        {
            LabelField control = (LabelField)bindable;
            if (newValue != null)
                control.FormatStyleField();
        }
        public LabelField()
        {
            InitializeComponent();
            // this.BindingContext = this;
            // lb_text.SetBinding(Label.TextProperty, "Text");
            lb_title.BindingContext = this;
        }

        private void Text_Tapped(object sender, EventArgs e)
        {
            TextTapped?.Invoke((object)this, EventArgs.Empty);
        }
        private void FormatTextFiled()
        {
            if (FormatText == FormatTexts.None) return;

            decimal value;
            if (decimal.TryParse(Text, out value))
            {
                if (FormatText == FormatTexts.Currency)
                    this.Text = StringFormatHelper.FormatCurrency(value);// + " đ";
                else if (FormatText == FormatTexts.Percent)
                    this.Text = StringFormatHelper.FormatPercent(value);// + " %";
            }      
        }
        private void FormatStyleField()
        {
            if (FormatStyle == FormatStyles.None) return;
            if (FormatStyle == FormatStyles.BoldRed)
            {
                lb_text.FontAttributes = FontAttributes.Bold;
                lb_text.TextColor = Color.Red;
            }   
            else if (FormatStyle == FormatStyles.BoldBlue)
            {
                lb_text.FontAttributes = FontAttributes.Bold;
                lb_text.TextColor= (Color)App.Current.Resources["Primary"];
            }
        }
    }
    public enum FormatTexts
    {
        Currency,
        Percent,
        None
    }
    public enum FormatStyles
    {
        BoldRed,
        BoldBlue,
        None
    }
}