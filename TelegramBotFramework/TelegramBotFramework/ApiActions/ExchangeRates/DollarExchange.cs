using System.Net;
using System.Text.RegularExpressions;
using TelegramBotFramework.ApiActions.ApiUrls;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.ApiActions.ExchangeRates
{
    public class DollarExchange
    {
        private readonly WebClient _webClient = new WebClient();
        public KursComUaModel GetRatesFromKursComUa()
        {
            var page = _webClient.DownloadString(ApiUrl.KursComUa());
            var matches = Regex.Split(page, @"course_first"">(.*)<div");

            return new KursComUaModel
            {
                Sale = matches[1],
                Buy = matches[3],
                Commercial = matches[5],
                Nbu = matches[7]
            };
        }
    }
}