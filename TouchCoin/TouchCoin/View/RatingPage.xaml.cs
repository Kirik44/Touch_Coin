using TouchCoin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TouchCoin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RatingPage : ContentPage
    {
        private RatingViewModel _viewModel;

        public RatingPage()
        {
            BindingContext = _viewModel = new RatingViewModel();
            
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}