using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Extensions
{
    public record Item
    {
        public string ItemName { get; set; }
        public string ItemIcon { get; set; }
    }

    public record Xur
    {
        public string LocationName { get; set; }
        public string LocationIcon { get; set; }

        public Item[] Items { get; set; }

        public static async Task<Xur> GetXurAsync()
        {
            Xur xur = new();

            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.light.gg/");

            var xurNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"xur-billboard\"]");

            xur.LocationName = xurNode.SelectSingleNode("div[2]/div[1]/span/text()").InnerText.Trim();
            xur.LocationIcon =  Regex.Match(xurNode.SelectSingleNode("div[1][@style]").Attributes["style"].Value, @"(?<=url\(\')(.*)(?=\'\))").Value;

            xur.Items = new Item[4];

            for (int i = 0; i < 4; i++)
            {
                xur.Items[i] = new();

                xur.Items[i].ItemName = xurNode.SelectSingleNode($"div[2]/div[2]/div[{i + 1}]/h3/a/text()").InnerText.Trim();

                xur.Items[i].ItemIcon = xurNode.SelectSingleNode($"div[2]/div[2]/div[{i + 1}]/a/img[@src]").Attributes["src"].Value;
            }

            return xur;
        }
    }
}
