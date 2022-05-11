using CustomerApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CustomerApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _text;
        public string Text { get => _text; set { _text = value; OnPropertyChanged(nameof(Text)); } }
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            Text = "Testttttttttttttt";
        }

        private async void OnLoginClicked(object obj)
        {
        }
    }
}

