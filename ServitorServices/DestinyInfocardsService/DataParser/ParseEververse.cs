using DestinyInfocardsDatabase.ORM.Eververse;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        public async Task<EververseInventory> ParseEververseAsync(int weekNumber)
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/eververseCalendar");

            List<EververseItem> eververseItems = new();

            var eververseWeekly = htmlDoc.DocumentNode.SelectSingleNode($"//*[contains(@id,'week{weekNumber}_content')]");

            if (eververseWeekly is not null)
            {
                for (int i = 1; i <= 5; i++)
                {
                    var container = eververseWeekly.SelectSingleNode($"//div[@class='eververseSeasonContainer'][{i}]//div[@class='eververseWeeklySeasonContentContainer']");

                    if (container is null)
                        break;

                    for (int j = 1; j <= 7; j++)
                    {
                        var itemContainer = container.SelectSingleNode($"./div[{j}]");

                        if (itemContainer is null)
                            break;

                        var itemIconURL = itemContainer.SelectSingleNode(".//*[@class='manifestItemIcon']").Attributes["src"].Value;
                        var seasonIconURL = itemContainer.SelectSingleNode(".//*[@class='manifestItemWatermarkIcon']")?.Attributes["src"].Value;

                        eververseItems.Add(new EververseItem
                        {
                            ItemCategory = i == 1 ? ItemCategory.Silver : (i < 4 ? ItemCategory.Dust : ItemCategory.ClassBased),
                            ItemIconURL = itemIconURL,
                            ItemSeasonIconURL = seasonIconURL
                        });
                    }
                }
            }

            return new EververseInventory
            {
                EververseItems = eververseItems
            };
        }
    }
}
