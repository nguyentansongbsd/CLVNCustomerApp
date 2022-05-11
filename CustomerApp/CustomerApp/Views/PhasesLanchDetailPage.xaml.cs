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
    public partial class PhasesLanchDetailPage : ContentPage
    {
        public Action<bool> OnCompleted;
        private PhasesLanchDetailPageViewModel viewModel;
        private Guid PhasesLanchId;
        public PhasesLanchDetailPage(Guid phasesLanchId)
        {
            InitializeComponent();
            PhasesLanchId = phasesLanchId;
            Init();
        }

        public async void Init()
        {
            BindingContext = viewModel = new PhasesLanchDetailPageViewModel();
            await viewModel.LoadPhasesLanch(PhasesLanchId);
            if (viewModel.PhasesLanch != null)
            {
                OnCompleted?.Invoke(true);
            }
            else
            {
                OnCompleted?.Invoke(false);
            }
        }
    }
}