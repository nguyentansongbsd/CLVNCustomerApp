using System;
using System.Collections.Generic;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class CaseDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        public CaseDetailPageViewModel viewModel;
        public CaseDetailPage(Guid caseId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new CaseDetailPageViewModel();
            viewModel.CaseId = caseId;
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadCaseInfor();
            if (viewModel.Case != null)
            {
                VisualStateManager.GoToState(radBorderThongTin, "Selected");
                VisualStateManager.GoToState(lbThongTin, "Selected");
                VisualStateManager.GoToState(radBorderPhanHoiLienQuan, "Normal");
                VisualStateManager.GoToState(lbPhanHoiLienQuan, "Normal");
                if (viewModel.Case.statuscode != 1)
                {
                    floatingButtonGroup.IsVisible = false;
                }
                else
                {
                    viewModel.ButtonCommandList.Add(new FloatButtonItem(Language.huy_phan_hoi, "FontAwesomeRegular", "\uf273", null, CancelCase));
                    floatingButtonGroup.IsVisible = true;
                }

                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }

        private void ThongTin_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderThongTin, "Selected");
            VisualStateManager.GoToState(lbThongTin, "Selected");
            VisualStateManager.GoToState(radBorderPhanHoiLienQuan, "Normal");
            VisualStateManager.GoToState(lbPhanHoiLienQuan, "Normal");
            TabThongTin.IsVisible = true;
            TabPhanHoiLienQuan.IsVisible = false;
        }

        private async void PhanHoiLienQuan_Tapped(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(radBorderThongTin, "Normal");
            VisualStateManager.GoToState(lbThongTin, "Normal");
            VisualStateManager.GoToState(radBorderPhanHoiLienQuan, "Selected");
            VisualStateManager.GoToState(lbPhanHoiLienQuan, "Selected");
            TabThongTin.IsVisible = false;
            TabPhanHoiLienQuan.IsVisible = true;
            if (viewModel.ListCase.Count == 0)
            {
                await viewModel.LoadListCase();
            }
        }

        private async void ShowMoreCase_Clicked(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.PageCase++;
            await viewModel.LoadListCase();
            LoadingHelper.Hide();
        }

        private async void CancelCase(object sender, EventArgs e)
        {
            string options = await DisplayActionSheet(Language.huy_phan_hoi, Language.khong, Language.co, Language.xac_nhan_huy_phan_hoi);
            if (options == Language.co)
            {
                LoadingHelper.Show();
                viewModel.Case.statecode = 2;
                viewModel.Case.statuscode = 6;
                if (await viewModel.UpdateCase())
                {
                    await viewModel.LoadCaseInfor();
                    floatingButtonGroup.IsVisible = viewModel.Case.statuscode != 1 ? false : true;
                    if (CasesPage.NeedToRefresh.HasValue) CasesPage.NeedToRefresh = true;
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                }
            }
            else if (options == Language.khong)
            {
                
            }
        }
    }
}
