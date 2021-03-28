using Flurl.Http;
using HtmlAgilityPack;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public static class TrialsOfOsirisParser
    {
        public static async Task<Stream> GetOsirisInventoryAsync()
        {
            using var background = new MemoryStream(ExtensionsRes.TrialsItemsBackground);

            using Image image = new Bitmap(background);

            using (var g = Graphics.FromImage(image))
            {
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
                            var node = trialsBillboard.SelectSingleNode($"div[2]/span[{i}]/a[{j}]/img[@src]");

                            if (node is null)
                                break;

                            using var stream = await node.Attributes["src"].Value.GetStreamAsync();
                            using Image icon = new Bitmap(stream);

                            g.DrawImage(icon, x, y);

                            x += intervalX;
                        }

                        Yi += intervalY;
                    }

                    int Xt = 254, Yt = 574;

                    Brush brush = new SolidBrush(Color.White);
                    Font locationFont = new Font("Arial", 20, FontStyle.Bold);

                    var location = trialsBillboard.SelectSingleNode("div[1]/span/text()");
                    var locationName = location is null ? "Невизначено" : location.InnerText.Trim();

                    g.DrawString(locationName, locationFont, brush, Xt, Yt);
                }
            }

            var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return ms;
        }
    }
}
