using DataProcessor.DatabaseWrapper;
using DataProcessor.Parsers.Inventory;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor.Parsers
{
    public class WeeklyResetParser : IInventoryParser<(EververseInventory, WeeklyMilestone)>
    {
        private readonly IDatabaseWrapperFactory _wrapperFactory;

        private readonly string _seasonName;
        private readonly DateTime _seasonStart;
        private readonly int _weekNumber;

        public WeeklyResetParser(IDatabaseWrapperFactory wrapperFactory, string seasonName, DateTime seasonStart, int weekNumber) =>
            (_wrapperFactory, _seasonName, _seasonStart, _weekNumber) = (wrapperFactory, seasonName, seasonStart, weekNumber);

        public async Task<(EververseInventory, WeeklyMilestone)> GetInventoryAsync()
        {
            var ev = new EververseParser(_seasonName, _seasonStart, _weekNumber);

            return (await ev.GetInventoryAsync(), await _wrapperFactory.GetWeeklyMilestoneAsync());
        }

        public async Task<Stream> GetImageAsync()
        {
            (var eververse, var milestone) = await GetInventoryAsync();

            var loader = new ImageLoader();

            using Image image = Image.Load(ExtensionsRes.WeeklyResetBackground);

            Font titleFont = new Font(SystemFonts.Find("Arial"), 52);

            image.Mutate(m =>
            {
                m.DrawText($"{eververse.WeekBegin.ToString("dd.MM HH:mm")} – {eververse.WeekEnd.ToString("dd.MM HH:mm")}",
                titleFont, Color.White, new Point(480, 29));

                m.DrawText(eververse.Week, titleFont, Color.White, new Point(38, 105));
            });

            {
                int[] Y = { 375, 580, 692, 897 };

                int i = 0;

                foreach (var itemList in eververse.EververseItems)
                {
                    int x = 63, y = Y[i++];

                    foreach (var item in itemList)
                    {
                        await EververseParser.DrawItemAsync(item, loader, image, x, y);

                        x += 112;
                    }
                }
            }
            {
                using Image icon = await loader.GetImageAsync(milestone.NightfallImageURL);
                icon.Mutate(m => m.Resize(560, 315));

                image.Mutate(m =>
                {
                    Font font = new Font(SystemFonts.Find("Arial"), 36);

                    if (milestone.IsIronBannerAvailable)
                    {
                        m.DrawText(new DrawingOptions
                        {
                            TextOptions = new TextOptions
                            {
                                HorizontalAlignment = HorizontalAlignment.Center
                            }
                        }, $"Доступний\n{milestone.IronBannerName}!", font, Color.White, new Point(1355, 84));
                    }
                    else
                    {
                        Font bannerFont = new Font(SystemFonts.Find("Arial"), 52);

                        m.DrawText("Н/Д", bannerFont, Color.White, new Point(1314, 95));
                    }

                    m.DrawImage(icon, new Point(966, 366), 1);

                    int y = 299;

                    foreach (var field in milestone.Fields)
                    {
                        m.DrawText(field.Value, font, Color.White, new Point(1051, y));

                        y += 499;
                    }
                });
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
