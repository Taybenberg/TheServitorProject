using DestinyInfocardsDatabase.ORM.Eververse;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace DestinyInfocardsService
{
    internal static partial class ImageGenerator
    {
        public static async Task<Image> GetEververseImageAsync(EververseInventory inventory)
        {
            var silverItems = inventory.EververseItems.Where(x => x.ItemCategory == ItemCategory.Silver);
            var dustItems = inventory.EververseItems.Where(x => x.ItemCategory == ItemCategory.Dust);
            var classBasedItems = inventory.EververseItems.Where(x => x.ItemCategory == ItemCategory.ClassBased);

            var count = classBasedItems.Count();

            Image image = count == 0 ?
                Image.Load(Properties.Resources.EververseInfocardSmall) :
                (count > 7 ?
                    Image.Load(Properties.Resources.EververseInfocardHuge) :
                    Image.Load(Properties.Resources.EververseInfocardRegular));

            await DrawItemsAsync(image, silverItems, 69);
            await DrawItemsAsync(image, dustItems, 242);
            await DrawItemsAsync(image, classBasedItems, 521);

            return image;
        }

        private static async Task DrawItemsAsync(Image image, IEnumerable<EververseItem> items, int y)
        {
            int interval = 106, counter = 0, x = 22;

            foreach (var item in items)
            {
                await DrawItemAsync(image, item, x, y);

                if (++counter > 7)
                {
                    counter = 0;

                    x = 22;

                    y += interval;
                }
                else
                    x += interval;
            }
        }

        private static async Task DrawItemAsync(Image image, EververseItem item, int x, int y)
        {
            using var itemIcon = await ImageLoader.GetImageAsync(item.ItemIconURL);

            image.Mutate(m => m.DrawImage(itemIcon, new Point(x, y), 1));

            if (item.ItemSeasonIconURL is not null)
            {
                using var seasonIcon = await ImageLoader.GetImageAsync(item.ItemSeasonIconURL);

                image.Mutate(m => m.DrawImage(seasonIcon, new Point(x, y), 1));
            }
        }
    }
}
