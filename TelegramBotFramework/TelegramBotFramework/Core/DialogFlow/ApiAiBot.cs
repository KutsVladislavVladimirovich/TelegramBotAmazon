namespace TelegramBotFramework.Core.DialogFlow
{
    internal class ApiAiBot
    {
        internal string Speech(string message)
        {
            var response = Configuration.ApiAi.TextRequest(message);
            var answer = response.Result.Fulfillment.Speech;
            if (answer.Equals(string.Empty))
                answer = $"Прости, я тебя не понял, скоро буду отвечать и на {message}";

            return answer;
        }
    }
}