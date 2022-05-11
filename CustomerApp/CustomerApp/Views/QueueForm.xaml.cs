using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.Settings;
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
    public partial class QueueForm : ContentPage
    {
        public Action<bool> OnCompleted;
        public QueueFormViewModel viewModel;
        public Guid QueueId;
        private bool from;
        public QueueForm(Guid unitId, bool fromDirectSale) // Direct Sales (add)
        {
            InitializeComponent();
            Init();
            viewModel.UnitId = unitId;
            from = fromDirectSale;
            Create();
        }

        public void Init()
        {
            this.BindingContext = viewModel = new QueueFormViewModel();
            viewModel.Customer = new OptionSetFilter { Val = UserLogged.Id.ToString(), Name = UserLogged.User };
            SetPreOpen();
        }

        public async void SetPreOpen()
        {
            lookUpDaiLy.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSalesAgentCompany();
                LoadingHelper.Hide();
            };
            lookUpCollaborator.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCollaboratorLookUp();
                LoadingHelper.Hide();
            };
            lookUpCustomerReferral.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCustomerReferralLookUp();
                LoadingHelper.Hide();
            };
        }

        public async void Create()
        {
            if (from)
            {
                await viewModel.LoadFromUnit(viewModel.UnitId);
                string res = await viewModel.createQueueDraft(false, viewModel.UnitId);
                topic.Text = viewModel.Queue.bsd_units_name;
                if (viewModel.Queue.bsd_units_id != Guid.Empty && viewModel.idQueueDraft != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(res);
                }
            }
            else
            {
                await viewModel.LoadFromProject(viewModel.UnitId);
                string res = await viewModel.createQueueDraft(true, viewModel.UnitId);
                topic.Text = viewModel.Queue.bsd_project_name + " - " + DateTime.Now.ToString("dd/MM/yyyy");
                if (viewModel.Queue.bsd_project_id != Guid.Empty && viewModel.idQueueDraft != Guid.Empty)
                    OnCompleted?.Invoke(true);
                else
                {
                    OnCompleted?.Invoke(false);
                    ToastMessageHelper.ShortMessage(res);
                }
            }
        }
        private void lookUpDaiLy_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.DailyOption != null && viewModel.DailyOption.Id != Guid.Empty)
            {
                viewModel.Collaborator = null;
                viewModel.CustomerReferral = null;
            }
        }
        private void lookUpCollaborator_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.Collaborator != null && viewModel.Collaborator.Id != Guid.Empty)
            {
                viewModel.DailyOption = null;
                viewModel.CustomerReferral = null;
            }
        }
        private void lookUpCustomerReferral_SelectedItemChange(object sender, LookUpChangeEvent e)
        {
            if (viewModel.CustomerReferral != null && viewModel.CustomerReferral.Id != Guid.Empty)
            {
                viewModel.DailyOption = null;
                viewModel.Collaborator = null;
            }
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            viewModel.isBusy = true;
            if (string.IsNullOrWhiteSpace(viewModel.Queue.name))
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_nhap_tieu_de_giu_cho);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.Customer == null || string.IsNullOrWhiteSpace(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.noti_vui_long_chon_khach_hang);
                LoadingHelper.Hide();
                return;
            }
            if (from)
            {
                if (!await viewModel.SetQueueTime())
                {
                    ToastMessageHelper.ShortMessage(Language.noti_khach_hang_da_tham_gia_giu_cho_cho_du_an_nay);
                    LoadingHelper.Hide();
                    return;
                }
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.DailyOption != null && viewModel.DailyOption.Id != Guid.Empty && viewModel.DailyOption.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.noti_khach_hang_phai_khac_dai_ly_ban_hang);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.Collaborator != null && viewModel.Collaborator.Id != Guid.Empty && viewModel.Collaborator.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.noti_khach_hang_phai_khac_cong_tac_vien);
                LoadingHelper.Hide();
                return;
            }
            if (viewModel.Customer != null && !string.IsNullOrWhiteSpace(viewModel.Customer.Val) && viewModel.CustomerReferral != null && viewModel.CustomerReferral.Id != Guid.Empty && viewModel.CustomerReferral.Id == Guid.Parse(viewModel.Customer.Val))
            {
                ToastMessageHelper.ShortMessage(Language.noti_khach_hang_phai_khac_khach_hang_gioi_thieu);
                LoadingHelper.Hide();
                return;
            }
            var created = await viewModel.UpdateQueue(viewModel.idQueueDraft);
            if (created)
            {
                RefreshPage();
                await Navigation.PopAsync();
                viewModel.isBusy = false;
                ToastMessageHelper.ShortMessage(Language.noti_thanh_cong);
                LoadingHelper.Hide();
            }
            else
            {
                viewModel.isBusy = false;
                LoadingHelper.Hide();
                if (!string.IsNullOrWhiteSpace(viewModel.Error_update_queue))
                    ToastMessageHelper.ShortMessage(viewModel.Error_update_queue);
                else
                    ToastMessageHelper.ShortMessage(Language.noti_that_bai);
            }
        }
        private void RefreshPage()
        {
            if (ProjectInfoPage.NeedToRefreshQueue.HasValue) ProjectInfoPage.NeedToRefreshQueue = true;
            if (ProjectInfoPage.NeedToRefreshNumQueue.HasValue) ProjectInfoPage.NeedToRefreshNumQueue = true;
            if (DirectSaleDetail.NeedToRefreshDirectSale.HasValue) DirectSaleDetail.NeedToRefreshDirectSale = true;
            if (UnitInfoPage.NeedToRefreshQueue.HasValue) UnitInfoPage.NeedToRefreshQueue = true;
            //if (DashboardPage.NeedToRefreshQueue.HasValue) DashboardPage.NeedToRefreshQueue = true;
        }
    }
}