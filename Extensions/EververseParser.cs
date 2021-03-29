using Flurl.Http;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public class EververseParser : IDisposable
    {
        private ConcurrentDictionary<string, Image> cachedImages = new();
        private async Task<Image> GetImage(string url)
        {
            if (!cachedImages.ContainsKey(url))
            {
                try
                {
                    using var stream = await url.GetStreamAsync();
                    cachedImages.TryAdd(url, Image.FromStream(stream));
                }
                catch
                {
                    return new Bitmap(1, 1);
                }
            }

            return cachedImages[url];
        }

        public async Task<Stream> GetEververseInventoryAsync(string seasonName, DateTime seasonStart, int weekNumber)
        {
            using var background = new MemoryStream(ExtensionsRes.EververseItemsBackground);

            using Image image = Image.FromStream(background);

            using (var g = Graphics.FromImage(image))
            {
                Brush brush = new SolidBrush(Color.White);
                Font font = new Font("Arial", 25, FontStyle.Bold);

                int Xt = 210, Yt1 = 10, Yt2 = 70;

                g.DrawString($"{seasonStart.AddDays((weekNumber - 1) * 7).ToString("dd.MM.yyyy")} – Тиждень {weekNumber}", font, brush, Xt, Yt1);
                g.DrawString($"Сезон \"{seasonName}\"", font, brush, Xt, Yt2);

                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseWeekly");
                var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div/div[{weekNumber}]");

                if (eververseWeekly is not null)
                {
                    string iconUrl = eververseWeekly.SelectSingleNode($"./div[1]/img").Attributes["src"].Value;
                    Image icon = await GetImage(iconUrl);
                    g.DrawImage(icon, 0, 0, 192, 192);

                    int X = 35, intervalX = 106;
                    int[] Y = { 178, 361, 467, 650 };

                    for (int i = 1; i <= 4; i++)
                    {
                        var container = eververseWeekly.SelectSingleNode($"./div[2]/div[{i}]/div");

                        if (container is not null)
                        {
                            int x = X, y = Y[i - 1];

                            for (int j = 1; j <= 7; j++)
                            {
                                var node = container.SelectSingleNode($"./div[{j}]/div[1]/div/img[3]")
                                ?? container.SelectSingleNode($"./div[{j}]/div[1]/div/img[2]");

                                if (node is not null)
                                {
                                    iconUrl = node.Attributes["src"].Value;
                                    icon = await GetImage(iconUrl);
                                    g.DrawImage(icon, x, y);
                                }

                                node = container.SelectSingleNode($"./div[{j}]/div[1]/div/img[1]");

                                if (node is null)
                                    break;

                                iconUrl = node.Attributes["src"].Value;
                                icon = await GetImage(iconUrl);
                                g.DrawImage(icon, x, y);

                                x += intervalX;
                            }
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            image.Save(ms, ImageFormat.Png);

            ms.Position = 0;

            return ms;
        }

        public void Dispose()
        {
            foreach (var image in cachedImages)
                image.Value.Dispose();

            cachedImages.Clear();
        }
    }
}
