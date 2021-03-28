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
                using var stream = await url.GetStreamAsync();

                cachedImages.TryAdd(url, new Bitmap(stream));
            }

            return cachedImages[url];
        }

        public async Task<Stream> GetEververseInventoryAsync(string seasonName, DateTime seasonStart)
        {
            using var background = new MemoryStream(ExtensionsRes.EververseItemsBackground);

            using Image image = new Bitmap(background);

            using (var g = Graphics.FromImage(image))
            {
                int currWeek = (int)(DateTime.Now - seasonStart).TotalDays / 7 + 1;

                Brush brush = new SolidBrush(Color.White);
                Font font = new Font("Arial", 25, FontStyle.Bold);

                int Xt = 210, Yt1 = 10, Yt2 = 70;

                g.DrawString($"{seasonStart.AddDays((currWeek - 1) * 7).ToString("dd.MM.yyyy")} – Тиждень {currWeek}", font, brush, Xt, Yt1);
                g.DrawString($"Сезон \"{seasonName}\"", font, brush, Xt, Yt2);

                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseWeekly");
                var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div/div[{currWeek}]");

                if (eververseWeekly is not null)
                {
                    string iconUrl = eververseWeekly.SelectSingleNode($"./div[1]/img").Attributes["src"].Value;
                    Image icon = await GetImage(iconUrl);
                    g.DrawImage(icon, 0, 0, 192, 192);

                    var silverContainer = eververseWeekly.SelectSingleNode($"./div[2]/div[1]/div");

                    if (silverContainer is not null)
                    {
                        int Xs = 35, Ys = 178, intervalX = 106;

                        for (int i = 1; i <= 5; i++)
                        {
                            iconUrl = silverContainer.SelectSingleNode($"./div[{i}]/div[1]/div/img[3]").Attributes["src"].Value;
                            icon = await GetImage(iconUrl);
                            g.DrawImage(icon, Xs, Ys);

                            iconUrl = silverContainer.SelectSingleNode($"./div[{i}]/div[1]/div/img[1]").Attributes["src"].Value;
                            icon = await GetImage(iconUrl);
                            g.DrawImage(icon, Xs, Ys);

                            Xs += intervalX;
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
