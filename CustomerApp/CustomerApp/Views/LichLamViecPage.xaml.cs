using System;
using System.Collections.Generic;
using CustomerApp.Helper;
using CustomerApp.Models;
using CustomerApp.Resources;
using Xamarin.Forms;

namespace CustomerApp.Views
{
    public partial class LichLamViecPage : ContentPage
    {
        public List<OptionSet> data { get; set; }
        public LichLamViecPage()
        {
            InitializeComponent();
            this.BindingContext = this;
            Init();
        }

        public async void Init()
        {
            data = new List<OptionSet>() { new OptionSet("1", Language.lich_lam_viec_theo_thang), new OptionSet("2", Language.lich_lam_viec_theo_tuan), new OptionSet("3", Language.lich_lam_viec_theo_tuan), };
            listView.ItemsSource = data;
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            OptionSet item = e.Item as OptionSet;
            if (item.Val == "1")
            {
                LoadingHelper.Show();
                LichLamViecTheoThangPage lichLamViecTheoThang = new LichLamViecTheoThangPage();
                lichLamViecTheoThang.OnComplete = async (OnComplete) =>
                {
                    if (OnComplete == true)
                    {
                        await Navigation.PushAsync(lichLamViecTheoThang);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_lich_lam_viec);
                    }
                };
            }
            else if (item.Val == "2")
            {
                LoadingHelper.Show();
                try
                {
                    LichLamViecTheoTuanPage lichLamViecTheoTuan = new LichLamViecTheoTuanPage();
                    lichLamViecTheoTuan.OnCompleted = async (isSuccess) =>
                    {
                        if (isSuccess == true)
                        {
                            await Navigation.PushAsync(lichLamViecTheoTuan);
                            LoadingHelper.Hide();
                        }
                        else
                        {
                            LoadingHelper.Hide();
                            ToastMessageHelper.ShortMessage(Language.khong_tim_thay_lich_lam_viec);
                        }
                    };
                }
                catch(Exception ex)
                {
                    LoadingHelper.Hide();
                }
                
            }
            else if (item.Val == "3")
            {
                LoadingHelper.Show();
                LichLamViecTheoNgayPage lichLamViecTheoNgay = new LichLamViecTheoNgayPage();
                lichLamViecTheoNgay.OnCompleted = async (IsSuccess) =>
                {
                    if (IsSuccess == true)
                    {
                        await Navigation.PushAsync(lichLamViecTheoNgay);
                        LoadingHelper.Hide();
                    }
                    else
                    {
                        LoadingHelper.Hide();
                        ToastMessageHelper.ShortMessage(Language.khong_tim_thay_lich_lam_viec);
                    }
                };
            }
        }
    }
}
