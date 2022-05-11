using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPageViewModel viewModel;
        public DashboardPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DashboardPageViewModel();
            Init();
        }

        public async void Init()
        {
            await Task.WhenAll(
                viewModel.LoadContactLoyalty(),
                viewModel.LoadQueueFourMonths(),
                viewModel.LoadQuoteFourMonths(),
                viewModel.LoadOptionEntryFourMonths(),
                viewModel.LoadUnitFourMonths(),
                viewModel.LoadMeetings(),
                viewModel.LoadQueues(),
                viewModel.LoadDeposits(),
                viewModel.LoadInstallments(),
                viewModel.LoadDepositsSigning(),
                viewModel.LoadDepositsSigning(),
                viewModel.LoadContracts()
                ) ;
        }
    }
}
