using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ResourcesParser
    {
        public static async Task<Stream> GetResourcesAsync()
        {
            using var loader = new ImageLoader();

            using Image image = Image.Load(ExtensionsRes.ResourcesBackground);

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/vendors");

            Font font = new Font(SystemFonts.Find("Arial"), 20, FontStyle.Bold);

            var spiderContainer = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@hash,'863940356')]/div[2]/div[5]/div");

            if (spiderContainer is not null)
            {
                image.Mutate(m => m.DrawText($"Павук", font, Color.Black, new Point(30, 10)));

                int y = 30;

                for (int i = 1; i <= 7; i++)
                {
                    var resIconUrl = spiderContainer.SelectSingleNode($"./div[{i}]/div[1]/div/img").Attributes["src"].Value;
                    Image res = await loader.GetImage(resIconUrl);
                    image.Mutate(m => m.DrawImage(res, new Point(30, y), 1));

                    var resName = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[1]/p[1]").InnerText[9..];
                    image.Mutate(m => m.DrawText(resName, font, Color.Black, new Point(130, y)));

                    var currencyIconUrl = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]/div/div/img").Attributes["src"].Value;
                    Image currency = await loader.GetImage(currencyIconUrl);
                    currency.Mutate(m => m.Resize(40, 40));
                    image.Mutate(m => m.DrawImage(currency, new Point(130, y + 50), 1));

                    var currencyName = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]/div/p[1]").InnerText;
                    image.Mutate(m => m.DrawText(currencyName, font, Color.Black, new Point(180, y + 50)));

                    var currencyQuantity = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]/div/p[2]").InnerText;
                    image.Mutate(m => m.DrawText(currencyQuantity, font, Color.Black, new Point(380, y + 50)));

                    y += 96 + 10;
                }
            }

            var bansheeContainer = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@hash,'672118013')]/div[2]/div[4]/div");

            if (bansheeContainer is not null)
            {
                image.Mutate(m => m.DrawText($"Банші-44", font, Color.Black, new Point(430, 10)));

                int y = 30;

                for (int i = 1; i <= 7; i++)
                {
                    var resIconUrl = (bansheeContainer.SelectSingleNode($"./div[{i}]/div[1]/div/img[2]") ??
                        bansheeContainer.SelectSingleNode($"./div[{i}]/div[1]/div/img")).Attributes["src"].Value;
                    Image res = await loader.GetImage(resIconUrl);
                    image.Mutate(m => m.DrawImage(res, new Point(430, y), 1));

                    var resName = bansheeContainer.SelectSingleNode($"./div[{i}]/div[3]/div[1]/p[1]").InnerText;
                    image.Mutate(m => m.DrawText(resName, font, Color.Black, new Point(530, y)));

                    var currencyContainer = bansheeContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]");

                    for (int j = 1; j <= 4; j++)
                    {
                        var currencyEntry = currencyContainer.SelectSingleNode($"./div[{j}]");

                        if (currencyEntry is null)
                            break;

                        y += 50;

                        var currencyIconUrl = currencyEntry.SelectSingleNode($"./div/img").Attributes["src"].Value;
                        Image currency = await loader.GetImage(currencyIconUrl);
                        currency.Mutate(m => m.Resize(40, 40));
                        image.Mutate(m => m.DrawImage(currency, new Point(530, y), 1));

                        var currencyName = currencyEntry.SelectSingleNode($"./p[1]").InnerText;
                        image.Mutate(m => m.DrawText(currencyName, font, Color.Black, new Point(580, y)));

                        var currencyQuantity = currencyEntry.SelectSingleNode($"./p[2]").InnerText;
                        image.Mutate(m => m.DrawText(currencyQuantity, font, Color.Black, new Point(780, y)));
                    }

                    y += 60;
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
