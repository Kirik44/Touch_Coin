using Android.Widget;
using System;
using TouchCoin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel;

        public MainPage()
        {
            BindingContext = _viewModel = new MainViewModel();

            InitializeComponent();
        }

        private async void GoToRating(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RatingPage());
        }

        private async void GoToAccount(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Настройки", "Name: " + App.Data.NikName + "\n\nВы хотите выйти из аккаунта?", "Да", "Нет");

            if (result)
                App.Data.Logout();
        }

        private async void GoExit(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Выход", "Вы уверены что хотите выйти из игры?", "Да", "Нет");

            if (answer)
            {
                await App.Data.SendData();
                throw new AggregateException("Приложение включено");
            }
        }

        private void BtnByuClicked(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Xamarin.Forms.Button;
                App.Data.ByuMining(int.Parse(btn.CommandParameter.ToString()));
                _viewModel.Update();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.Message, ToastLength.Short).Show();
            }
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
            base.OnAppearing();
        }
    }
}