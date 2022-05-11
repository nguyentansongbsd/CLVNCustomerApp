using CustomerApp.Helper;
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
    public partial class TransactionPage : ContentPage
    {
        public static bool? NeedToRefreshContract = null;
        public static bool? NeedToRefreshDatCoc = null;
        private ContractContentview ContractContentview;
        private DatCocContentView DatCocContentView;
        public TransactionPage()
        {
            LoadingHelper.Show();
            InitializeComponent();
            NeedToRefreshContract = false;
            NeedToRefreshDatCoc = false;
            Init();
        }
        public async void Init()
        {
            VisualStateManager.GoToState(radBorderDatCoc, "InActive");
            VisualStateManager.GoToState(radBorderContract, "Active");
            VisualStateManager.GoToState(lblDatCoc, "InActive");
            VisualStateManager.GoToState(lblContract, "Active");
            if (ContractContentview == null)
            {
                LoadingHelper.Show();
                ContractContentview = new ContractContentview();
            }
            ContractContentview.OnCompleted = (IsSuccess) =>
            {
                TransactionContentView.Children.Add(ContractContentview);
                LoadingHelper.Hide();
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (ContractContentview != null && NeedToRefreshContract == true)
            {
                LoadingHelper.Show();
                await ContractContentview.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshContract = false;
                LoadingHelper.Hide();
            }

            if (DatCocContentView != null && NeedToRefreshDatCoc == true)
            {
                LoadingHelper.Show();
                await DatCocContentView.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshDatCoc = false;
                LoadingHelper.Hide();
            }
        }      

        private void DatCoc_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderDatCoc, "Active");
            VisualStateManager.GoToState(radBorderContract, "InActive");
            VisualStateManager.GoToState(lblDatCoc, "Active");
            VisualStateManager.GoToState(lblContract, "InActive");
            if (DatCocContentView == null)
            {
                LoadingHelper.Show();
                DatCocContentView = new DatCocContentView();
            }
            DatCocContentView.OnCompleted = (IsSuccess) =>
            {
                TransactionContentView.Children.Add(DatCocContentView);
                LoadingHelper.Hide();
            };
            DatCocContentView.IsVisible = true;
            if (ContractContentview != null)
            {
                ContractContentview.IsVisible = false;
            }
        }

        private void Contract_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderDatCoc, "InActive");
            VisualStateManager.GoToState(radBorderContract, "Active");
            VisualStateManager.GoToState(lblDatCoc, "InActive");
            VisualStateManager.GoToState(lblContract, "Active");
            if (ContractContentview == null)
            {
                LoadingHelper.Show();
                ContractContentview = new ContractContentview();
            }
            ContractContentview.OnCompleted = (IsSuccess) =>
            {
                TransactionContentView.Children.Add(ContractContentview);
                LoadingHelper.Hide();
            };
            ContractContentview.IsVisible = true;
            if (DatCocContentView != null)
            {
                DatCocContentView.IsVisible = false;
            }
        }      
    }
}