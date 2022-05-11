using System;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class LookUpEntry : Entry
    {
        public LookUpEntry()
        {
            TextColor = Color.Black;
            this.FontSize = 15;
            this.PlaceholderColor = Color.Gray;
            this.HeightRequest = 40;
        }
    }
}
