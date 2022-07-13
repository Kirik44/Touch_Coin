using System;
using System.Threading.Tasks;
using TouchCoin.Models;
using TouchCoin.Service;
using TouchCoin.View;
using TouchCoin.View.AuthPage;
using TouchCoin.View.TrainingPage;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TouchCoin
{
    public partial class App : Application
    {
        public static AppData Data { get; set; } = new AppData();
        public static HttpService HttpService { get; set; } = new HttpService();
        public static MiningParameters MiningParameter { get; set; } = new MiningParameters();

        public App()
        {
            InitializeComponent();

            if (Preferences.Get("IsFirstLaunch", true))
            {
                MainPage = new NavigationPage(new TrainingPage1());
                return;
            }

            try
            {
                var t = Task.Run(async () => await Data.Login());
                bool result = t.Result;

                if (result)
                {
                    MainPage = new NavigationPage(new MainPage());
                    return;
                }

                MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                MainPage = new ErrorPage(ex);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
