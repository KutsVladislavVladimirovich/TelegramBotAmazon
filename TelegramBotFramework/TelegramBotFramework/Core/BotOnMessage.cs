using System;
using System.Text.RegularExpressions;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotFramework.ApiActions.ExchangeRates;
using TelegramBotFramework.ApiActions.Weather;
using TelegramBotFramework.Core.DialogFlow;

namespace TelegramBotFramework.Core
{
    internal static class BotOnMessage
    {
        private static readonly Weather Weather = new Weather();
        private static readonly DollarExchange DollarExchange = new DollarExchange();
        private static readonly ApiAiBot ApiAiBot = new ApiAiBot();
        internal static async void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine($"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName} нажал кнопку {e.CallbackQuery.Data}.");

            await Configuration.Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {e.CallbackQuery.Data}.");
            if (Regex.IsMatch(e.CallbackQuery.Data, @"\d{1,15} [U|u][S|s][D|d]"))
                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, DollarExchange.CalculateDollarByDigit(decimal.Parse(e.CallbackQuery.Data.Split(' ')[0])));
        }

        internal static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message == null || !e.Message.Type.Equals(MessageType.Text))
                return;

            Console.WriteLine($"Contact with name {e.Message.From.FirstName} {e.Message.From.LastName} and id {e.Message.Chat.Id} send message with text '{e.Message.Text}'.");

            if (Regex.IsMatch(e.Message.Text, @"\d{1,15} [U|u][S|s][D|d]"))
            {
                await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, DollarExchange.CalculateDollarByDigit(decimal.Parse(e.Message.Text.Split(' ')[0])));
                return;
            }

            switch (e.Message.Text)
            {
                case "/start":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, Constants.Start);
                    break;
                case "/menu":
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("Курс Доллара"),
                        new KeyboardButton("Погода в Днепре"),
                        new KeyboardButton("Погода в Новодонецком"),
                        new KeyboardButton("Конвертировать доллар в гривну")
                    }, oneTimeKeyboard: true);

                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт меню.", replyMarkup: markup);
                    break;
                case "Сделай скрин":
                case "/getscreen":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "А всё, теперь нельзя, бот размещен на удалённой машине)");
                    break;
                case "Курс Доллара":
                case "/getusd":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, DollarExchange.GetDollarInfo());
                    break;
                case "/dollarexchange":
                case "Конвертировать доллар в гривну":
                    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("100 USD"),
                            InlineKeyboardButton.WithCallbackData("700 USD"),
                            InlineKeyboardButton.WithCallbackData("1000 USD"),
                            InlineKeyboardButton.WithCallbackData("1200 USD")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт или введите сумму + USD", replyMarkup: inlineKeyboard);
                    break;
                case "Погода в Днепре":
                case "/getweatherdnipro":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, Weather.GetDniproWeather());
                    break;
                case "Погода в Новодонецком":
                case "/getweathernovodonetskoe":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, Weather.GetNovodonetckoeWeather());
                    break;
                default:
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, ApiAiBot.Speech(e.Message.Text));
                    break;
            }
        }
    }
}