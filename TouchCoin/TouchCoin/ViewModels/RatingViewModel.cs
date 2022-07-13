using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TouchCoin.Models;
using TouchCoin.View;
using Xamarin.Forms;

namespace TouchCoin.ViewModels
{
    public class RatingViewModel : BaseViewModel
    {
        public Command LoadItemsCommand { get; }
        public ObservableCollection<RatingModel> Items { get; private set; }

        private string _userPlace;
        private double _userCoin;

        private bool _isBusy = false;

        public RatingViewModel()
        {
            Items = new ObservableCollection<RatingModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public string UserPlace
        {
            get { return _userPlace; }
            set { SetProperty(ref _userPlace, value); }
        }

        public double UserCoin
        {
            get { return _userCoin; }
            set { SetProperty(ref _userCoin, value); }
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            Items.Clear();
            try
            {
                if (string.IsNullOrEmpty(UserPlace))
                    UserPlace = "?";

                UserCoin = (double)App.Data.Coin / 1000;

                Items = await App.HttpService.GetUser();

                foreach (var item in Items)
                {
                    if (item.NikName != App.Data.NikName)
                        continue;

                    UserPlace = item.Place.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage = new ErrorPage(ex);
            }

            OnPropertyChanged("");
            IsBusy = false;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}
