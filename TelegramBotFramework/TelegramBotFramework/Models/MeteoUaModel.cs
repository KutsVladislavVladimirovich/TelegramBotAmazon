namespace TelegramBotFramework.Models
{
    internal class MeteoUaModel
    {
        internal string MetaInfo => $"\r\n{this.Cloud}🖼\r\nВетер - {this.Wind}🌪\r\n\r\nНочь - {this.Temperature.Night}🌚\r\nУтро - {this.Temperature.Morning}🌏\r\nДень - {this.Temperature.Day}🌝\r\nВечер - {this.Temperature.Evening}🌓\r\n\r\nСейчас - {this.Now}✅";
        internal Temperature Temperature { get; set; }
        internal string Now { get; set; }
        internal string Cloud { get; set; }
        internal string Wind { get; set; }
    }

    internal class Temperature
    {
        internal string Night { get; set; }
        internal string Morning { get; set; }
        internal string Day { get; set; }
        internal string Evening { get; set; }
    }
}