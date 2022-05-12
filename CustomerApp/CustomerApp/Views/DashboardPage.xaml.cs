using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.Resources;
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
            await Task.WhenAll( viewModel.LoadContracts(),viewModel.LoadProjects());


            //await Task.WhenAll(
            //    viewModel.LoadContactLoyalty(),
            //    viewModel.LoadQueueFourMonths(),
            //    viewModel.LoadQuoteFourMonths(),
            //    viewModel.LoadOptionEntryFourMonths(),
            //    viewModel.LoadUnitFourMonths(),
            //    viewModel.LoadMeetings(),
            //    viewModel.LoadQueues(),
            //    viewModel.LoadDeposits(),
            //    viewModel.LoadInstallments(),
            //    viewModel.LoadDepositsSigning(),
            //    viewModel.LoadDepositsSigning(),
            //    viewModel.LoadContracts()
            //    ) ;
        }

        private void ContractDetail_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            try
            {
                Guid id = (Guid)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                ContractDetailPage contract = new ContractDetailPage(id);
                contract.OnCompleted = async (isSuccess) => {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(contract);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };

            }
            catch(Exception ex)
            {

            }
            
        }
        
        private async void ShowMore_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new TransactionPage());
            LoadingHelper.Hide();
        }

        
        private async void ShowMoreDuAn_Tapped(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            await Navigation.PushAsync(new ProjectsPage());
            LoadingHelper.Hide();
        }

        private void ProjectDetail_Tapped(object sender, EventArgs e)
        {
            try
            {
                LoadingHelper.Show();
                string id = (string)((sender as Grid).GestureRecognizers[0] as TapGestureRecognizer).CommandParameter;
                ProjectInfoPage project = new ProjectInfoPage(Guid.Parse(id));
                project.OnCompleted = async (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        await Navigation.PushAsync(project);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.noti_khong_tim_thay_thong_tin_vui_long_thu_lai);
                    }
                };
            }
            catch(Exception ex)
            {

            }
            
        }

    }
}
