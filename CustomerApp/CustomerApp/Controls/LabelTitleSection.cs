using System;
using System.Collections.Generic;
using System.Text;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class LabelTitleSection : Label
    {
        public LabelTitleSection()
        {
            this.FontSize = 16;
            this.VerticalTextAlignment = TextAlignment.Center;
            this.FontAttributes = FontAttributes.Bold;
            this.BackgroundColor = Color.FromHex("#80f1f1f1");
            TextColor = Color.FromHex("#444444");
            Padding= new Thickness(25,10);
        }
    }
}
