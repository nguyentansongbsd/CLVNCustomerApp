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
        public static bool? NeedToRefreshLead = null;
        public static bool? NeedToRefreshContact = null;
        public static bool? NeedToRefreshAccount = null;
        private QueueContentView QueueContentView;
        private ContractContentview ContractContentview;
        private DatCocContentView DatCocContentView;
        public TransactionPage()
        {
            LoadingHelper.Show();
            InitializeComponent();
            NeedToRefreshLead = false;
            NeedToRefreshContact = false;
            NeedToRefreshAccount = false;
            Init();
        }
        public async void Init()
        {
            VisualStateManager.GoToState(radBorderLead, "Active");
            VisualStateManager.GoToState(radBorderAccount, "InActive");
            VisualStateManager.GoToState(radBorderContact, "InActive");
            VisualStateManager.GoToState(lblLead, "Active");
            VisualStateManager.GoToState(lblAccount, "InActive");
            VisualStateManager.GoToState(lblContact, "InActive");
            if (QueueContentView == null)
            {
                QueueContentView = new QueueContentView();
            }
            QueueContentView.OnCompleted = async (IsSuccess) =>
            {
                TransactionContentView.Children.Add(QueueContentView);
                LoadingHelper.Hide();
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (QueueContentView != null && NeedToRefreshLead == true)
            {
                LoadingHelper.Show();
                await QueueContentView.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshLead = false;
                LoadingHelper.Hide();
            }

            if (ContractContentview != null && NeedToRefreshContact == true)
            {
                LoadingHelper.Show();
                await ContractContentview.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshContact = false;
                LoadingHelper.Hide();
            }

            if (DatCocContentView != null && NeedToRefreshAccount == true)
            {
                LoadingHelper.Show();
                await DatCocContentView.viewModel.LoadOnRefreshCommandAsync();
                NeedToRefreshAccount = false;
                LoadingHelper.Hide();
            }
        }

        private void Lead_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderLead, "Active");
            VisualStateManager.GoToState(radBorderAccount, "InActive");
            VisualStateManager.GoToState(radBorderContact, "InActive");
            VisualStateManager.GoToState(lblLead, "Active");
            VisualStateManager.GoToState(lblAccount, "InActive");
            VisualStateManager.GoToState(lblContact, "InActive");
            QueueContentView.IsVisible = true;
            if (DatCocContentView != null)
            {
                DatCocContentView.IsVisible = false;
            }
            if (ContractContentview != null)
            {
                ContractContentview.IsVisible = false;
            }
        }

        private void Account_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderLead, "InActive");
            VisualStateManager.GoToState(radBorderAccount, "Active");
            VisualStateManager.GoToState(radBorderContact, "InActive");
            VisualStateManager.GoToState(lblLead, "InActive");
            VisualStateManager.GoToState(lblAccount, "Active");
            VisualStateManager.GoToState(lblContact, "InActive");
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
            QueueContentView.IsVisible = false;
            DatCocContentView.IsVisible = true;
            if (ContractContentview != null)
            {
                ContractContentview.IsVisible = false;
            }
        }

        private void Contact_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderLead, "InActive");
            VisualStateManager.GoToState(radBorderAccount, "InActive");
            VisualStateManager.GoToState(radBorderContact, "Active");
            VisualStateManager.GoToState(lblLead, "InActive");
            VisualStateManager.GoToState(lblAccount, "InActive");
            VisualStateManager.GoToState(lblContact, "Active");
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
            QueueContentView.IsVisible = false;
            ContractContentview.IsVisible = true;
            if (DatCocContentView != null)
            {
                DatCocContentView.IsVisible = false;
            }
        }      
    }
}