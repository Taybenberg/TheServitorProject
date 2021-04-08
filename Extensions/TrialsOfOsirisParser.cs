using Flurl.Http;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public static class TrialsOfOsirisParser
    {
        public static async Task<Stream> GetOsirisInventoryAsync()
        {
            using Image image = Image.Load(ExtensionsRes.TrialsItemsBackground);

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

            var trialsBillboard = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"trials-billboard\"]/div[2]");

            if (trialsBillboard is not null)
            {
                int Xi = 252, Yi = 30;
                int intervalX = 121, intervalY = 136;

                for (int i = 1; i <= 4; i++)
                {
                    int x = Xi, y = Yi;

                    for (int j = 1; j <= 3; j++)
                    {
                        var node = trialsBillboard.SelectSingleNode($"./div[2]/span[{i}]/a[{j}]/img[@src]");

                        if (node is null)
                            break;

                        try
                        {
                            using var stream = await node.Attributes["src"].Value.GetStreamAsync();
                            using Image icon = await Image.LoadAsync(stream);

                            image.Mutate(m => m.DrawImage(icon, new Point(x, y), 1));
                        }
                        catch
                        {
                            continue;
                        }

                        x += intervalX;
                    }

                    Yi += intervalY;
                }

                int Xt = 257, Yt = 574;

                Font locationFont = new Font(SystemFonts.Find("Arial"), 28, FontStyle.Bold);

                var location = trialsBillboard.SelectSingleNode("./div[1]/span/text()");
                var locationName = location?.InnerText.Trim() ?? "Невизначено";

                image.Mutate(m => m.DrawText(locationName, locationFont, Color.White, new Point(Xt, Yt)));
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
