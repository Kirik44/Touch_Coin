using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View.AuthPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
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

                bool result = await App.HttpService.Register(nikname, pass);

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

        //private async void BtneLogin(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        bool Error = false;

        //        if (!string.IsNullOrEmpty(ErrorMailMess.Text))
        //            ErrorMailMess.Text = "";
        //        if (!string.IsNullOrEmpty(ErrorPassMess.Text))
        //            ErrorPassMess.Text = "";

        //        if (string.IsNullOrEmpty(Nick.Text) || Nick.Text.Length < 4)
        //        {
        //            ErrorMailMess.Text = "Ошибка! Mail не может быть короче 4 символов.";
        //            Nick.Text = "";
        //            Error = true;
        //        }
        //        if (string.IsNullOrEmpty(Pass.Text) || Pass.Text.Length < 8)
        //        {
        //            ErrorPassMess.Text = "Ошибка! Длинна пароля должна составлять не менее 8 символов.";
        //            Pass.Text = "";
        //            Error = true;
        //        }
        //        if (Error) return;

        //        bool result = await App.HttpService.Register(Nick.Text, Pass.Text);

        //        if (result)
        //        {
        //            Application.Current.MainPage = new NavigationPage(new MainPage());
        //            return;
        //        }

        //        await DisplayAlert("Ошибка!", "", "ОK");
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Ошибка!", ex.Message, "ОK");
        //    }
        //}
    }
}