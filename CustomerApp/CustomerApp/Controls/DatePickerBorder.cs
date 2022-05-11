using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Xamarin.Forms;

namespace CustomerApp.Controls
{
    public class DatePickerBorder : DatePicker
    {
        public event EventHandler OnChangeState;
        
        public DatePickerBorder()
        {
            this.HeightRequest = 40;
            this.FontSize = 15;
            this.TextColor = Color.FromHex("#333333");
        }

        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
            OnChangeState?.Invoke(this, EventArgs.Empty);
        }
    }
}
