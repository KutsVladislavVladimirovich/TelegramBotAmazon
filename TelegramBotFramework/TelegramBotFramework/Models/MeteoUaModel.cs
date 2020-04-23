namespace TelegramBotFramework.Models
{
    internal class MeteoUaModel
    {
        internal string MetaInfo => $"\r\nСейчас - {this.Now}✅\r\nВетер - {this.Wind}🌪\r\n\r\nНочь - {this.Temperature.Night}🌚 {this.Cloud.Night}\r\nУтро - {this.Temperature.Morning}🌏 {this.Cloud.Morning}\r\nДень - {this.Temperature.Day}🌝 {this.Cloud.Day}\r\nВечер - {this.Temperature.Evening}🌓 {this.Cloud.Evening}";
        internal Temperature Temperature { get; set; }
        internal string Now { get; set; }
        internal Cloud Cloud { get; set; }
        internal string Wind { get; set; }
    }

    internal class Temperature
    {
        internal string Night { get; set; }
        internal string Morning { get; set; }
        internal string Day { get; set; }
        internal string Evening { get; set; }
    }

    internal class Cloud
    {
        internal string Night { get; set; }
        internal string Morning { get; set; }
        internal string Day { get; set; }
        internal string Evening { get; set; }
    }
}