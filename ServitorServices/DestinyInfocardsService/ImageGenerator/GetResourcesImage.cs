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
        public static async Task<Image> GetResourcesImageAsync(LostSectorsDailyReset lostSectors)
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



//        public async Task<Stream> GetImageAsync()
//        {
//            var inventory = await GetInventoryAsync();

//            var loader = new ImageLoader();

//            using Image image = Image.Load(ExtensionsRes.ResourcesBackground);

//            Font dateFont = new Font(SystemFonts.Find("Arial"), 24, FontStyle.Bold);

//            image.Mutate(m => m
//                .DrawText($"{inventory.ResetBegin.ToString("dd.MM HH:mm")} – {inventory.ResetEnd.ToString("dd.MM HH:mm")}",
//                dateFont, Color.White, new Point(21, 81)));

//            Font font = new Font(SystemFonts.Find("Arial"), 23, FontStyle.Regular);

//            int x = 22, y = 225;

//            foreach (var item in inventory.SpiderResources)
//            {
//                await DrawItemAsync(item, loader, image, font, x, y);

//                y += 135;
//            }

//            y = 263;

//            foreach (var itemList in new List<List<ResourceItem>> { inventory.Ada1Resources, inventory.Banshee44Resources })
//            {
//                x = 459;

//                foreach (var item in itemList)
//                {
//                    await DrawItemAsync(item, loader, image, font, x, y);

//                    x += 433;
//                }

//                y += 281;
//            }

//            var ms = new MemoryStream();

//            await image.SaveAsPngAsync(ms);

//            ms.Position = 0;

//            return ms;
//        }

//        internal static async Task DrawItemAsync(ResourceItem item, ImageLoader loader, Image image, Font font, int x, int y)
//        {
//            using Image currencyIcon = await loader.GetImageAsync(item.ResourceCurrencyIconURL);

//            currencyIcon.Mutate(m => m.Resize(40, 40));

//            using Image resIcon = await loader.GetImageAsync(item.ResourceIconURL);

//            image.Mutate(m =>
//            {
//                m.DrawImage(resIcon, new Point(x, y), 1);

//                m.DrawText(item.ResourceName, font, Color.Black, new Point(107 + x, 7 + y));

//                m.DrawImage(currencyIcon, new Point(107 + x, 50 + y), 1);

//                m.DrawText(item.ResourceCurrencyQuantity, font, Color.Black, new Point(156 + x, 58 + y));
//            });
//        }
//    }
//}
