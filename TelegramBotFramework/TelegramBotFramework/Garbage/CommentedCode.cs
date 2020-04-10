namespace TelegramBotFramework.Garbage
{
    class CommentedCode
    {
        //Make screenshot by btn
        //    ScreenshotFeature.MakeScreenshot(Constants.SreenshotName);
        //    if (!e.CallbackQuery.Message.Chat.Id.Equals(Constants.AdminId))
        //    using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
        //    {
        //        var inputMedia = new InputMedia(stream, stream.Name);
        //        await bot.SendPhotoAsync(Constants.AdminId, inputMedia);
        //    }

        //using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
        //{
        //var inputMedia = new InputMedia(stream, stream.Name);
        //await bot.SendPhotoAsync(e.CallbackQuery.Message.Chat.Id, inputMedia);

        //Make screenshot by /getscreen
        //    ScreenshotFeature.MakeScreenshot(Constants.SreenshotName);
        //    if (!e.Message.Chat.Id.Equals(Constants.AdminId))
        //    using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
        //    {
        //        var inputMedia = new InputMedia(stream, stream.Name);
        //        await bot.SendPhotoAsync(Constants.AdminId, inputMedia);
        //    }

        //using (var stream = System.IO.File.OpenRead(Constants.SreenshotName))
        //{
        //var inputMedia = new InputMedia(stream, stream.Name);
        //await bot.SendPhotoAsync(e.Message.Chat.Id, inputMedia);
        //}
        //await bot.SendTextMessageAsync(Constants.AdminId, $"Пользователь с id - {e.Message.Chat.Id} и именем {e.Message.From.FirstName} {e.Message.From.LastName} получил скриншот вашего рабочего стола.");
    }
}