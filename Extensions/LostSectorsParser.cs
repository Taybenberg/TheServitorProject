using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public static class LostSectorsParser
    {
        public static async Task<Stream> GetLostSectorsAsync()
        {
            using Image image = Image.Load(ExtensionsRes.LostSectorsBackground);

            {
                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/");
                
                var sectorMaster = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'bl_lost_sector_master')]");

                Console.WriteLine(sectorMaster.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText); //reward
                Console.WriteLine(sectorMaster.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText); //name
                Console.WriteLine(sectorMaster.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value); //icon

                var sectorLegend = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@id,'bl_lost_sector_legend')]");

                Console.WriteLine(sectorLegend.SelectSingleNode("./div[13]/div[4]/div[1]/p[1]").InnerText);
                Console.WriteLine(sectorLegend.SelectSingleNode("./div[12]/div[3]/p[2]").InnerText);
                Console.WriteLine(sectorLegend.SelectSingleNode("./div[12]/div[1]/div/div/img").Attributes["src"].Value);

                Font itemName = new Font(SystemFonts.Find("Arial"), 34);
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
