using CustomerApp.Helper;
using CustomerApp.Models;
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
    public partial class ContractContentview : ContentView
    {
        public Action<bool> OnCompleted;
        public ContractContentviewViewModel viewModel;
        public ContractContentview()
        {
            InitializeComponent();
            BindingContext = viewModel = new ContractContentviewViewModel();
            LoadingHelper.Show();
            Init();
        }
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

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            LoadingHelper.Show();
            ContractModel item = e.Item as ContractModel;
            ContractDetailPage contractDetailPage = new ContractDetailPage(item.salesorderid);
            await Navigation.PushAsync(contractDetailPage);

            //contractDetailPage.OnCompleted = async (OnCompleted) =>
            //{
            //    if (OnCompleted == true)
            //    {
            //        await Navigation.PushAsync(contractDetailPage);
            //        LoadingHelper.Hide();
            //    }
            //    else
            //    {
            //        LoadingHelper.Hide();
            //        ToastMessageHelper.ShortMessage("Không tìm thấy thông tin hợp đồng");
            //    }

            //};
        }
    }
}