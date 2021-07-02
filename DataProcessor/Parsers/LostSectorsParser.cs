using DataProcessor.Parsers.Inventory;
using Flurl.Http;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor.Parsers
{
    public class LostSectorsParser : IInventoryParser<LostSectorsInventory>
    {
        public async Task<LostSectorsInventory> GetInventoryAsync()
        {
            var inventory = new LostSectorsInventory();

            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17).ToLocalTime();

            if (currDate.Hour < 17)
            {
                inventory.ResetBegin = resetTime.AddDays(-1);
                inventory.ResetEnd = resetTime;
            }
            else
            {
                inventory.ResetBegin = resetTime;
                inventory.ResetEnd = resetTime.AddDays(1);
            }

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            foreach (var sector in new string[] { "//*[contains(@id,'bl_lost_sector_legend')]", "//*[contains(@id,'bl_lost_sector_master')]" })
            {
                var node = htmlDoc.DocumentNode.SelectSingleNode(sector);

                if (node is not null)
                    inventory.LostSectors.Add(new LostSector()
                    {
                        LightLevel = int.Parse(node.SelectSingleNode("./div[14]/div[1]").InnerText),
                        SectorIconURL = node.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value,
                        SectorName = node.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText,
                        SectorReward = node.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7]
                    });
            }

            return inventory;
        }

        public async Task<Stream> GetImageAsync()
        {
            using Image image = Image.Load(ExtensionsRes.LostSectorsBackground);

            var inventory = await GetInventoryAsync();

            Font dateFont = new Font(SystemFonts.Find("Arial"), 32, FontStyle.Bold);
            Font lightFont = new Font(SystemFonts.Find("Arial"), 32);
            Font sectorFont = new Font(SystemFonts.Find("Arial"), 28);

            image.Mutate(m => m
                .DrawText($"{inventory.ResetBegin.ToString("dd.MM HH:mm")} – {inventory.ResetEnd.ToString("dd.MM HH:mm")}",
                dateFont, Color.White, new Point(142, 61)));

            int i = 0;

            foreach (var sector in inventory.LostSectors)
            {
                using var stream = await sector.SectorIconURL.GetStreamAsync();
                using Image icon = await Image.LoadAsync(stream);
                icon.Mutate(m => m.Resize(346, 194));

                image.Mutate(m =>
                {
                    m.DrawText(sector.LightLevel.ToString(), lightFont, Color.Black, new Point(306 + i, 129));

                    m.DrawImage(icon, new Point(35 + i, 178), 1);

                    m.DrawText(sector.SectorName, sectorFont, Color.Black, new Point(33 + i, 419));

                    m.DrawText(Localization.TranslationDictionaries.ItemNames[sector.SectorReward], sectorFont, Color.Black, new Point(33 + i, 491));
                });

                i += 386;
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
