using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View.TrainingPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage1 : ContentPage
    {
        public TrainingPage1()
        {
            InitializeComponent();
        }

        private async void Further(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingPage2());
        }
    }
}