using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Models;
using CustomerApp.Settings;
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
                                      TitleEn = item.Object.TitleEn,
                                      Body = item.Object.Body,
                                      BodyEn = item.Object.BodyEn,
                                      ProjectId = item.Object.ProjectId,
                                      NotificationType = item.Object.NotificationType,
                                      IsRead = item.Object.IsRead,
                                      CreatedDate = item.Object.CreatedDate,
                                  }).OrderByDescending(x=>x.CreatedDate);
            foreach (var item in Items)
            {
                item.Title = UserLogged.Language == "vi" ? item.Title : item.TitleEn;
                item.Body = UserLogged.Language == "vi" ? item.Body : item.BodyEn;
                this.Notifications.Add(item);
            }
        }

        public async Task UpdateStatus(string key, NotificaModel data)
        {
            await firebaseClient.Child("Notifications").Child(key).PutAsync(new NotificaModel() { Id = data.Id, Title = data.Title, TitleEn = data.TitleEn, Body = data.Body, BodyEn = data.BodyEn, ProjectId = data.ProjectId,IsRead= true,NotificationType = data.NotificationType,CreatedDate=data.CreatedDate });
        }
    }
}
