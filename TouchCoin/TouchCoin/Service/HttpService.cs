using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using TouchCoin.Models;
using TouchCoin.View;
using Xamarin.Forms;

namespace TouchCoin.Service
{
    public class HttpService
    {
        private static readonly HttpClient HttpClient;
        private const string key = "Secret key";

        static HttpService()
        {
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri("Uri")
            };
            HttpClient.DefaultRequestHeaders.Add("KEY", key);
        }

        public async Task<ObservableCollection<RatingModel>> GetUser()
        {
            var response = await HttpClient.GetAsync("user");

            if (!response.IsSuccessStatusCode)
                throw new Exception(message: $"Ошибка загрузки рейтинга\n" + response.StatusCode.ToString());

            var result = await response.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<List<RatingModel>>(result.ToString());

            ObservableCollection<RatingModel> RatingList = new ObservableCollection<RatingModel>();

            int place = 1;
            foreach (var item in userList)
            {
                RatingList.Add(new RatingModel() { Place = place, NikName = item.NikName, Coin = item.Coin / 1000 });
                place++;
            }

            return RatingList;
        }

        public async Task SendData(UserData userData)
        {
            try
            {
                var jsonData = JsonConvert.SerializeObject(userData);
                var response = await HttpClient.PostAsync("data", new StringContent(jsonData, null, "application/json"));

                if (!response.IsSuccessStatusCode)
                    throw new Exception(message: response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                Application.Current.MainPage = new ErrorPage("Произошла ошибка при отправке данных.\nMessage: " + ex.Message.ToString());
            }
        }

        public async Task<bool> Login(string NikName, string Pass)
        {
            var data = new { NikName, Pass };
            var jsonData = JsonConvert.SerializeObject(data);

            var response = await HttpClient.PostAsync("login", new StringContent(jsonData, null, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var statusCode = response.StatusCode;

            switch (statusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    JObject jsonResult = JObject.Parse(result);
                    var userdata = JsonConvert.DeserializeObject<UserData>(jsonResult.ToString());
                    return App.Data.SetHttpData(userdata);

                case System.Net.HttpStatusCode.BadRequest:
                    throw new ArgumentException(result);

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new Exception(message: "Пользователь не найден");

                default:
                    throw new Exception($"Произошла неизвестная ошибка на сервере\nHttp Status Code: " + statusCode.ToString() + ",\nMessage: " + result);
            }
        }

        public async Task<bool> Register(string NikName, string Pass)
        {
            var data = new { NikName, Pass };
            var jsonData = JsonConvert.SerializeObject(data);

            var response = await HttpClient.PostAsync("user", new StringContent(jsonData, null, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var statusCode = response.StatusCode;

            switch (statusCode)
            {
                case System.Net.HttpStatusCode.Created:
                    JObject jsonResult = JObject.Parse(result);
                    var userdata = JsonConvert.DeserializeObject<UserData>(jsonResult?.ToString());
                    return App.Data.SetHttpData(userdata);

                case System.Net.HttpStatusCode.NotAcceptable:
                    throw new Exception($"Пользователь с таким именем уже существует\n" + result + "\nHttp Status Code: " + statusCode.ToString());

                case System.Net.HttpStatusCode.BadRequest:
                    throw new Exception("Проверьте правильность написанных данных");

                case System.Net.HttpStatusCode.Unauthorized:
                    throw new Exception("Ник занят");

                default:
                    throw new Exception($"Произошла неизвестная ошибка на сервере\nHttp Status Code: " + statusCode.ToString() + ",\nMessage: " + result);
            }
        }
    }
}
