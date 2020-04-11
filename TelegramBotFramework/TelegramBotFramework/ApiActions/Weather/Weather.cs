using System.Net;
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

            return new MeteoUaModel
            {
                Now = now.Groups[1].Value,

                Temperature = new Temperature
                {
                    Night = temperatures[11],
                    Morning = temperatures[13],
                    Day = temperatures[15],
                    Evening = temperatures[17],
                }
            };
        }

        internal string GetDniproWeather()
        {
            var dniproWeather = GetWeatherInfo(ApiUrl.City.Dnipro);

            return $"Погода в городе Днепре🌈\r\nСейчас - {dniproWeather.Now}✅\r\nНочь - {dniproWeather.Temperature.Night}🌚\r\nУтро - {dniproWeather.Temperature.Morning}🌏\r\nДень - {dniproWeather.Temperature.Day}🌝\r\nВечер - {dniproWeather.Temperature.Evening}🌓";
        }

        internal string GetNovodonetckoeWeather()
        {
            var novodonetckoeWeather = new Weather().GetWeatherInfo(ApiUrl.City.Novodonetskoe);

            return $"Погода в пгт Новодонецком🌈\r\nСейчас - {novodonetckoeWeather.Now}✅\r\nНочь - {novodonetckoeWeather.Temperature.Night}🌚\r\nУтро - {novodonetckoeWeather.Temperature.Morning}🌏\r\nДень - {novodonetckoeWeather.Temperature.Day}🌝\r\nВечер - {novodonetckoeWeather.Temperature.Evening}🌓";
        }
    }
}