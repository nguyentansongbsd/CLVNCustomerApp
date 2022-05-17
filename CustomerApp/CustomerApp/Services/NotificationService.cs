using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CustomerApp.IServices;
using CustomerApp.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CustomerApp.Services
{
    public class NotificationService : INotificationService
    {
        public async Task<string> SaveToken()
        {
            return await Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.GetTokenAsync();
        }

        public async Task SendNotification(NotificaModel model,string Token)
        {
            FirebaseNotificationModel firebaseNotification = new FirebaseNotificationModel()
            {
                to = Token,
                notification = new FirebaseNotification()
                {
                    title = model.Title,
                    body = model.Body,
                    badge = 2
                },
                data = new Dictionary<string, object>()
                        {
                            {
                                "NotificationData", model
                            },
                        }
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=AAAAB5Ywrvo:APA91bFrHONPF2DsVp6WYxH0a8rbGO2VlAFpWDF3uXFRdMI8Rtmv5Q6F3odAZHn0vPeEHpMuUBIG4e2Bb0qq3X8zg9f3HE694hd5LyO35R6ZfoGZybdsLjI7WeAfhM1U63Yi53YAVMQH");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string objContent = JsonConvert.SerializeObject(firebaseNotification);
                    HttpContent content = new StringContent(objContent, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://fcm.googleapis.com/fcm/send", content);

                    var data = await response.Content.ReadAsStringAsync();
                    FirebaseNotiResponse firebaseNoti = JsonConvert.DeserializeObject<FirebaseNotiResponse>(data);
                    if (firebaseNoti.results.Any())
                    {
                        var item = firebaseNoti.results.FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(item.error))
                        {
                            await Shell.Current.DisplayAlert("", item.error, "ok");
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }

        public async Task HandleTapNotification(NotificaModel notification, Action<bool> OnCompeleted)
        {
            //if (!notification.IsRead)
            //{
            //    ApiResponse readResponse = await ApiHelper.Put($"api/notification/read/{notification.Id}", null, true);

            //    if (readResponse.IsSuccess)
            //    {
            //        notification.IsRead = true;
            //    }

            //    INotificationBadge notificationBadge = DependencyService.Get<INotificationBadge>();
            //    var currentBadgeCount = notificationBadge.Get();
            //    int newBadgeCount = currentBadgeCount - 1;
            //    notificationBadge.Set(newBadgeCount >= 0 ? newBadgeCount : 0);
            //}

            //if (notification.NotificationType == NotificationType.ViewPost && notification.GuidItemId.HasValue) //ViewPost
            //{
            //    Helpers.NavigateHelpers.PostDetailPageHelper.GoTo(notification.GuidItemId.Value);
            //}
            //else if (notification.NotificationType == NotificationType.ViewAppointment && notification.GuidItemId.HasValue) //ViewAppointment
            //{
            //    LoadingHelper.Show();
            //    await Shell.Current.Navigation.PushAsync(new AppointmentPage(notification.GuidItemId.Value));
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.ViewPostItem && string.IsNullOrWhiteSpace(notification.PostItemId) == false)
            //{
            //    PostItemDetailPage detailPage = null;
            //    detailPage = new PostItemDetailPage(notification.PostItemId, async (bool isSuccess) =>
            //     {
            //         if (isSuccess)
            //         {
            //             await Task.Delay(1);
            //             await Shell.Current.Navigation.PushAsync(detailPage);
            //             OnCompeleted?.Invoke(true);
            //         }
            //         else
            //         {
            //             OnCompeleted?.Invoke(false);
            //         }
            //     });
            //}
            //else if (notification.NotificationType == NotificationType.VIewFurniturePostItem && string.IsNullOrWhiteSpace(notification.PostItemId) == false)
            //{
            //    LoadingHelper.Show();
            //    await Shell.Current.Navigation.PushAsync(new FurniturePostItemDetailPage(notification.PostItemId));
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.ViewLiquidationPostItem && string.IsNullOrWhiteSpace(notification.PostItemId) == false)
            //{
            //    LoadingHelper.Show();
            //    await Shell.Current.Navigation.PushAsync(new Views.LiquidationViews.PostItemDetailPage(notification.PostItemId));
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.ViewB2BPostItem && string.IsNullOrWhiteSpace(notification.PostItemId) == false)
            //{
            //    LoadingHelper.Show();
            //    await Shell.Current.Navigation.PushAsync(new Views.CompanyViews.B2BDetailPage(notification.PostItemId));
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.ViewInternalPostItem && string.IsNullOrWhiteSpace(notification.PostItemId) == false)
            //{
            //    await Shell.Current.Navigation.PushAsync(new Views.CompanyViews.InternalDetailPage(notification.PostItemId));
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.ViewServicePostItem && string.IsNullOrWhiteSpace(notification.PostItemId) == false)
            //{
            //    LoadingHelper.Show();
            //    await Shell.Current.Navigation.PushAsync(new Views.ServiceViews.PostItemDetailPage(notification.PostItemId));
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.RegisterEmployeeSuccess)
            //{
            //    // lay lai thong tin dang ky
            //    var response = await ApiHelper.Get<User>(ApiRouter.USER_GET_USER_BY_ID + "/" + UserLogged.Id);
            //    if (response.IsSuccess)
            //    {
            //        var userData = response.Content as User;
            //        UserLogged.Type = userData.Type.HasValue ? userData.Type.Value : 0;
            //        UserLogged.RoleId = userData.RoleId.HasValue ? userData.RoleId.Value : -1;
            //        UserLogged.CompanyId = userData.CompanyId.HasValue ? userData.CompanyId.Value.ToString() : null;

            //        var appShell = (AppShell)Shell.Current;
            //        appShell.AddMenu_QuanLyCongTy();
            //        appShell.AddMenu_QuanLyMoiGioi();
            //        await Shell.Current.GoToAsync("//" + AppShell.QUANLYCONGTY);
            //    }
            //    OnCompeleted?.Invoke(true);
            //}
            //else if (notification.NotificationType == NotificationType.UpdateVersion)
            //{
            //    var result = await Shell.Current.DisplayAlert("Cập nhật", "Cập nhật ứng dụng để trải nghiệm tính năng mới", "Cập nhật", Language.dong);
            //    if (result)
            //    {
            //        DependencyService.Get<IOpenApp>().OpenAppStore();
            //    }
            //    OnCompeleted?.Invoke(true);
            //}
        }
    }
}
