using DataProcessor.DatabaseWrapper;
using DataProcessor.Parsers.Inventory;
using NetVips;
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

            Image image = Image.NewFromBuffer(ExtensionsRes.WeeklyResetBackground);

            using var weekBegin = ImageLoader
                .RenderText
                ($"{eververse.WeekBegin.ToString("dd.MM HH:mm")} – {eververse.WeekEnd.ToString("dd.MM HH:mm")}",
                "Arial 52", new int[] { 255, 255, 255 });
            image = image.Composite(weekBegin, Enums.BlendMode.Over, 482, 40);

            using var week = ImageLoader
                .RenderText(eververse.Week, "Arial 52", new int[] { 255, 255, 255 });
            image = image.Composite(week, Enums.BlendMode.Over, 40, 111);

            {
                int[] Y = { 375, 580, 692, 897 };

                int i = 0;

                foreach (var itemList in eververse.EververseItems)
                {
                    int x = 63, y = Y[i++];

                    foreach (var item in itemList)
                    {
                        image = await EververseParser.DrawItemAsync(loader, image, item, x, y);

                        x += 112;
                    }
                }
            }
            {
                using var icon = await loader.GetImageAsync(milestone.NightfallImageURL);
                image = image.Composite
                    (icon.ThumbnailImage(560, 315, Enums.Size.Force), Enums.BlendMode.Over, 971, 366);

                if (milestone.IsIronBannerAvailable)
                {
                    using var bannerText = ImageLoader
                        .RenderText
                        ($"Доступний\n{milestone.IronBannerName}!",
                        "Arial 36", new int[] { 255, 255, 255 }, Enums.Align.Centre);
                    image = image.Composite(bannerText, Enums.BlendMode.Over, 1240, 90);
                }
                else
                {
                    using var bannerText = ImageLoader
                        .RenderText("Н/Д", "Arial 52", new int[] { 255, 255, 255 });
                    image = image.Composite(bannerText, Enums.BlendMode.Over, 1316, 104);
                }

                int y = 305;

                foreach (var field in milestone.Fields)
                {
                    using var value = ImageLoader
                        .RenderText(field.Value, "Arial 36", new int[] { 255, 255, 255 });
                    image = image.Composite(value, Enums.BlendMode.Over, 1053, y);

                    y += 499;
                }
            }

            var ms = new MemoryStream();

            image.PngsaveStream(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
