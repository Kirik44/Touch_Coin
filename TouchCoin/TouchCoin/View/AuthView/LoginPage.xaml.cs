using Android.Widget;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View.AuthPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        const string nikpattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void ClicedToRegister(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void GoLogin(object sender, EventArgs e)
        {
            string nikname = Nick.Text?.Trim();
            string pass = Pass.Text?.Trim();

            if (string.IsNullOrEmpty(nikname) || nikname.Length < 4)
            {
                Toast.MakeText(Android.App.Application.Context, "Ошибка, длинна никнейма должна составлять 4 и более символа!", ToastLength.Short).Show();
                return;
            }

            if (string.IsNullOrEmpty(pass) || pass.Length < 8)
            {
                Toast.MakeText(Android.App.Application.Context, "Ошибка, пароль нолжен иметь не менее 8-ми символов!", ToastLength.Short).Show();
                return;
            }

            try
            {
                LoadIndicator.IsRunning = true;
                BtnLogin.IsEnabled = false;

                bool result = await App.HttpService.Login(nikname, pass);

                if (result)
                {
                    Application.Current.MainPage = new NavigationPage(new MainPage());
                    return;
                }

                Toast.MakeText(Android.App.Application.Context, "Произошла неизвестная ошибка, перезагрузите приложение и попробуйте ещё раз.", ToastLength.Short).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.Message.ToString(), ToastLength.Short).Show();
            }
            finally
            {
                LoadIndicator.IsRunning = false;
                BtnLogin.IsEnabled = true;
            }
        }
    }
}