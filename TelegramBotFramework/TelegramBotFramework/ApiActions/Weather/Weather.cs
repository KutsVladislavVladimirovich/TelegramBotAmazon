using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TelegramBotFramework.ApiActions.ApiUrls;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.ApiActions.Weather
{
    internal class Weather
    {
        private readonly WebClient _webClient = new WebClient();
        internal MeteoUaModel GetWeatherInfo(ApiUrl.City city)
        {
            var page = _webClient.DownloadString(ApiUrl.MeteoUa(city));
            var temperatures = Regex.Split(page, @"(\+\d{1,2}|\-d{1,2})&deg;");
            var now = Regex.Match(page, @"<div class=""thermometer"" title=""(\+\d{1,2}|\-\d{1,2})&deg;C"" alt=""");
            var cloud = Regex.Split(page, @"<img src=""\/themes\/default\/images\/for_icn\/small\/(\w\d{3}\.(\w{3}))"" title=""(\D*)"">");
            var wind = Regex.Split(page, @"<canvas alt=""(.*)"" title=");

            return new MeteoUaModel
            {
                Now = now.Groups[1].Value,
                Cloud = Encoding.UTF8.GetString(Encoding.Default.GetBytes(cloud[3])),
                Wind = Encoding.UTF8.GetString(Encoding.Default.GetBytes(wind[1])),

                Temperature = new Temperature
                {
                    Night = temperatures[11],
                    Morning = temperatures[13],
                    Day = temperatures[15],
                    Evening = temperatures[17]
                }
            };
        }

        internal string GetDniproWeather()
        {
            var dniproWeather = GetWeatherInfo(ApiUrl.City.Dnipro);

            return $"Погода в городе Днепре🌈\r\n{dniproWeather.MetaInfo}";
        }

        internal string GetNovodonetckoeWeather()
        {
            var novodonetckoeWeather = new Weather().GetWeatherInfo(ApiUrl.City.Novodonetskoe);

            return $"Погода в пгт Новодонецком🌈\r\n{novodonetckoeWeather.MetaInfo}";
        }
    }
}