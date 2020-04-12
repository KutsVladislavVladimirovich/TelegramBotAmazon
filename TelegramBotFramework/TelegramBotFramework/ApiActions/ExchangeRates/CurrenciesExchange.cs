using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using TelegramBotFramework.ApiActions.ApiUrls;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.ApiActions.ExchangeRates
{
    internal class CurrenciesExchange
    {
        private readonly WebClient _webClient = new WebClient();

        internal KursComUaModel GetRatesFromKursComUa()
        {
            var page = _webClient.DownloadString(ApiUrl.KursComUa);
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

            var convertedData = (digit * Convert.ToDecimal(rates.Nbu.Replace('.', ','))).ToString(CultureInfo.InvariantCulture);
            var result = convertedData.Any(s => s.Equals('.')) ? convertedData : convertedData.Insert(convertedData.Length - 3, ".");

            return $"{digit} USD = {result} UAH";
        }

        internal string CalculateCurrency(decimal digit,string ccy)
        {
            ccy = ccy.ToLower();
            var rate = GetPrivatBankExchange().First(r => r.Ccy.ToLower().Equals(ccy));

            var convertedData = $"{rate.Sale * digit}";
            var result = convertedData.Any(s => s.Equals('.'))
                ? convertedData
                : convertedData.Insert(convertedData.Length - 3, ".").Split('.')[0];

            return $"{digit} {ccy.ToUpper()} = {result} {rate.BaseCcy}";
        }

        internal string GetExchangeRates()
        {
            var result = string.Empty;
            var rates = GetPrivatBankExchange();
            foreach (var rate in rates)
            {
                result += $"{rate.Ccy} - {rate.BaseCcy}\r\nПокупа {rate.Buy}\r\nПродажа {rate.Sale}\r\n\r\n";
            }

            return result.TrimEnd();
        }

        private List<PrivatBankModel> GetPrivatBankExchange() =>
            JsonConvert.DeserializeObject<List<PrivatBankModel>>(_webClient.DownloadString(ApiUrl.PrivatBank));
    }
}