using System;
using CustomerApp.ViewModels;

namespace CustomerApp.Models
{
    public class NotificaModel :BaseViewModel
    {
        public string Key { get; set; }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string Body { get; set; }
        public string BodyEn { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid ProjectId { get; set; }

        private string _backgroundColor;
        public string BackgroundColor { get => _backgroundColor; set { _backgroundColor = value; OnPropertyChanged(nameof(BackgroundColor)); } }
    }
}
