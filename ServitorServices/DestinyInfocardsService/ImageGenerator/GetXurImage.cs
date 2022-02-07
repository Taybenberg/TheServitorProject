using CommonData.Localization;
using DestinyInfocardsDatabase.ORM.Xur;
using HtmlAgilityPack;
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
            //var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            Image image = Image.Load(Properties.Resources.LostSectorsInfocard);

            /*
            Font dateFont = new Font(SystemFonts.Find("Arial"), 32, FontStyle.Bold);
            Font lightFont = new Font(SystemFonts.Find("Arial"), 32);
            Font sectorFont = new Font(SystemFonts.Find("Arial"), 28);

            int i = 0;

            foreach (var sector in lostSectors.LostSectors)
            {
                using Image icon = await ImageLoader.GetImageAsync(sector.ImageURL);
                icon.Mutate(m => m.Resize(362, 210));

                image.Mutate(m =>
                {
                    m.DrawText(sector.LightLevel, lightFont, Color.Black, new Point(291 + i, 18));

                    m.DrawImage(icon, new Point(12 + i, 59), 1);

                    m.DrawText(sector.Name, sectorFont, Color.Black, new Point(18 + i, 308));

                    m.DrawText(Translation.ItemNames[sector.Reward], sectorFont, Color.Black, new Point(18 + i, 380));
                });

                i += 376;
            }
            */

            return image;
        }
    }
}
//using CommonData.Localization;
//using DataProcessor.Parsers.Inventory;
//using HtmlAgilityPack;
//using SixLabors.Fonts;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Drawing.Processing;
//using SixLabors.ImageSharp.Processing;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//        public async Task<Stream> GetImageAsync()
//        {
//            var inventory = await GetInventoryAsync();

//            var loader = new ImageLoader();

//            using Image image = Image.Load(ExtensionsRes.XurItemsBackground);

//            Font locationFont = new Font(SystemFonts.Find("Arial"), 28, FontStyle.Bold);

//            image.Mutate(m => m.DrawText(inventory.Location, locationFont, Color.Black, new Point(257, 574)));

//            Font itemName = new Font(SystemFonts.Find("Arial"), 34);
//            Font itemType = new Font(SystemFonts.Find("Arial"), 23);

//            int Yi = 30, Yt1 = 43, Yt2 = 89;
//            int interval = 136;

//            foreach (var item in inventory.XurItems)
//            {
//                using Image icon = await loader.GetImageAsync(item.ItemIconURL);

//                image.Mutate(m =>
//                {
//                    m.DrawImage(icon, new Point(30, Yi), 1);

//                    m.DrawText(item.ItemName, itemName, Color.Black, new Point(146, Yt1));
//                    m.DrawText(item.ItemClass, itemType, Color.Black, new Point(153, Yt2));
//                });

//                Yi += interval;
//                Yt1 += interval;
//                Yt2 += interval;
//            }

//            var ms = new MemoryStream();

//            await image.SaveAsPngAsync(ms);

//            ms.Position = 0;

//            return ms;
//        }
//    }
//}
