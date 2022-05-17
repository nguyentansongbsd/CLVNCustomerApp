using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerApp.Models;

namespace CustomerApp.IServices
{
    public interface INotificationService
    {
        Task<string> SaveToken();
        Task SendNotification(NotificaModel model,string token);
        Task HandleTapNotification(NotificaModel model, Action<bool> OnCompleted = null);
    }
}