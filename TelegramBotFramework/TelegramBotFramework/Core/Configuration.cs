using System;
using ApiAiSDK;
using Telegram.Bot;

namespace TelegramBotFramework.Core
{
    internal static class Configuration
    {
        internal static ITelegramBotClient Bot;
        internal static ApiAi ApiAi;
        internal static void InitializeBot()
        {
            if (Bot == null && ApiAi == null)
            {
                Bot = new TelegramBotClient(Constants.BotToken);
                ApiAi = new ApiAi(new AIConfiguration(Constants.SmartTalkToken, SupportedLanguage.Russian));

                var me = Bot.GetMeAsync().Result;
                Console.WriteLine($"Bot Name - {me.FirstName}.");
                Console.WriteLine($"Bot Username on Telegram - @{me.Username}.");

                Bot.OnMessage += BotOnMessage.Bot_OnMessage;
                Bot.OnCallbackQuery += BotOnMessage.BotOnCallbackQuery;
                Bot.StartReceiving();
            }
            else
                Console.WriteLine($"Bot is already initialized.");

            Console.ReadKey();
        }
    }
}