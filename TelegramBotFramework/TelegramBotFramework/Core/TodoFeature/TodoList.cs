using Newtonsoft.Json;
using System.IO;
using System.Linq;
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

        public void ChangeIsDone(int index, bool status)
        {
            var listOfModels = GetTodoList().ToList();
            listOfModels[index].IsDone = status;

            SaveData(listOfModels.ToArray());
        }

        public void Add(TodoModel todoModel)
        {
            var listOfModels = GetTodoList().ToList();
            listOfModels.Add(todoModel);

            SaveData(listOfModels.ToArray());
        }

        public void RemoveElement(int index)
        {
            var listOfModels = GetTodoList().ToList();
            listOfModels.RemoveAt(index);

            SaveData(listOfModels.ToArray());
        }

        public void SaveData(TodoModel[] todoModel)
        {
            using (var writer = File.CreateText($"{_path}\\{Constants.JsonFile}"))
            {
                var output = JsonConvert.SerializeObject(todoModel);
                writer.Write(output);
            }
        }

        public void ClearFile()
        {
            File.Delete($"{_path}\\{Constants.JsonFile}");
            File.CreateText($"{_path}\\{Constants.JsonFile}").Dispose();
        }
    }
}