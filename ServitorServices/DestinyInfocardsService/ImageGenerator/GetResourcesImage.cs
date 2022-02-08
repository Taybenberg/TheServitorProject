using DestinyInfocardsDatabase.ORM.Resources;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace DestinyInfocardsService
{
    internal static partial class ImageGenerator
    {
        public static async Task<Image> GetResourcesImageAsync(VendorsDailyReset resources)
        {
            Image image = Image.Load(Properties.Resources.ResourcesInfocard);

            Font font = new Font(SystemFonts.Find("Futura PT Book"), 28);

            int interval = 126, y;

            y = 79;
            foreach (var item in resources.ResourceItems.Where(x => x.DestinyVendor == DestinyVendor.Spider))
            {
                await DrawItemAsync(image, font, item, 32, y);
                y += interval;
            }

            y = 79;
            foreach (var item in resources.ResourceItems.Where(x => x.DestinyVendor == DestinyVendor.Ada1))
            {
                await DrawItemAsync(image, font, item, 701, y);
                y += interval;
            }

            y = 457;
            foreach (var item in resources.ResourceItems.Where(x => x.DestinyVendor == DestinyVendor.Bunshee44))
            {
                await DrawItemAsync(image, font, item, 701, y);
                y += interval;
            }

            return image;
        }

        private static async Task DrawItemAsync(Image image, Font font, ResourceItem item, int x, int y)
        {
            using Image currencyIcon = await ImageLoader.GetImageAsync(item.ResourceCurrencyIconURL);

            currencyIcon.Mutate(m => m.Resize(40, 40));

            using Image resIcon = await ImageLoader.GetImageAsync(item.ResourceIconURL);

            image.Mutate(m =>
            {
                m.DrawImage(resIcon, new Point(x, y), 1);

                m.DrawText(item.ResourceName, font, Color.Black, new Point(107 + x, 7 + y));

                m.DrawImage(currencyIcon, new Point(107 + x, 50 + y), 1);

                m.DrawText(item.ResourceCurrencyQuantity, font, Color.Black, new Point(156 + x, 52 + y));
            });
        }
    }
}
