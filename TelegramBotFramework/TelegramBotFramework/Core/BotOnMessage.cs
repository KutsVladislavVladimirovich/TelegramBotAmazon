using System;
using System.Globalization;
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
        private static readonly CurrenciesExchange CurrenciesExchange = new CurrenciesExchange();
        private static readonly ApiAiBot ApiAiBot = new ApiAiBot();
        private static readonly IFormatProvider FormatProvider = new CultureInfo("ru-ru");
        internal static async void BotOnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            Console.WriteLine($"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName} нажал кнопку {e.CallbackQuery.Data}.");

            await Configuration.Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {e.CallbackQuery.Data}.");
            if (Regex.IsMatch(e.CallbackQuery.Data, @"\d{1,15} (?:[U|u][S|s][D|d]|[E|e][U|u][R|r]|[R|r][U|u][R|r])"))
            {
                var text = e.CallbackQuery.Data.Split(' ');
                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, CurrenciesExchange.CalculateCurrency(decimal.Parse(text[0]), text[1]));
                return;
            }

            switch (e.CallbackQuery.Data)
            {

                case "Меню погоды":
                    var weatherKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Погода в Днепре"),
                            InlineKeyboardButton.WithCallbackData("Погода в Новодонецком")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите населенный пункт.", replyMarkup: weatherKeyboard);
                    break;
                case "Погода в Днепре":
                case "/getweatherdnipro":
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Weather.GetDniproWeather());
                    break;
                case "Погода в Новодонецком":
                case "/getweathernovodonetskoe":
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Weather.GetNovodonetckoeWeather());
                    break;
                case "Курс валют":
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, CurrenciesExchange.GetExchangeRates());
                    break;
                case "UAH - USD":
                    var usdKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("100 USD"),
                            InlineKeyboardButton.WithCallbackData("700 USD"),
                            InlineKeyboardButton.WithCallbackData("1000 USD"),
                            InlineKeyboardButton.WithCallbackData("1200 USD")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пункт или введите сумму + USD", replyMarkup: usdKeyboard);
                    break;
                case "UAH - EUR":
                    var eurKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("100 EUR"),
                            InlineKeyboardButton.WithCallbackData("700 EUR"),
                            InlineKeyboardButton.WithCallbackData("1000 EUR"),
                            InlineKeyboardButton.WithCallbackData("1200 EUR")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пункт или введите сумму + EUR", replyMarkup: eurKeyboard);
                    break;
                case "UAH - RUR":
                    var rurKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("100 RUR"),
                            InlineKeyboardButton.WithCallbackData("700 RUR"),
                            InlineKeyboardButton.WithCallbackData("1000 RUR"),
                            InlineKeyboardButton.WithCallbackData("1200 RUR")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пункт или введите сумму + USD", replyMarkup: rurKeyboard);
                    break;
                default:
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Такой команды не знаю =(");
                    break;
            }
        }

        internal static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message == null || !e.Message.Type.Equals(MessageType.Text))
            {
                if (e.Message != null && e.Message.Type.Equals(MessageType.Photo))
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Вау {e.Message.From.FirstName}! Это очень классная фотка!");

                return;
            }

            Console.WriteLine($"Contact with name {e.Message.From.FirstName} {e.Message.From.LastName} and id {e.Message.Chat.Id} send message with text '{e.Message.Text}'.");

            if (Regex.IsMatch(e.Message.Text, @"\d{1,15} (?:[U|u][S|s][D|d]|[E|e][U|u][R|r]|[R|r][U|u][R|r])"))
            {
                var text = e.Message.Text.Split(' ');
                await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, CurrenciesExchange.CalculateCurrency(decimal.Parse(text[0]), text[1]));
                return;
            }

            switch (e.Message.Text)
            {
                case "/start":
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("Меню валют"),
                        new KeyboardButton("Меню погоды"),
                        new KeyboardButton("Текущая дата")
                    }, oneTimeKeyboard: true);
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот всё что я умею)", replyMarkup: markup);
                    break;
                case "Меню погоды":
                    var weatherKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Погода в Днепре"),
                            InlineKeyboardButton.WithCallbackData("Погода в Новодонецком")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите населенный пункт.", replyMarkup: weatherKeyboard);
                    break;
                case "Меню валют":
                    var exchangeKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Курс валют"),
                            InlineKeyboardButton.WithCallbackData("UAH - USD"),
                            InlineKeyboardButton.WithCallbackData("UAH - EUR"),
                            InlineKeyboardButton.WithCallbackData("UAH - RUR")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт меню.", replyMarkup: exchangeKeyboard);
                    break;
                case "Текущая дата":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, DateTime.Now.ToString("f", FormatProvider));
                    break;
                default:
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, ApiAiBot.Speech(e.Message.Text));
                    break;
            }
        }
    }
}