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

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            Font font = new Font(SystemFonts.Find("Arial"), 20, FontStyle.Bold);

            image.Mutate(m => m.DrawText($"Загублені сектори – {DateTime.Now.ToShortDateString()}", font, Color.Black, new Point(30, 10)));

            var sectorMaster = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'bl_lost_sector_master')]");

            if (sectorMaster is not null)
            {
                image.Mutate(m => m.DrawText
                (
                    $"Складність рівня \"Майстер\"", font, Color.Black, new Point(30, 30))
                );

                var sectorIcon = sectorMaster.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;

                using var stream = await sectorIcon.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);
                icon.Mutate(m => m.Resize(500, 281));

                image.Mutate(m => m.DrawImage(icon, new Point(60, 55), 1));

                var sectorName = sectorMaster.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;

                image.Mutate(m => m.DrawText($"Сектор: {sectorName}", font, Color.Black, new Point(30, 340)));

                var reward = sectorMaster.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                image.Mutate(m => m.DrawText($"Соло-нагорода: {reward}", font, Color.Black, new Point(30, 360)));
            }

            var sectorLegend = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'bl_lost_sector_legend')]");

            if (sectorLegend is not null)
            {
                image.Mutate(m => m.DrawText($"Складність рівня \"Легенда\"", font, Color.Black, new Point(30, 390)));

                var sectorIcon = sectorLegend.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;

                using var stream = await sectorIcon.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);
                icon.Mutate(m => m.Resize(500, 281));

                image.Mutate(m => m.DrawImage(icon, new Point(60, 415), 1));

                var sectorName = sectorLegend.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;

                image.Mutate(m => m.DrawText($"Сектор: {sectorName}", font, Color.Black, new Point(30, 700)));

                var reward = sectorLegend.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                image.Mutate(m => m.DrawText($"Соло-нагорода: {reward}", font, Color.Black, new Point(30, 720)));
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
