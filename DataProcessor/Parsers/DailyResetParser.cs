using DataProcessor.Parsers.Inventory;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
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

            using Image image = Image.Load(ExtensionsRes.DailyResetBackground);

            Font titleFont = new Font(SystemFonts.Find("Arial"), 52);

            image.Mutate(m =>
            {
                m.DrawText($"{sectors.ResetBegin.ToString("dd.MM HH:mm")} – {sectors.ResetEnd.ToString("dd.MM HH:mm")}",
                titleFont, Color.White, new Point(400, 28));

                m.DrawText($"Тиждень {_weekNumber}. Сезон \"{_seasonName}\"",
                titleFont, Color.White, new Point(38, 104));
            });

            {
                Font lightFont = new Font(SystemFonts.Find("Arial"), 44);
                Font sectorFont = new Font(SystemFonts.Find("Arial"), 28);

                int j = 0;

                foreach (var sector in sectors.LostSectors)
                {
                    using Image icon = await loader.GetImageAsync(sector.SectorIconURL);

                    icon.Mutate(m => m.Resize(346, 194));

                    image.Mutate(m =>
                    {
                        m.DrawText(sector.LightLevel.ToString(), lightFont, Color.White, new Point(296, 358 + j));

                        m.DrawImage(icon, new Point(51, 414 + j), 1);

                        m.DrawText(sector.SectorName, sectorFont, Color.White, new Point(414, 456 + j));

                        m.DrawText(Localization.TranslationDictionaries.ItemNames[sector.SectorReward], sectorFont, Color.White, new Point(414, 554 + j));
                    });

                    j += 295;
                }
            }
            {
                Font font = new Font(SystemFonts.Find("Arial"), 23, FontStyle.Regular);

                int y = 246;

                foreach (var itemList in new List<List<ResourceItem>> { resources.Ada1Resources, resources.Banshee44Resources })
                {
                    int x = 1084;

                    foreach (var item in itemList)
                    {
                        await ResourcesParser.DrawItemAsync(item, loader, image, font, x, y);

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
                        await ResourcesParser.DrawItemAsync(item, loader, image, font, x, y);

                        x -= 433;
                    }

                    y += 164;
                }
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
