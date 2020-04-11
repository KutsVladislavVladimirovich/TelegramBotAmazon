namespace TelegramBotFramework.Models
{
    internal class MeteoUaModel
    {
        internal Temperature Temperature { get; set; }
        internal string Now { get; set; }

    }

    internal class Temperature
    {
        internal string Night { get; set; }
        internal string Morning { get; set; }
        internal string Day { get; set; }
        internal string Evening { get; set; }
    }
}