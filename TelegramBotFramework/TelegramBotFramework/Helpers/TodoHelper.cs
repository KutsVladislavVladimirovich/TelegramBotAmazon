using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotFramework.Core;
using TelegramBotFramework.Core.TodoFeature;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.Helpers
{
    public static class TodoHelper
    {
        private static string Done = "Выполнено👍";
        private static string NotDone = "Не выполнено👎";
        public static void MessageTodoList(string senderId, string message)
        {
            if (message.StartsWith("Дело ") || message.StartsWith("дело "))
            {
                new TodoList(senderId).Add(new TodoModel { Text = message.Substring(5) });
                SendTodoList(senderId);
            }
        }

        public static async void CallbackToDoList(CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data.StartsWith("Да, удалить дело №"))
            {
                var index = int.Parse(e.CallbackQuery.Data.Replace("Да, удалить дело №", string.Empty));
                var list = new TodoList(e.CallbackQuery.From.Id.ToString());
                if (list.GetTodoList().Length < index)
                {
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Дела №{index} нет.\r\nНаверное я его уже удалил.");
                    return;
                }

                list.RemoveElement(index - 1);
                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Дело №{index} удалено.");
                SendTodoList(e.CallbackQuery.From.Id.ToString());
            }
            else if (e.CallbackQuery.Data.EndsWith("✅"))
            {
                var index = int.Parse(e.CallbackQuery.Data[6].ToString());
                var list = new TodoList(e.CallbackQuery.From.Id.ToString());
                if (list.GetTodoList().Length < index)
                {
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Дела №{index} нет.\r\nНаверное я его уже удалил.");
                    return;
                }
                list.ChangeIsDone(index - 1, false);
                SendTodoList(e.CallbackQuery.From.Id.ToString());
            }
            else if (e.CallbackQuery.Data.EndsWith("❌"))
            {
                var index = int.Parse(e.CallbackQuery.Data[6].ToString());
                var list = new TodoList(e.CallbackQuery.From.Id.ToString());
                if (list.GetTodoList().Length < index)
                {
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Дела №{index} нет.\r\nНаверное я его уже удалил.");
                    return;
                }
                list.ChangeIsDone(index - 1, true);
                SendTodoList(e.CallbackQuery.From.Id.ToString());
            }
            else if (e.CallbackQuery.Data.StartsWith("Удалить дело №"))
            {
                var index = int.Parse(e.CallbackQuery.Data.Replace("Удалить дело №", String.Empty));
                var list = new TodoList(e.CallbackQuery.From.Id.ToString());
                if (list.GetTodoList().Length < index)
                {
                    await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Дела №{index} нет.\r\nНаверное я его уже удалил.");
                    return;
                }
                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Вы уверенны?", replyMarkup: new InlineKeyboardMarkup(new[] 
                    { new [] { InlineKeyboardButton.WithCallbackData($"Да, удалить дело №{e.CallbackQuery.Data.Replace("Удалить дело №", string.Empty)}") } }));

            }
        }

        public static async void SendTodoList(string senderId)
        {
            var todoList = new TodoList(senderId);
            var actualTodoList = todoList.GetTodoList();

            if (actualTodoList.Length == 0)
            {
                var leadListKeyboardKeyboard = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Добавить дело")
                    }
                });
                await Configuration.Bot.SendTextMessageAsync(senderId, "Список дел пуст.", replyMarkup: leadListKeyboardKeyboard);

                return;
            }

            await Configuration.Bot.SendTextMessageAsync(senderId, "Список дел:");

            for (var i = 0; i < actualTodoList.Length; i++)
            {
                var doneStatus = actualTodoList[i].IsDone.Equals(true) ? Done : NotDone;
                await Configuration.Bot.SendTextMessageAsync(senderId, $"{actualTodoList[i].Text}\r\nДело добавлено - {actualTodoList[i].CreationDate}\r\n{doneStatus}", replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(actualTodoList[i].IsDone.Equals(true) ? $"Дело №{i + 1}✅" : $"Дело №{i + 1}⭕️"),
                        InlineKeyboardButton.WithCallbackData($"Удалить дело №{i + 1}")
                    }
                }));

            }

            await Configuration.Bot.SendTextMessageAsync(senderId, "Если хотите добавить дело напишите 'дело' и текст.");
        }
    }
}