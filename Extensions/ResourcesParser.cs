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

            int[][] vendorsInt = { new int[] { 3, 1, 1 }, new int[] { 7, 2, 2 } };
            string[][] vendorsStr = { new string[] { "Павук", "Банші-44", "Ада-1" }, new string[] { "//*[contains(@hash,'863940356')]/div[2]/div[5]/div", "//*[contains(@hash,'672118013')]/div[2]/div[4]/div", "//*[contains(@hash,'350061650')]/div[2]/div[4]/div" } };

            for (int i = 0; i < vendorsStr[0].Length; i++)
            {
                var container = htmlDoc.DocumentNode.SelectSingleNode(vendorsStr[1][i]);

                if (container is not null)
                {
                    image.Mutate(m => m.DrawText(vendorsStr[0][i], font, Color.Black, new Point(30 + 350 * i, 10)));

                    int y = 30;

                    for (int j = vendorsInt[0][i]; j <= vendorsInt[1][i]; j++)
                    {
                        var resIconUrl = (container.SelectSingleNode($"./div[{j}]/div[1]/div/img[2]") ??
                            container.SelectSingleNode($"./div[{j}]/div[1]/div/img")).Attributes["src"].Value;
                        Image res = await loader.GetImage(resIconUrl);
                        image.Mutate(m => m.DrawImage(res, new Point(30 + 350 * i, y), 1));

                        var resName = container.SelectSingleNode($"./div[{j}]/div[3]/div[1]/p[1]").InnerText.Replace("Purchase ", "");
                        image.Mutate(m => m.DrawText(resName, font, Color.Black, new Point(130 + 350 * i, y)));

                        var currencyContainer = container.SelectSingleNode($"./div[{j}]/div[3]/div[2]/div[2]");

                        for (int z = 1; z <= 4; z++)
                        {
                            var currencyEntry = currencyContainer.SelectSingleNode($"./div[{z}]");

                            if (currencyEntry is null)
                                break;

                            y += 50;

                            var currencyIconUrl = currencyEntry.SelectSingleNode($"./div/img").Attributes["src"].Value;
                            Image currency = await loader.GetImage(currencyIconUrl);
                            currency.Mutate(m => m.Resize(40, 40));
                            image.Mutate(m => m.DrawImage(currency, new Point(130 + 350 * i, y), 1));

                            /*
                            var currencyName = currencyEntry.SelectSingleNode($"./p[1]").InnerText;
                            image.Mutate(m => m.DrawText(currencyName, font, Color.Black, new Point(180 + 350 * i, y)));
                            */

                            var currencyQuantity = currencyEntry.SelectSingleNode($"./p[2]").InnerText;
                            image.Mutate(m => m.DrawText(currencyQuantity, font, Color.Black, new Point(180 + 350 * i, y)));
                        }

                        y += 60;
                    }
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
