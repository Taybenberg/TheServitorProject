using CommonData.Localization;
using DestinyInfocardsDatabase.ORM.LostSectors;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace DestinyInfocardsService
{
    internal static partial class ImageGenerator
    {
        public static async Task<Image> GetEververseImageAsync(LostSectorsDailyReset lostSectors)
        {
            Image image = Image.Load(Properties.Resources.LostSectorsInfocard);

            Font dateFont = new Font(SystemFonts.Find("Futura PT Book"), 32, FontStyle.Bold);
            Font lightFont = new Font(SystemFonts.Find("Futura PT Book"), 32);
            Font sectorFont = new Font(SystemFonts.Find("Futura PT Book"), 28);

            int i = 0;

            foreach (var sector in lostSectors.LostSectors)
            {
                using Image icon = await ImageLoader.GetImageAsync(sector.ImageURL);
                icon.Mutate(m => m.Resize(362, 210));

                image.Mutate(m =>
                {
                    m.DrawText(sector.LightLevel, lightFont, Color.Black, new Point(291 + i, 14));

                    m.DrawImage(icon, new Point(12 + i, 59), 1);

                    m.DrawText(sector.Name, sectorFont, Color.Black, new Point(18 + i, 305));

                    m.DrawText(Translation.ItemNames[sector.Reward], sectorFont, Color.Black, new Point(18 + i, 377));
                });

                i += 376;
            }

            return image;
        }
    }
}

//        public async Task<Stream> GetImageAsync(int? weekNumber)
//        {
//            var inventory = await GetInventoryAsync(weekNumber);

//            var loader = new ImageLoader();

//            using Image image = Image.Load(ExtensionsRes.EververseItemsBackground);

//            Font font = new Font(SystemFonts.Find("Arial"), 30, FontStyle.Bold);

//            using Image icon = await loader.GetImageAsync(inventory.SeasonIconURL);

//            icon.Mutate(m => m.Resize(192, 192));

//            image.Mutate(m =>
//            {
//                m.DrawText(inventory.Week, font, Color.White, new Point(212, 12));

//                m.DrawText
//                    ($"{inventory.WeekBegin.ToString("dd.MM HH:mm")} – {inventory.WeekEnd.ToString("dd.MM HH:mm")}",
//                    font, Color.White, new Point(212, 73));

//                m.DrawImage(icon, new Point(0, 0), 1);
//            });

//            int[] Y = { 178, 361, 467, 650 };

//            int i = 0;

//            foreach (var itemList in inventory.EververseItems)
//            {
//                int x = 35, y = Y[i++];

//                foreach (var item in itemList)
//                {
//                    await DrawItemAsync(item, loader, image, x, y);

//                    x += 106;
//                }
//            }

//            var ms = new MemoryStream();

//            await image.SaveAsPngAsync(ms);

//            ms.Position = 0;

//            return ms;
//        }

//        internal static async Task DrawItemAsync(EververseItem item, ImageLoader loader, Image image, int x, int y)
//        {
//            if (item.Icon1URL is not null)
//            {
//                using Image icon1 = await loader.GetImageAsync(item.Icon1URL);

//                image.Mutate(m => m.DrawImage(icon1, new Point(x, y), 1));
//            }

//            if (item.Icon2URL is not null)
//            {
//                using Image icon2 = await loader.GetImageAsync(item.Icon2URL);

//                image.Mutate(m => m.DrawImage(icon2, new Point(x, y), 1));
//            }
//        }

//        public async Task<Stream> GetFullInventoryAsync(DateTime seasonEnd)
//        {
//            int weeksTotal = 11;

//            int rows = (int)Math.Sqrt(weeksTotal);
//            int columns = (int)Math.Ceiling((double)weeksTotal / rows);

//            Stream[] streams = new Stream[weeksTotal];

//            Parallel.For(0, weeksTotal, (i) =>
//            {
//                streams[i] = GetImageAsync(i + 16).Result;
//            });

//            using var image = new Image<Rgba32>(columns * 802, rows * 902);

//            int week = 0;

//            for (int i = 0; i < rows; i++)
//            {
//                for (int j = 0; j < columns; j++)
//                {
//                    if (week < weeksTotal)
//                    {
//                        using var str = streams[week];

//                        using var img = Image.Load(str);

//                        image.Mutate(m => m.DrawImage(img, new Point(j * 802, i * 902), 1));
//                    }

//                    week++;
//                }
//            }

//            var ms = new MemoryStream();

//            await image.SaveAsPngAsync(ms);

//            ms.Position = 0;

//            return ms;
//        }
//    }
//}
