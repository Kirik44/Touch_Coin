using System;
using System.Threading.Tasks;
using TouchCoin.View.AuthPage;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TouchCoin.Models
{
    public class AppData
    {
        public string NikName { get; private set; }
        public string Pass { get; private set; }
        public ulong Coin { get; private set; }
        public byte[] QuantityMining { get; private set; }
        public bool IsDataCreated { get; private set; }

        private byte _ticktimervalue = 0;

        public AppData()
        {
            QuantityMining = new byte[7];

            NikName = Preferences.Get("NikName", string.Empty);
            Pass = Preferences.Get("Pass", string.Empty);

            IsDataCreated = CheckUserData();
        }

        public bool CheckUserData()
        {
            if (string.IsNullOrEmpty(NikName) || string.IsNullOrEmpty(Pass))
                return false;

            if (NikName.Length < 4 || Pass.Length < 8)
                return false;

            return true;
        }

        public bool CheckUserData(string NikName, string Pass)
        {
            if (string.IsNullOrEmpty(NikName) || string.IsNullOrEmpty(Pass))
                return false;

            if (NikName.Length < 4 || Pass.Length < 8)
                return false;

            return true;
        }

        public async Task<bool> Login()
        {
            if (!IsDataCreated)
                return false;

            bool result = await App.HttpService.Login(NikName, Pass);
            if (result)
                return true;

            return false;
        }

        public void Logout()
        {
            Preferences.Set("NikName", string.Empty);
            Preferences.Set("Pass", string.Empty);

            NikName = string.Empty;
            Pass = string.Empty;
            Coin = 0;
            QuantityMining = new byte[7];
            IsDataCreated = false;

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }

        public bool SetHttpData(UserData userData)
        {
            if (!CheckUserData(userData.NikName, userData.Pass))
                throw new ArgumentException("Error data (User Data)");

            if (userData.Coin < 0)
                throw new ArgumentException("Error data (Coin value)");

            Coin = userData.Coin;

            byte[] value = new byte[7];

            value[0] = userData.LvlClick;
            value[1] = userData.LvlMining1;
            value[2] = userData.LvlMining2;
            value[3] = userData.LvlMining3;
            value[4] = userData.LvlMining4;
            value[5] = userData.LvlMining5;
            value[6] = userData.LvlMining6;

            foreach (byte level in value)
            {
                if (level < 0 || level > 50)
                    throw new ArgumentException("Error level mining");
            }

            QuantityMining = value;

            NikName = userData.NikName;
            Pass = userData.Pass;
            Preferences.Set("NikName", NikName);
            Preferences.Set("Pass", Pass);
            IsDataCreated = true;

            return true;
        }

        public UserData GetHttpData()
        {
            UserData userData = new UserData()
            {
                NikName = NikName,
                Pass = Pass,
                Coin = Coin,
                LvlClick = QuantityMining[0],
                LvlMining1 = QuantityMining[1],
                LvlMining2 = QuantityMining[2],
                LvlMining3 = QuantityMining[3],
                LvlMining4 = QuantityMining[4],
                LvlMining5 = QuantityMining[5],
                LvlMining6 = QuantityMining[6],
            };

            return userData;
        }

        public ulong GetCoinTickProfitability()
        {
            ulong tickCoin = 0;
            byte i = 0;
            foreach (var item in QuantityMining)
            {
                i++;
                if (i - 1 == 0)
                    continue;
                if (item == 0)
                    continue;

                tickCoin += (ulong)App.MiningParameter.Parameter[i - 1].Profitability * item;
            }

            return tickCoin;
        }

        public double Click()
        {
            if (!IsDataCreated)
                throw new InvalidOperationException(nameof(Click));
            Coin += QuantityMining[0];
            double coin = (double)QuantityMining[0] / 1000;
            return coin;
        }

        public double MiningTick()
        {
            if (!IsDataCreated)
                return 0;

            _ticktimervalue++;

            if (_ticktimervalue > 10)
            {
                var task = Task.Run(async () => await SendData());
            }

            ulong coin = GetCoinTickProfitability();

            if (coin <= 0)
                return 0;

            Coin += coin;
            return (double)coin / 1000;
        }

        public async Task SendData()
        {
            if (!IsDataCreated)
                throw new Exception(message: "Пользователь не авторизован!");

            _ticktimervalue = 0;
            await App.HttpService.SendData(GetHttpData());
        }

        public void ByuMining(int level)
        {
            if (level < 0 || level > (QuantityMining.Length - 1))
                throw new ArgumentOutOfRangeException();

            var paramsbyumining = App.MiningParameter.Parameter[level];

            if (paramsbyumining.Id != level)
                throw new Exception(message: "Произошла ошибка, пожалуйста, перезапустите приложение");

            if (paramsbyumining.MaxQuantity <= QuantityMining[paramsbyumining.Id])
                throw new Exception(message: "Максимальный уровень улучшения");

            ulong costnextlevel = (ulong)Math.Round(paramsbyumining.DefaultCost * Math.Pow(App.MiningParameter.FactorCost, QuantityMining[paramsbyumining.Id]));

            if (Coin < costnextlevel)
                throw new Exception(message: "Нужно больше Tcoin");

            Coin -= costnextlevel;
            QuantityMining[paramsbyumining.Id]++;

            var task = Task.Run(async () => await SendData());
        }
    }
}
