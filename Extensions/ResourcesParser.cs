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
    public static class ResourcesParser
    {
        public static async Task<Stream> GetResourcesAsync()
        {
            using var loader = new ImageLoader();

            using Image image = Image.Load(ExtensionsRes.ResourcesBackground);

            {
                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/vendors");
                /*
                /html/body/main/div/div[15]
                /html/body/main/div/div[15]/div[2]/div[5]/div
                /html/body/main/div/div[15]/div[2]/div[5]/div/div[1]/div[1]/div/img
                */

                var banshee44 = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@hash,'672118013')]");
                var bansheeContainer = banshee44.SelectSingleNode("./div[2]/div[4]/div");

                /*
                for (int i = 1; i <= 7; i++)
                {
                    var iconUrl = bansheeContainer.SelectSingleNode($"/div[{i}]/div[1]/div/img").Attributes["src"].Value;
                }
                */

                var spider = htmlDoc.DocumentNode.SelectSingleNode("//*[contains(@hash,'863940356')]");
                var spiderContainer = spider.SelectSingleNode("./div[2]/div[5]/div");
                
                for (int i = 1; i <= 7; i++)
                {
                    var iconUrl = spiderContainer.SelectSingleNode($"./div[{i}]/div[1]/div/img").Attributes["src"].Value;

                    var resIconUrl = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]/div/div/img").Attributes["src"].Value;

                    var resName = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]/div/p[1]").InnerText;

                    var resQuantity = spiderContainer.SelectSingleNode($"./div[{i}]/div[3]/div[2]/div[2]/div/p[2]").InnerText;

                    Console.WriteLine($"{iconUrl}\n{resIconUrl}\n{resName}\n{resQuantity}\n\n\n");
                }


                Font itemName = new Font(SystemFonts.Find("Arial"), 34);
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
