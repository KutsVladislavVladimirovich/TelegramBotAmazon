using System;

namespace TelegramBotFramework
{
    internal class ApiUrls
    {
        /// <summary>
        /// Get url for send request.
        /// </summary>
        /// <param name="date">dd.MM.yyyy</param>
        /// <returns></returns>
        internal string BankGovUa() =>
            $"https://bank.gov.ua/markets/exchangerates/?date={DateTime.Now.ToString("dd.MM.yyyy")}&period=daily";

        internal string KursComUa() => "https://kurs.com.ua/valyuta/usd";
    }
}