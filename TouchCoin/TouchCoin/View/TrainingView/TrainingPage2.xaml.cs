using System;
using TouchCoin.View.AuthPage;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View.TrainingPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage2 : ContentPage
    {
        public TrainingPage2()
        {
            InitializeComponent();
        }

        private void GoMain(object sender, EventArgs e)
        {
            Preferences.Set("IsFirstLaunch", false);
            if (App.Data.IsDataCreated)
            {
                Application.Current.MainPage = new NavigationPage(new MainPage());
                return;
            }

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}