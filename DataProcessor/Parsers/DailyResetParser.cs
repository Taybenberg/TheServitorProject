using DataProcessor.Parsers.Inventory;
using NetVips;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.Parsers
{
    public class DailyResetParser : IInventoryParser<(LostSectorsInventory, ResourcesInventory)>
    {
        private readonly string _seasonName;
        private readonly int _weekNumber;

        public DailyResetParser(string seasonName, int weekNumber) =>
            (_seasonName, _weekNumber) = (seasonName, weekNumber);

        public async Task<(LostSectorsInventory, ResourcesInventory)> GetInventoryAsync()
        {
            var ls = new LostSectorsParser();

            var rs = new ResourcesParser();

            return (await ls.GetInventoryAsync(), await rs.GetInventoryAsync());
        }

        public async Task<Stream> GetImageAsync()
        {
            (var sectors, var resources) = await GetInventoryAsync();

            var loader = new ImageLoader();

            Image image = Image.NewFromBuffer(ExtensionsRes.DailyResetBackground);

            using var resetBegin = ImageLoader
                .RenderText
                ($"{sectors.ResetBegin.ToString("dd.MM HH:mm")} – {sectors.ResetEnd.ToString("dd.MM HH:mm")}",
                "Arial 52", new int[] { 255, 255, 255 });
            image = image.Composite(resetBegin, Enums.BlendMode.Over, 402, 38);

            using var week = ImageLoader
                .RenderText($"Тиждень {_weekNumber}. Сезон \"{_seasonName}\"", "Arial 52", new int[] { 255, 255, 255 });
            image = image.Composite(week, Enums.BlendMode.Over, 40, 110);

            {
                int j = 0;

                foreach (var sector in sectors.LostSectors)
                {
                    using var sectorIcon = await loader.GetImageAsync(sector.SectorIconURL);
                    image = image.Composite
                        (sectorIcon.ThumbnailImage(346, 194, Enums.Size.Force), Enums.BlendMode.Over, 51, 414 + j);

                    using var lightLevel = ImageLoader
                        .RenderText(sector.LightLevel.ToString(), "Arial 44", new int[] { 255, 255, 255 });
                    image = image.Composite(lightLevel, Enums.BlendMode.Over, 300, 366 + j);

                    using var sectorName = ImageLoader
                        .RenderText(sector.SectorName.ToString(), "Arial 28", new int[] { 255, 255, 255 });
                    image = image.Composite(sectorName, Enums.BlendMode.Over, 416, 462 + j);

                    using var sectorReward = ImageLoader
                        .RenderText(Localization.TranslationDictionaries.ItemNames[sector.SectorReward],
                        "Arial 28", new int[] { 255, 255, 255 });
                    image = image.Composite(sectorReward, Enums.BlendMode.Over, 416, 560 + j);

                    j += 295;
                }
            }
            {
                int y = 246;

                foreach (var itemList in new List<List<ResourceItem>> { resources.Ada1Resources, resources.Banshee44Resources })
                {
                    int x = 1084;

                    foreach (var item in itemList)
                    {
                        image = await ResourcesParser.DrawItemAsync(loader, image, item, x, y);

                        x += 433;
                    }

                    y += 164;
                }

                foreach (var list in resources.SpiderResources
                    .Select((x, i) => new { Index = i, Value = x })
                    .GroupBy(x => x.Index / 2)
                    .Select(x => x.Select(v => v.Value)))
                {
                    int x = 1517;

                    foreach (var item in list.Reverse())
                    {
                        image = await ResourcesParser.DrawItemAsync(loader, image, item, x, y);

                        x -= 433;
                    }

                    y += 164;
                }
            }

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
