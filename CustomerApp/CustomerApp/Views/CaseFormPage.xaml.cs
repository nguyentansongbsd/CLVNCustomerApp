using System;
using System.Collections.Generic;
using CustomerApp.Datas;
using CustomerApp.Helper;
using CustomerApp.Resources;
using CustomerApp.ViewModels;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class CaseFormPage : ContentPage
    {
        public CaseFormPageViewModel viewModel;
        public CaseFormPage()
        {
            InitializeComponent();
            Init();
        }

        private async void Init()
        {
            this.BindingContext = viewModel = new CaseFormPageViewModel();
            SerPreOpen();
        }

        private void SerPreOpen()
        {
            lookupCaseType.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.CaseTypes = CaseTypeData.CasesData();
                LoadingHelper.Hide();
            };

            lookupSubjects.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadSubjects();
                LoadingHelper.Hide();
            };

            lookupCaseOrigin.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.CaseOrigins = CaseOriginData.Origins();
                LoadingHelper.Hide();
            };

            lookupCaseLienQuan.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                await viewModel.LoadCaseLienQuan();
                LoadingHelper.Hide();
            };

            lookupProjects.PreOpenAsync = async () =>
            {
                LoadingHelper.Show();
                viewModel.Projects = await viewModel.LoadProjects();
                LoadingHelper.Hide();
            };

            lookupUnits.PreOpenOneTime = false;
            lookupUnits.PreOpen = async () => {
                if (viewModel.Project == null)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_du_an);
                }
            };
        }

        private async void ProjectItem_Changed(object sender, EventArgs e)
        {
            LoadingHelper.Show();
            viewModel.Unit = null;
            viewModel.Units = null;
            await viewModel.LoadUnits();
            LoadingHelper.Hide();
        }

        private async void SaveCase_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (viewModel.CaseType == null)
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_chon_loai_phan_hoi);
                    return;
                }

                if (string.IsNullOrWhiteSpace(viewModel.CaseModel.title))
                {
                    ToastMessageHelper.ShortMessage(Language.vui_long_nhap_tieu_de);
                    return;
                }

                LoadingHelper.Show();
                bool isSuccess = await viewModel.CreateCase();
                if (isSuccess)
                {
                    if (CasesPage.NeedToRefresh.HasValue) CasesPage.NeedToRefresh = true;
                    await Navigation.PopAsync();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_thanh_cong);
                    LoadingHelper.Hide();
                }
                else
                {
                    LoadingHelper.Hide();
                    ToastMessageHelper.ShortMessage(Language.thong_bao_that_bai);
                }
            }
            catch(Exception ex)
            {
                LoadingHelper.Hide();
                ToastMessageHelper.LongMessage(ex.Message);
            }
            
        }
    }
}
