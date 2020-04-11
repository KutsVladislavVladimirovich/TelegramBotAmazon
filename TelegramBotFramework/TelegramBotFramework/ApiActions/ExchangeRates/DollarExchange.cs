using System;
using System.Net;
using System.Text.RegularExpressions;
using TelegramBotFramework.ApiActions.ApiUrls;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.ApiActions.ExchangeRates
{
    internal class DollarExchange
    {
        private readonly WebClient _webClient = new WebClient();
        internal KursComUaModel GetRatesFromKursComUa()
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

        internal string GetDollarInfo()
        {
            var rates = GetRatesFromKursComUa();

            return $"Курс Доллара в Днепре🏧\r\nПокупка - {rates.Buy}\r\nПродажа - {rates.Sale}\r\nКоммерческий - {rates.Commercial}\r\nНациональный Банк Украины - {rates.Nbu}";
        }

        internal string CalculateDollarByDigit(decimal digit)
        {
            var rates = GetRatesFromKursComUa();

            return $"{digit} USD = {digit * Convert.ToDecimal(rates.Nbu.Replace('.', ','))} UAH.";
        }
    }
}