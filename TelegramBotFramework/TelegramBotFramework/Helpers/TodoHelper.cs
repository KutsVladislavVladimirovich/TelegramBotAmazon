using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotFramework.Core;
using TelegramBotFramework.Core.TodoFeature;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.Helpers
{
    public static class TodoHelper
    {
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
                new TodoList(e.CallbackQuery.From.Id.ToString()).RemoveElement(index - 1);

                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Дело №{index} удалено.");

                SendTodoList(e.CallbackQuery.From.Id.ToString());
            }
            else if (e.CallbackQuery.Data.EndsWith("✅"))
            {
                new TodoList(e.CallbackQuery.From.Id.ToString()).ChangeIsDone(int.Parse(e.CallbackQuery.Data[6].ToString()) - 1, false);
                SendTodoList(e.CallbackQuery.From.Id.ToString());
            }
            else if (e.CallbackQuery.Data.EndsWith("❌"))
            {
                new TodoList(e.CallbackQuery.From.Id.ToString()).ChangeIsDone(int.Parse(e.CallbackQuery.Data[6].ToString()) - 1, true);
                SendTodoList(e.CallbackQuery.From.Id.ToString());
            }
            else if (e.CallbackQuery.Data.StartsWith("Удалить дело №"))
                await Configuration.Bot.SendTextMessageAsync(e.CallbackQuery.From.Id, "Вы уверенны?", replyMarkup: new InlineKeyboardMarkup(new[]
                    { new [] { InlineKeyboardButton.WithCallbackData($"Да, удалить дело №{e.CallbackQuery.Data.Replace("Удалить дело №", string.Empty)}") } }));
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
                await Configuration.Bot.SendTextMessageAsync(senderId, $"{actualTodoList[i].Text}\r\nДело добавлено - {actualTodoList[i].CreationDate}", replyMarkup: new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(actualTodoList[i].IsDone.Equals(true) ? $"Дело №{i + 1}✅" : $"Дело №{i + 1}❌"),
                        InlineKeyboardButton.WithCallbackData($"Удалить дело №{i + 1}")
                    }
                }));

            await Configuration.Bot.SendTextMessageAsync(senderId, "Если хотите добавить дело напишите 'дело' и текст.");
        }
    }
}