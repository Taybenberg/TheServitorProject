using Flurl.Http;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Concurrent;
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
                    cachedImages.TryAdd(url, await Image.LoadAsync(stream));
                }
                catch
                {
                    return new Image<Rgba32>(1, 1);
                }
            }

            return cachedImages[url];
        }

        public async Task<Stream> GetEververseInventoryAsync(string seasonName, DateTime seasonStart, int weekNumber)
        {
            using Image image = Image.Load(ExtensionsRes.EververseItemsBackground);

            Font font = new Font(SystemFonts.Find("Arial"), 30, FontStyle.Bold);

            int Xt = 212, Yt1 = 12, Yt2 = 73;

            image.Mutate(m => m.DrawText
            (
                $"{seasonStart.AddDays((weekNumber - 1) * 7).ToString("dd.MM.yyyy")} – Тиждень {weekNumber}",
                font, Color.White, new Point(Xt, Yt1))
            );

            image.Mutate(m => m.DrawText
            (
                $"Сезон \"{seasonName}\"",
                font, Color.White, new Point(Xt, Yt2))
            );

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseWeekly");
            var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"/html/body/main/div/div[{weekNumber}]");

            if (eververseWeekly is not null)
            {
                string iconUrl = eververseWeekly.SelectSingleNode($"./div[1]/img").Attributes["src"].Value;
                Image icon = (await GetImage(iconUrl)).Clone(m => m.Resize(192, 192));
                image.Mutate(m => m.DrawImage(icon, new Point(0, 0), 1));

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
                                image.Mutate(m => m.DrawImage(icon, new Point(x, y), 1));
                            }

                            node = container.SelectSingleNode($"./div[{j}]/div[1]/div/img[1]");

                            if (node is null)
                                break;

                            iconUrl = node.Attributes["src"].Value;
                            icon = await GetImage(iconUrl);
                            image.Mutate(m => m.DrawImage(icon, new Point(x, y), 1));

                            x += intervalX;
                        }
                    }
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

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
