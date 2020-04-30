using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotFramework.ApiActions.ExchangeRates;
using TelegramBotFramework.ApiActions.Weather;
using TelegramBotFramework.Core.DialogFlow;
using TelegramBotFramework.Core.TodoFeature;
using TelegramBotFramework.Helpers;
using TelegramBotFramework.Models;

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
            await Configuration.Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {e.CallbackQuery.Data}.");

            if (Regex.IsMatch(e.CallbackQuery.Data, @"\d{1,15} (?:[U|u][S|s][D|d]|[E|e][U|u][R|r]|[R|r][U|u][R|r])"))
            {
                var text = e.CallbackQuery.Data.Split(' ');
                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, CurrenciesExchange.CalculateCurrency(decimal.Parse(text[0]), text[1]));

                return;
            }

            TodoHelper.CallbackToDoList(e);

            switch (e.CallbackQuery.Data)
            {
                case "Добавить дело":
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Напишите 'дело' и текст.");
                    break;
                case "Погода в Днепре":
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, Weather.GetDniproWeather());
                    break;
                case "Погода в Новодонецком":
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
                            InlineKeyboardButton.WithCallbackData("100 USD"), InlineKeyboardButton.WithCallbackData("700 USD"), InlineKeyboardButton.WithCallbackData("1000 USD"), InlineKeyboardButton.WithCallbackData("1200 USD")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пункт или введите сумму + USD", replyMarkup: usdKeyboard);
                    break;
                case "UAH - EUR":
                    var eurKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("100 EUR"), InlineKeyboardButton.WithCallbackData("700 EUR"), InlineKeyboardButton.WithCallbackData("1000 EUR"), InlineKeyboardButton.WithCallbackData("1200 EUR")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пункт или введите сумму + EUR", replyMarkup: eurKeyboard);
                    break;
                case "UAH - RUR":
                    var rurKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("100 RUR"), InlineKeyboardButton.WithCallbackData("700 RUR"), InlineKeyboardButton.WithCallbackData("1000 RUR"), InlineKeyboardButton.WithCallbackData("1200 RUR")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Выберите пункт или введите сумму + USD", replyMarkup: rurKeyboard);
                    break;
                    //default:
                    //    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Такой команды не знаю =(");
                    //    break;
            }

            if (!e.CallbackQuery.From.Id.Equals(Constants.AdminId))
                await Configuration.Bot.SendTextMessageAsync(Constants.AdminId, $"Пользователь с именем {e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName} and id {e.CallbackQuery.From.Id} прислал сообщение боту с текстом - {e.CallbackQuery.Data}.");
        }

        internal static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message == null || !e.Message.Type.Equals(MessageType.Text))
            {
                if (e.Message != null && e.Message.Type.Equals(MessageType.Photo))
                {
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Вау, {e.Message.From.FirstName}! Это очень классная фотка!");
                    await Configuration.Bot.SendPhotoAsync(Constants.AdminId, e.Message.Photo.Last().FileId);
                    await Configuration.Bot.SendTextMessageAsync(Constants.AdminId, $"Пользователь {e.Message.From.Username} прислал фото боту.");
                }

                return;
            }

            if (!e.Message.From.Id.Equals(Constants.AdminId))
                await Configuration.Bot.SendTextMessageAsync(Constants.AdminId, $"Пользователь с именем {e.Message.From.FirstName} {e.Message.From.LastName} и id {e.Message.From.Id} прислал сообщение боту с текстом - {e.Message.Text}.");

            if (Regex.IsMatch(e.Message.Text, @"\d{1,15} (?:[U|u][S|s][D|d]|[E|e][U|u][R|r]|[R|r][U|u][R|r])"))
            {
                var text = e.Message.Text.Split(' ');
                await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, CurrenciesExchange.CalculateCurrency(decimal.Parse(text[0]), text[1]));

                return;
            }

            if (e.Message.Text.StartsWith("Дело ") || e.Message.Text.StartsWith("дело "))
            {
                TodoHelper.MessageTodoList(e.Message.From.Id.ToString(), e.Message.Text);

                return;
            }

            switch (e.Message.Text)
            {
                case "Удалить все дела":
                    new TodoList(e.Message.Chat.Id.ToString()).ClearFile();

                    var clearLeadListKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Добавить дело")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Список дел пуст.", replyMarkup: clearLeadListKeyboard);

                    break;
                case "Список дел":
                    TodoHelper.SendTodoList(e.Message.Chat.Id.ToString());
                    break;
                case "/start":
                    var markup = new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("Меню валют"), new KeyboardButton("Меню погоды"), new KeyboardButton("Текущая дата"), new KeyboardButton("Список дел")
                    }, oneTimeKeyboard: true);
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Вот всё что я умею)", replyMarkup: markup);
                    break;
                case "Меню погоды":
                    var weatherKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Погода в Днепре"), InlineKeyboardButton.WithCallbackData("Погода в Новодонецком")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите населенный пункт.", replyMarkup: weatherKeyboard);
                    break;
                case "Меню валют":
                    var exchangeKeyboard = new InlineKeyboardMarkup(new[]
                    {
                        new []
                        {
                            InlineKeyboardButton.WithCallbackData("Курс валют"), InlineKeyboardButton.WithCallbackData("UAH - USD"), InlineKeyboardButton.WithCallbackData("UAH - EUR"), InlineKeyboardButton.WithCallbackData("UAH - RUR")
                        }
                    });
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите пункт меню.", replyMarkup: exchangeKeyboard);
                    break;
                case "Текущая дата":
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, DateTime.Now.AddHours(3).ToString("f", FormatProvider));
                    break;
                default:
                    await Configuration.Bot.SendTextMessageAsync(e.Message.Chat.Id, ApiAiBot.Speech(e.Message.Text));
                    break;
            }
        }
    }
}