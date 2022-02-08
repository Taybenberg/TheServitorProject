using CommonData.Localization;
using DestinyInfocardsDatabase.ORM.Xur;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace DestinyInfocardsService
{
    internal static partial class ImageGenerator
    {
        public static async Task<Image> GetXurImageAsync(XurInventory inventory)
        {
            Image image = Image.Load(Properties.Resources.XurInfocard);

            Font itemName = new Font(SystemFonts.Find("Futura PT Book"), 34);
            Font itemType = new Font(SystemFonts.Find("Futura PT Book"), 23);

            int Yi = 20, Yt1 = 29, Yt2 = 76;
            int interval = 126;

            foreach (var item in inventory.XurItems)
            {
                using Image icon = await ImageLoader.GetImageAsync(item.ItemIconURL);

                image.Mutate(m =>
                {
                    m.DrawImage(icon, new Point(20, Yi), 1);

                    m.DrawText(item.ItemName, itemName, Color.Black, new Point(136, Yt1));
                    m.DrawText(Translation.ItemNames[item.ItemClass], itemType, Color.Black, new Point(143, Yt2));
                });

                Yi += interval;
                Yt1 += interval;
                Yt2 += interval;
            }

            return image;
        }
    }
}
