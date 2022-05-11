using System;
using System.Collections.Generic;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class NewsPage : ContentPage
    {
        private string url = "https://www.phatdat.com.vn/news_category/tin-phat-dat/";
        public NewsPageViewModel viewModel;
        public NewsPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new NewsPageViewModel();
            Init();
        }

        private async void Init()
        {
            VisualStateManager.GoToState(radBorderNews, "Selected");
            VisualStateManager.GoToState(lblNews, "Selected");
            VisualStateManager.GoToState(radBorderPhasesLaunch, "Normal");
            VisualStateManager.GoToState(lblPhasesLaunch, "Normal");
            webNews.Source = url;
        }

        private void News_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderNews, "Selected");
            VisualStateManager.GoToState(lblNews, "Selected");
            VisualStateManager.GoToState(radBorderPhasesLaunch, "Normal");
            VisualStateManager.GoToState(lblPhasesLaunch, "Normal");
            webNews.IsVisible = true;
            listPhasesLaunch.IsVisible = false;
        }

        private async void PhasesLaunch_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderNews, "Normal");
            VisualStateManager.GoToState(lblNews, "Normal");
            VisualStateManager.GoToState(radBorderPhasesLaunch, "Selected");
            VisualStateManager.GoToState(lblPhasesLaunch, "Selected");
            webNews.IsVisible = false;
            listPhasesLaunch.IsVisible = true;
            if (viewModel.PhasesLaunchs == null)
            {
                await viewModel.LoadPhasesLaunchs();
            }
        }
    }
}
