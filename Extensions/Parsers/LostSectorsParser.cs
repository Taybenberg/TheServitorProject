using Flurl.Http;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public class LostSectorsParser : IInventoryParser
    {
        public async Task<Stream> GetImageAsync()
        {
            using Image image = Image.Load(ExtensionsRes.LostSectorsBackground);

            Font dateFont = new Font(SystemFonts.Find("Arial"), 32, FontStyle.Bold);

            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17).ToLocalTime();

            image.Mutate(m => m.DrawText((currDate.Hour < 17 ?
                $"{resetTime.AddDays(-1).ToString("dd.MM HH:mm")} – {resetTime.ToString("dd.MM HH:mm")}" :
                $"{resetTime.ToString("dd.MM HH:mm")} – {resetTime.AddDays(1).ToString("dd.MM HH:mm")}"),
                dateFont, Color.White, new Point(142, 61)));

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            Font lightFont = new Font(SystemFonts.Find("Arial"), 32);
            Font sectorFont = new Font(SystemFonts.Find("Arial"), 28);

            string[] sectors = { "//*[contains(@id,'bl_lost_sector_legend')]", "//*[contains(@id,'bl_lost_sector_master')]" };

            for (int i = 0; i < sectors.Length; i++)
            {
                var sector = htmlDoc.DocumentNode.SelectSingleNode(sectors[i]);

                if (sector is not null)
                {
                    var lightLevel = sector.SelectSingleNode("./div[14]/div[1]").InnerText;

                    image.Mutate(m => m.DrawText(lightLevel, lightFont, Color.Black, new Point(306 + 386 * i, 129)));

                    var sectorIcon = sector.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;

                    using var stream = await sectorIcon.GetStreamAsync();
                    using Image icon = await Image.LoadAsync(stream);
                    icon.Mutate(m => m.Resize(346, 194));

                    image.Mutate(m => m.DrawImage(icon, new Point(35 + 386 * i, 178), 1));

                    var sectorName = sector.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;

                    image.Mutate(m => m.DrawText(sectorName, sectorFont, Color.Black, new Point(33 + 386 * i, 419)));

                    var reward = sector.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                    image.Mutate(m => m.DrawText(Localization.ItemNames[reward], sectorFont, Color.Black, new Point(33 + 386 * i, 491)));
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
