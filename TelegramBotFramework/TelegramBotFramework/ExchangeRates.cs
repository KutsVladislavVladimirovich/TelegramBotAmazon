using System.Text.RegularExpressions;
using TelegramBotFramework.Models;

namespace TelegramBotFramework
{
    public class ExchangeRates
    {
        private readonly System.Net.WebClient _webClient = new System.Net.WebClient();
        public KursComUaModel GetRatesFromKursComUa()
        {
            var page = _webClient.DownloadString(new ApiUrls().KursComUa());
            var matches = Regex.Split(page, @"course_first"">(.*)<div");
            //var matches2 = Regex.Split(page, @"""csvw:value"" : ""(.*)"",");

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