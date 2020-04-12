using Newtonsoft.Json;

namespace TelegramBotFramework.Models
{
    internal class PrivatBankModel
    {
        [JsonProperty("ccy")]
        internal string Ccy { get; set; }
        [JsonProperty("base_ccy")]
        internal string BaseCcy { get; set; }
        [JsonProperty("buy")]
        internal decimal Buy { get; set; }
        [JsonProperty("sale")]
        internal decimal Sale { get; set; }
    }
}