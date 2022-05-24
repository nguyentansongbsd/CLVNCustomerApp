﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.Helper;
using CustomerApp.IServices;
using CustomerApp.Models;
using CustomerApp.Resources;
using Firebase.Database;
using Firebase.Database.Query;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CustomerApp
{
    public partial class BlankPage : ContentPage
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://smsappcrm-default-rtdb.asia-southeast1.firebasedatabase.app/",
            new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult("kLHIPuBhEIrL6s3J6NuHpQI13H7M0kHjBRLmGEPF") });
        FirebaseStorageHelper storageHelper = new FirebaseStorageHelper();

        private List<ProjectList> _projects;
        public List<ProjectList> Projects { get => _projects; set { _projects = value; OnPropertyChanged(nameof(Projects)); } }
        private ProjectList _project;
        public ProjectList Project { get => _project; set { _project = value;OnPropertyChanged(nameof(Project)); } }

        public Stream Stream { get; set; }

        public string FileName { get; set; }

        public BlankPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            Init();
        }

        public async void Init()
        {
            PreOpen();
        }

        public void PreOpen()
        {
            lookupDuAn.PreOpenAsync = async () => {
                await LoadProject();
            };
        }

        public async Task LoadProject()
        {
            string fetchXml = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                <entity name='bsd_project'>
                                    <attribute name='bsd_projectid'/>
                                    <attribute name='bsd_projectcode'/>
                                    <attribute name='bsd_name'/>
                                    <attribute name='createdon' />
                                    <order attribute='bsd_name' descending='false' />
                                    <filter type='and'>
                                      <condition attribute='statuscode' operator='eq' value='861450002' />
                                    </filter>
                                  </entity>
                            </fetch>";
            var result = await CrmHelper.RetrieveMultiple<RetrieveMultipleApiResponse<ProjectList>>("bsd_projects", fetchXml);
            if (result == null || result.value.Any() == false) return;

            this.Projects = result.value;
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                var Tokens = (await firebaseClient
                                  .Child("NotificationToken")
                                  .OnceAsync<TokenModel>()).Select(item => new TokenModel() {

                                      Token = item.Object.Token
                                  });

                string fileName = FileName.Split('.')[0];

                NotificaModel model = new NotificaModel()
                {
                    Id = Guid.NewGuid(),
                    Title = "Tiến độ dự án",
                    Body = $"Tiến độ dự án về \"Thi công phần hoàn thiện\" của dự án \"Feliz En Vista\" đã được cập nhật thêm hình ảnh/ video \"{fileName}\" mới.",
                    ProjectId = Guid.Parse(this.Project.bsd_projectid),
                    NotificationType = NotificationType.ConstructionProgess,
                    CreatedDate = DateTime.Now,
                    IsRead = false
                };

                var a = await firebaseClient.Child("Notifications").PostAsync(model);
                model.Key = a.Key;

                await sendNoti(model, Tokens);
            }
            catch(Exception ex)
            {

            }
        }

        private async Task sendNoti(NotificaModel model,IEnumerable<TokenModel> data)
        {
            foreach (var item in data)
            {
                await DependencyService.Get<INotificationService>().SendNotification(model, item.Token);
            }
        }

        async void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            //var a = firebaseClient.Child("ATC").PostAsync(new CollectionData()
            //{
            //    Id = Guid.NewGuid(),
            //    ImageSource = "song.jng",
            //    SharePointType = SharePointType.Image,
            //    MediaSourceId = null,
            //    Name = "song",
            //    Index = 0,
            //    Thumnail = "song"
            //});
            string[] options = new string[] { Language.thu_vien, Language.chup_hinh };
            string asw = await DisplayActionSheet(Language.tuy_chon, Language.huy, null, options);
            if (asw == Language.thu_vien)
            {
                LoadingHelper.Show();
                await CrossMedia.Current.Initialize();
                PermissionStatus photostatus = await PermissionHelper.RequestPhotosPermission();
                if (photostatus == PermissionStatus.Granted)
                {
                    try
                    {
                        var pickerFile = await FilePicker.PickAsync(new PickOptions()
                        {
                            FileTypes = FilePickerFileType.Images,
                            PickerTitle = "Chọn File",
                        });
                        if (pickerFile != null)
                        {
                            LoadingHelper.Show();
                            FileName = pickerFile.FileName;
                            this.Stream = await pickerFile.OpenReadAsync();

                            var isSuccess = await storageHelper.UploadFile(this.Stream, Project.bsd_projectcode, "images", pickerFile.FileName);
                            if (!string.IsNullOrWhiteSpace(isSuccess))
                            {
                                string fileName = FileName.Split('.')[0];
                                var a = firebaseClient.Child(this.Project.bsd_projectcode).PostAsync(new CollectionData()
                                {
                                    Id = Guid.NewGuid(),
                                    ImageSource = pickerFile.FileName,
                                    SharePointType = SharePointType.Image,
                                    MediaSourceId = null,
                                    Name = fileName,
                                    Index = 0,
                                    Thumnail = "",
                                    GroupName = "",
                                    GroupId = "",
                                    ParentId = "3",
                                    IsGroup = false
                                });
                                ToastMessageHelper.ShortMessage(Language.noti_thanh_cong);
                            }
                            LoadingHelper.Hide();
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        ToastMessageHelper.LongMessage(ex.Message);
                        LoadingHelper.Hide();
                    }
                }
                LoadingHelper.Hide();
            }
            //else if (asw == Language.chup_hinh)
            //{
            //    LoadingHelper.Show();
            //    await CrossMedia.Current.Initialize();
            //    PermissionStatus camerastatus = await PermissionHelper.RequestCameraPermission();
            //    if (camerastatus == PermissionStatus.Granted)
            //    {
            //        string fileName = $"{Guid.NewGuid()}.jpg";
            //        var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            //        {
            //            Name = fileName,
            //            SaveToAlbum = false,
            //            PhotoSize = PhotoSize.MaxWidthHeight,
            //            MaxWidthHeight = 600
            //        });
            //        if (file != null)
            //        {

            //        }
            //    }
            //    LoadingHelper.Hide();
            //}
        }
    }
}
