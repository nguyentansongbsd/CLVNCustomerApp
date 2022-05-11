using System;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class MainEditor : Editor
    {
        public MainEditor()
        {
            TextColor = Color.Black;
            FontSize = 15;
            this.PlaceholderColor = Color.Gray;
            this.FontFamily = "Segoe";
        }
    }
}
