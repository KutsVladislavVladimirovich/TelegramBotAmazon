using System;

namespace TelegramBotFramework.ApiActions.ApiUrls
{
    internal static class ApiUrl
    {
        internal static string KursComUa => "https://kurs.com.ua/valyuta/usd";

        internal static string MeteoUa(City city) => 
            $"https://meteo.ua/{(city == City.Dnipro ? Dnipro : city == City.Novodonetskoe ? Novodonetskoe : throw new Exception($"City with name {city} does not implemented."))}";

        internal static string PrivatBank => "https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5";

        private static string Dnipro => "164/dnepr-dnepropetrovsk";
        private static string Novodonetskoe => "358/novodonetskoe";

        internal enum City
        {
            Dnipro,
            Novodonetskoe
        }
    }
}