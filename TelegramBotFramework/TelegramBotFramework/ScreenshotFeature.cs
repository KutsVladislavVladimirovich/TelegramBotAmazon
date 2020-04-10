using System.Drawing;
using System.Windows.Forms;

namespace TelegramBotFramework
{
    public static class ScreenshotFeature
    {
        public static void MakeScreenshot(string name)
        {
            var image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            var graph = Graphics.FromImage(image);

            graph.CopyFromScreen(0, 0, 0, 0, image.Size);

            image.Save(name);
        }
    }
}