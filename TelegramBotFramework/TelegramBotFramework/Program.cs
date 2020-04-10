using ApiAiSDK;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotFramework
{
    class Program
    {
        private static ITelegramBotClient bot;
        private static ApiAi apiAi;
        static void Main(string[] args)
        {
            //var dollarInfo = new ExchangeRates().GetRatesFromKursComUa();

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

            await bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {e.CallbackQuery.Data}.");

            ScreenshotFeature.MakeScreenshot(Constants.SreenshotName);
            if (!e.CallbackQuery.Message.Chat.Id.Equals(Constants.AdminId))
                using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
                {
                    var inputMedia = new InputMedia(stream, stream.Name);
                    await bot.SendPhotoAsync(Constants.AdminId, inputMedia);
                }

            using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
            {
                var inputMedia = new InputMedia(stream, stream.Name);
                await bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, inputMedia);
            }
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
/getScreen - сделать скрин";
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, textForResponse);
                    break;
                case "/menu":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Сделай скриншот!")
                        }
                    });
                    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт меню.", replyMarkup: inlineKeyboard);
                    break;
                case "Сделай скрин":
                case "/getScreen":
                    ScreenshotFeature.MakeScreenshot(Constants.SreenshotName);
                    if (!e.Message.Chat.Id.Equals(Constants.AdminId))
                        using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
                        {
                            var inputMedia = new InputMedia(stream, stream.Name);
                            await bot.SendPhotoAsync(Constants.AdminId, inputMedia);
                        }

                    using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
                    {
                        var inputMedia = new InputMedia(stream, stream.Name);
                        await bot.SendPhotoAsync(e.Message.Chat.Id, inputMedia);
                    }
                    await bot.SendTextMessageAsync(Constants.AdminId, $"Пользователь с id - {e.Message.Chat.Id} и именем {e.Message.From.FirstName} {e.Message.From.LastName} получил скриншот вашего рабочего стола.");
                    break;
                case "/getusd":
                    var rates = new ExchangeRates().GetRatesFromKursComUa();
                    var dollarInfo = $"Курс Доллара: Продажа - {rates.Sale}. Покупка - {rates.Buy}. Коммерческий - {rates.Commercial}. Национальный Банк Украины - {rates.Nbu}.";

                    await bot.SendTextMessageAsync(e.Message.Chat.Id, dollarInfo);
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
