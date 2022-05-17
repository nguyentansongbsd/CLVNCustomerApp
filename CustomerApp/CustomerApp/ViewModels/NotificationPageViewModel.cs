using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Models;
using Firebase.Database;
using Firebase.Database.Query;

namespace CustomerApp.ViewModels
{
    public class NotificationPageViewModel : BaseViewModel
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://smsappcrm-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult("kLHIPuBhEIrL6s3J6NuHpQI13H7M0kHjBRLmGEPF") });

        public ObservableCollection<NotificaModel> Notifications { get; set; } = new ObservableCollection<NotificaModel>();

        public NotificationPageViewModel()
        {
        }

        public async Task LoadData()
        {
            var Items = (await firebaseClient
                                  .Child("Notifications")
                                  .OnceAsync<NotificaModel>()).Select(item => new NotificaModel
                                  {
                                      Key = item.Key,
                                      Id = item.Object.Id,
                                      Title = item.Object.Title,
                                      Body = item.Object.Body,
                                      ProjectId = item.Object.ProjectId,
                                      IsRead = item.Object.IsRead,
                                      CreatedDate = item.Object.CreatedDate,
                                  }).OrderByDescending(x=>x.CreatedDate);
            foreach (var item in Items)
            {
                if (item.IsRead)
                {
                    item.BackgroundColor = "#F2F2F2";
                }
                else
                {
                    item.BackgroundColor = "#ffffff";
                }
                this.Notifications.Add(item);
            }
        }

        public async Task UpdateStatus(string key, NotificaModel data)
        {
            await firebaseClient.Child("Notifications").Child(key).PutAsync(new NotificaModel() { Id = data.Id, Title = data.Title,Body = data.Body,ProjectId = data.ProjectId,IsRead= true,CreatedDate=data.CreatedDate });
        }
    }
}
