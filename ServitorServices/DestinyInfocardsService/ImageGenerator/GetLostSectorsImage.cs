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
        public static async Task<Image> GetLostSectorsImageAsync(LostSectorsDailyReset lostSectors)
        {
            Image image = Image.Load(Properties.Resources.LostSectorsInfocard);

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

            return image;
        }
    }
}
