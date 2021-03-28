using Flurl.Http;
using HtmlAgilityPack;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public static class EververseParser
    {
        public static async Task<Stream> GetEververseInventoryAsync()
        {
            using var background = new MemoryStream(ExtensionsRes.EververseItemsBackground);

            using Image image = new Bitmap(background);

            using (var g = Graphics.FromImage(image))
            {
                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseWeekly");
            }

            var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return ms;
        }
    }
}
