using System;

namespace TelegramBotFramework.ApiActions.ApiUrls
{
    internal static class ApiUrl
    {
        /// <summary>
        /// Get url for send request.
        /// </summary>
        /// <param name="date">dd.MM.yyyy</param>
        /// <returns></returns>
        internal static string BankGovUa() =>
            $"https://bank.gov.ua/markets/exchangerates/?date={DateTime.Now.ToString("dd.MM.yyyy")}&period=daily";

        internal static string KursComUa() => "https://kurs.com.ua/valyuta/usd";

        internal static string GismeteoDnipro() => "https://www.gismeteo.ua/weather-dnipro-5077/";

        internal static string O56Weather => "https://www.056.ua/weather";

        internal static string MeteoUa => "https://meteo.ua/164/dnepr-dnepropetrovsk";
    }
}