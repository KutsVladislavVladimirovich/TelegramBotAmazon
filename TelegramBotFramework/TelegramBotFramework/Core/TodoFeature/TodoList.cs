using Newtonsoft.Json;
using System.IO;
using TelegramBotFramework.Models;

namespace TelegramBotFramework.Core.TodoFeature
{
    public class TodoList
    {
        private readonly string _path;

        public TodoList(string leadId)
        {
            _path = $"{Constants.PathToDebug}\\{leadId}";
        }

        public TodoModel[] GetTodoList()
        {
            var jsonFile = $"{_path}\\{Constants.JsonFile}";
            if (!File.Exists(jsonFile))
            {
                Directory.CreateDirectory(_path);

                File.CreateText(jsonFile).Dispose();

                return new TodoModel[0];
            }

            if (File.ReadAllText(jsonFile).Length == 0)
                return new TodoModel[0];

            using (var reader = File.OpenText(jsonFile))
            {
                var fileText = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<TodoModel[]>(fileText);
            }
        }

        public void SaveData(TodoModel[] todoModel)
        {
            using (var writer = File.CreateText($"{_path}\\{Constants.JsonFile}"))
            {
                var output = JsonConvert.SerializeObject(todoModel);
                writer.Write(output.Replace(",null", string.Empty));
            }
        }

        public void ClearFile()
        {
            File.Delete($"{_path}\\{Constants.JsonFile}");
            File.CreateText($"{_path}\\{Constants.JsonFile}").Dispose();
        }
    }
}