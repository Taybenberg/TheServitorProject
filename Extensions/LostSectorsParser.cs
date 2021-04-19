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
    public static class LostSectorsParser
    {
        public static async Task<Stream> GetLostSectorsAsync()
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

            var sectorLegend = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'bl_lost_sector_legend')]");

            if (sectorLegend is not null)
            {
                var lightLevel = sectorLegend.SelectSingleNode("./div[14]/div[1]").InnerText;

                image.Mutate(m => m.DrawText(lightLevel, lightFont, Color.Black, new Point(306, 129)));

                var sectorIcon = sectorLegend.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;

                using var stream = await sectorIcon.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);
                icon.Mutate(m => m.Resize(346, 194));

                image.Mutate(m => m.DrawImage(icon, new Point(35, 178), 1));

                var sectorName = sectorLegend.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;

                image.Mutate(m => m.DrawText(sectorName, sectorFont, Color.Black, new Point(33, 419)));

                var reward = sectorLegend.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                image.Mutate(m => m.DrawText(Localization.ItemNames[reward], sectorFont, Color.Black, new Point(33, 491)));
            }

            var sectorMaster = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'bl_lost_sector_master')]");

            if (sectorMaster is not null)
            {
                var lightLevel = sectorMaster.SelectSingleNode("./div[14]/div[1]").InnerText;

                image.Mutate(m => m.DrawText(lightLevel, lightFont, Color.Black, new Point(694, 129)));

                var sectorIcon = sectorMaster.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;

                using var stream = await sectorIcon.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);
                icon.Mutate(m => m.Resize(346, 194));

                image.Mutate(m => m.DrawImage(icon, new Point(421, 178), 1));

                var sectorName = sectorMaster.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;

                image.Mutate(m => m.DrawText(sectorName, sectorFont, Color.Black, new Point(419, 419)));

                var reward = sectorMaster.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                image.Mutate(m => m.DrawText(Localization.ItemNames[reward], sectorFont, Color.Black, new Point(419, 491)));
            }
            
            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
