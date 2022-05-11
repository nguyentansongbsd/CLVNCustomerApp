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
    public partial class QueueContentView : ContentView
    {
        public Action<bool> OnCompleted;
        public QueueContentViewViewModel viewModel;
        public QueueContentView()
        {
            InitializeComponent();
            LoadingHelper.Show();
            BindingContext = viewModel = new QueueContentViewViewModel();
            Init();
        }
        public async void Init()
        {
            await Task.WhenAll(
                  viewModel.LoadData(),
                  viewModel.LoadProject());
            viewModel.LoadStatus();
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

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            QueuesModel val = e.Item as QueuesModel;
            LoadingHelper.Show();
            QueuesDetialPage queuesDetialPage = new QueuesDetialPage(val.opportunityid);
            queuesDetialPage.OnCompleted = async (isSuccess) =>
            {
                if (isSuccess)
                {
                    await Navigation.PushAsync(queuesDetialPage);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                }
            };
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

        private async void FiltersProject_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }

        private async void FiltersStatus_SelectedItemChanged(object sender, LookUpChangeEvent e)
        {
            LoadingHelper.Show();
            await viewModel.LoadOnRefreshCommandAsync();
            LoadingHelper.Hide();
        }
    }
}