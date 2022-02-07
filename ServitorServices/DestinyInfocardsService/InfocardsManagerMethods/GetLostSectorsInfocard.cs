using CommonData.Localization;
using DestinyInfocardsService.Infocards;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<LostSectorsInfocard> GetLostSectorsInfocardAsync()
        {
            using Image image = Image.Load(Properties.Resources.LostSectorsInfocard);

            Font dateFont = new Font(SystemFonts.Find("Arial"), 32, FontStyle.Bold);
            Font lightFont = new Font(SystemFonts.Find("Arial"), 32);
            Font sectorFont = new Font(SystemFonts.Find("Arial"), 28);

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");

            int i = 0;

            foreach (var sector in new string[] { "//*[contains(@id,'bl_lost_sector_legend')]", "//*[contains(@id,'bl_lost_sector_master')]" })
            {
                var node = htmlDoc.DocumentNode.SelectSingleNode(sector);

                if (node is not null)
                {
                    var lightLevel = node.SelectSingleNode("./div[14]/div[1]").InnerText;
                    var sectorImageURL = node.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value;
                    var sectorName = node.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText;
                    var sectorReward = node.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText[10..^7];

                    using Image icon = await ImageLoader.GetImageAsync(sectorImageURL);
                    icon.Mutate(m => m.Resize(362, 210));

                    image.Mutate(m =>
                    {
                        m.DrawText(lightLevel, lightFont, Color.Black, new Point(291 + i, 18));

                        m.DrawImage(icon, new Point(12 + i, 59), 1);

                        m.DrawText(sectorName, sectorFont, Color.Black, new Point(18 + i, 308));

                        m.DrawText(Translation.ItemNames[sectorReward], sectorFont, Color.Black, new Point(18 + i, 380));
                    });
                }

                i += 376;
            }

            (var resetBegin, var resetEnd) = GetDailyResetInterval();
            var imageLink = await UploadImageAsync(image);

            return new LostSectorsInfocard
            {
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime(),
                InfocardImageURL = imageLink
            };
        }
    }
}
