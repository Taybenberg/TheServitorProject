using DataProcessor.Parsers.Inventory;
using HtmlAgilityPack;
using NetVips;
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
            var inventory = await GetInventoryAsync();

            var loader = new ImageLoader();

            Image image = Image.NewFromBuffer(ExtensionsRes.LostSectorsBackground);

            using var dateText = ImageLoader
                .RenderText
                ($"<b>{inventory.ResetBegin.ToString("dd.MM HH:mm")} – {inventory.ResetEnd.ToString("dd.MM HH:mm")}</b>",
                "Arial 32", new int[] { 255, 255, 255 });
            image = image.Composite(dateText, Enums.BlendMode.Over, 144, 67);

            int i = 0;

            foreach (var sector in inventory.LostSectors)
            {
                image = await DrawSectorAsync(loader, image, sector, i);
                i += 386;
            }

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }

        internal async Task<Image> DrawSectorAsync(ImageLoader loader, Image image, LostSector sector, int offset)
        {
            using var sectorIcon = await loader.GetImageAsync(sector.SectorIconURL);
            image = image.Composite
                (sectorIcon.ThumbnailImage(346, 194, Enums.Size.Force), Enums.BlendMode.Over, 35 + offset, 178);

            using var lightLevel = ImageLoader
                .RenderText(sector.LightLevel.ToString(), "Arial 32", new int[] { 0, 0, 0 });
            image = image.Composite(lightLevel, Enums.BlendMode.Over, 308 + offset, 135);

            using var sectorName = ImageLoader
                .RenderText(sector.SectorName.ToString(), "Arial 28", new int[] { 0, 0, 0 });
            image = image.Composite(sectorName, Enums.BlendMode.Over, 35 + offset, 425);

            using var sectorReward = ImageLoader
                .RenderText(Localization.TranslationDictionaries.ItemNames[sector.SectorReward],
                "Arial 28", new int[] { 0, 0, 0 });
            image = image.Composite(sectorReward, Enums.BlendMode.Over, 35 + offset, 497);

            return image;
        }
    }
}
