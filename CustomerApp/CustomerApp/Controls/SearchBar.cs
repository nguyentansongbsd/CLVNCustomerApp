using CustomerApp.Resources;
using System;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class SearchBar : Xamarin.Forms.SearchBar
    {
        public SearchBar()
        {
            Placeholder = Language.tim_kiem;
            FontSize = 15;
            TextColor = Color.FromHex("#444444");
            FontFamily = "Segoe";
            BackgroundColor = Color.White;
        }
    }
}
