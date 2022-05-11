using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatCocContentView : ContentView
    {
        public Action<bool> OnCompleted;
        public DatCocContentViewViewModel viewModel;
        public static bool? NeedToRefresh;
        public DatCocContentView()
        {
            InitializeComponent();
            BindingContext = viewModel = new DatCocContentViewViewModel();
            NeedToRefresh = false;
            LoadingHelper.Show();
            Init();
        }

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (NeedToRefresh == true)
        //    {
        //        LoadingHelper.Show();
        //        await viewModel.LoadOnRefreshCommandAsync();
        //        LoadingHelper.Hide();
        //        NeedToRefresh = false;
        //    }
        //}

        public async void Init()
        {
            await viewModel.LoadData();
            if (viewModel.Data.Count > 0)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
            LoadingHelper.Hide();
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ReservationModel val = e.Item as ReservationModel;
            LoadingHelper.Show();
            BangTinhGiaDetailPage newPage = new BangTinhGiaDetailPage(val.quoteid) { Title = Language.dat_coc };
            newPage.OnCompleted = async (OnCompleted) =>
            {
                if (OnCompleted == true)
                {
                    await Navigation.PushAsync(newPage);
                }
                LoadingHelper.Hide();
            };
        }

        private async void SearchBar_SearchButtonPressed(System.Object sender, System.EventArgs e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(viewModel.Keyword))
            {
                SearchBar_SearchButtonPressed(null, EventArgs.Empty);
            }
        }
    }
}