using ApiAiSDK;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotFramework.ApiActions.ExchangeRates;
using TelegramBotFramework.ApiActions.Weather;

namespace TelegramBotFramework
{
    class Program
    {
        private static ITelegramBotClient bot;
        private static ApiAi apiAi;
        static void Main(string[] args)
        {
            bot = new TelegramBotClient(Constants.BotToken);
            apiAi = new ApiAi(new AIConfiguration(Constants.SmartTalkToken, SupportedLanguage.Russian));

            var me = bot.GetMeAsync().Result;
            Console.WriteLine($"Bot Name - {me.FirstName}.");
            Console.WriteLine($"Bot Username on Telegram - @{me.Username}.");

            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += BotOnCallbackQuery;
            bot.StartReceiving();

            Console.ReadKey();
        }

        private static async void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine($"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName} нажал кнопку {e.CallbackQuery.Data}.");

            if (e.CallbackQuery.Data.Equals("Погода"))


                await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {e.CallbackQuery.Data}.");
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message == null || !e.Message.Type.Equals(MessageType.Text))
                return;

            Console.WriteLine($"Contact with name {e.Message.From.FirstName} {e.Message.From.LastName} and id {e.Message.Chat.Id} send message with text '{e.Message.Text}'.");

            switch (e.Message.Text)
            {
                case "/start":
                    var textForResponse =
@"Список команд:
/start - Запуск бота
/menu - Показать меню
/getscreen - Сделать скрин
/getusd - Курс доллара";
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, textForResponse);
                    break;
                case "/menu":
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("Курс Доллара"),
                        new KeyboardButton("Погода"),
                    }, oneTimeKeyboard: true);

                    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт меню.", replyMarkup: markup);
                    break;
                case "Сделай скрин":
                case "/getscreen":
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, "А всё, теперь нельзя, бот размещен на удалённой машине)");
                    break;
                case "Курс Доллара":
                case "/getusd":
                    var rates = new DollarExchange().GetRatesFromKursComUa();
                    var dollarInfo = $"Курс Доллара🏧\r\nПокупка - {rates.Buy}\r\nПродажа - {rates.Sale}\r\nКоммерческий - {rates.Commercial}\r\nНациональный Банк Украины - {rates.Nbu}";

                    await bot.SendTextMessageAsync(e.Message.Chat.Id, dollarInfo);
                    break;
                case "Погода":
                case "/getweather":
                    var weather = new Gismeteo().GetWeatherInfo();
                    var todayInfo = $"Утро - {weather.Morning}🌏\r\nДень - {weather.Day}🌝\r\nВечер - {weather.Evening}🌓\r\nНочь - {weather.Night}🌚";

                    await bot.SendTextMessageAsync(e.Message.Chat.Id, todayInfo);
                    break;
                default:
                    var response = apiAi.TextRequest(e.Message.Text);
                    var answer = response.Result.Fulfillment.Speech;
                    if (answer.Equals(string.Empty))
                        answer = $"Прости, я тебя не понял, скоро буду отвечать и на {e.Message.Text}";

                    await bot.SendTextMessageAsync(e.Message.Chat.Id, answer);
                    break;
            }
        }
    }
}
