using System;
using System.Globalization;

namespace TelegramBotFramework.Models
{
    public class TodoModel
    {
        public string CreationDate { get; set; } = DateTime.Now.AddHours(3).ToString("F", new CultureInfo("ru-ru"));

        private bool _isDone;
        private string _text;

        public bool IsDone
        {
            get => _isDone;
            set => _isDone = value;
        }

        public string Text
        {
            get => _text;
            set => _text = value;
        }
    }
}