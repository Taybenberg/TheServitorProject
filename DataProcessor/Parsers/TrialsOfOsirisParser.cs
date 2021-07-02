using DataProcessor.Parsers.Inventory;
using Flurl.Http;
using HtmlAgilityPack;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace DataProcessor.Parsers
{
    public class TrialsOfOsirisParser : IInventoryParser<OsirisInventory>
    {
        public async Task<OsirisInventory> GetInventoryAsync()
        {
            var inventory = new OsirisInventory();

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

            var trialsBillboard = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"trials-billboard\"]/div[2]");

            if (trialsBillboard is not null)
            {
                var location = HttpUtility.HtmlEncode(trialsBillboard.SelectSingleNode("./div[1]/span/text()")?.InnerText.Trim() ?? string.Empty);

                if (string.IsNullOrWhiteSpace(location))
                    location = "Невизначено";

                inventory.Location = location;

                for (int i = 1; i <= 4; i++)
                {
                    List<string> itemList = new();

                    for (int j = 1; j <= 3; j++)
                    {
                        var node = trialsBillboard.SelectSingleNode($"./div[2]/span[{i}]/a[{j}]/img[@src]");

                        if (node is null)
                            break;

                        itemList.Add(node.Attributes["src"].Value);
                    }

                    inventory.IconURLs.Add(itemList);
                }
            }
            else
                return inventory with { Location = "Невизначено" };

            return inventory;
        }

        public async Task<Stream> GetImageAsync()
        {
            using Image image = Image.Load(ExtensionsRes.TrialsItemsBackground);

            var inventory = await GetInventoryAsync();

            Font locationFont = new Font(SystemFonts.Find("Arial"), 28, FontStyle.Bold);
            image.Mutate(m => m.DrawText(inventory.Location, locationFont, Color.White, new Point(257, 574)));

            int y = 30;

            foreach (var list in inventory.IconURLs)
            {
                int x = 252;

                foreach (var link in list)
                {
                    using var stream = await link.GetStreamAsync();
                    using Image icon = await Image.LoadAsync(stream);
                    image.Mutate(m => m.DrawImage(icon, new Point(x, y), 1));

                    x += 121;
                }

                y += 136;
            }

            var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            return ms;
        }
    }
}
