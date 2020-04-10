using System.Net;
using System.Text.RegularExpressions;
using TelegramBotFramework.ApiActions.ApiUrls;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.ApiActions.Weather
{
    public class Gismeteo
    {
        private readonly WebClient _webClient = new WebClient();
        public MeteoUaModel GetWeatherInfo()
        {
            var page = _webClient.DownloadString(ApiUrl.MeteoUa);
            var temperatures = Regex.Split(page, @"(\+\d{1,2}|\-d{1,2})&deg;");

            return new MeteoUaModel
            {
                Night = temperatures[11],
                Morning = temperatures[13],
                Day = temperatures[15],
                Evening = temperatures[17],
            };
        }
    }
}