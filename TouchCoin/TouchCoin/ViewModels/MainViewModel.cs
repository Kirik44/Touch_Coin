using System;
using System.Collections.ObjectModel;
using TouchCoin.Models;
using Xamarin.Forms;

namespace TouchCoin.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Command BtnCoinClicked { get; private set; }
        public Command SwitchByuMenu { get; private set; }

        public ObservableCollection<MiningItem> UpdateItems { get; private set; }
        public ObservableCollection<MiningItem> MiningItems { get; private set; }

        private bool _byumenustate = false;
        private double _cointic = 0;
        private double _coin = 0;

        public MainViewModel()
        {
            BtnCoinClicked = new Command(() => CoinClicked());
            SwitchByuMenu = new Command(() => SwitchMenu());

            UpdateItems = new ObservableCollection<MiningItem>();
            MiningItems = new ObservableCollection<MiningItem>();

            Device.StartTimer(TimeSpan.FromSeconds(1), MiningTick);

            Update();
        }

        public bool ByuMenuState
        {
            get { return _byumenustate; }
            set { SetProperty(ref _byumenustate, value); }
        }
        public double TicCoin
        {
            get { return _cointic; }
            set { SetProperty(ref _cointic, value); }
        }
        public double Coin
        {
            get { return _coin; }
            set { SetProperty(ref _coin, value); }
        }

        private void SwitchMenu() => ByuMenuState = !ByuMenuState;
        private void CoinClicked() => Coin += App.Data.Click();
        private bool MiningTick()
        {
            Coin += App.Data.MiningTick();
            return true;
        }

        public void Update()
        {
            UpdateData();
            UpdateMiningItem();
        }

        private void UpdateData()
        {
            Coin = (double)App.Data.Coin / 1000;
            TicCoin = (double)App.Data.GetCoinTickProfitability() / 1000;
        }

        private void UpdateMiningItem()
        {
            UpdateItems.Clear(); 
            foreach (var item in App.MiningParameter.Parameter)
            {
                int _id = item.Id;
                string _name = item.Name;
                double _profitability = (double)item.Profitability / 1000;
                double _cost = (double)(Math.Round(item.DefaultCost * Math.Pow(App.MiningParameter.FactorCost, App.Data.QuantityMining[_id])) / 1000);
                byte _value = App.Data.QuantityMining[_id];
                string _imgscr = item.ImgName;

                UpdateItems.Add(new MiningItem() { Id = _id, Value = _value, Cost = _cost, Name = _name, Profitability = _profitability, ImgScr = _imgscr });
            }

            MiningItems.Clear();
            foreach (var item in UpdateItems)
            {
                if (item.Value == 0)
                    continue;

                MiningItems.Add(item);
            }
        }

        public void OnAppearing()
        {
            Update();
        }
    }
}
