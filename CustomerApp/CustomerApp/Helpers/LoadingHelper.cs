using System;
using System.Threading.Tasks;
using CustomerApp.IServices;
using Xamarin.Forms;

namespace CustomerApp.Helper
{
    public class LoadingHelper
    {
        public static void Show()
        {
            try
            {
                DependencyService.Get<IServices.ILoadingService>().Show();
            }
            catch (Exception ex)
            {

            }
        }

        public static void Hide()
        {
            try
            {
                DependencyService.Get<IServices.ILoadingService>().Hide();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
