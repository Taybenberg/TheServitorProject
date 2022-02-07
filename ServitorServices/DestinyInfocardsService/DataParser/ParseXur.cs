using BungieSharper.Client;
using BungieSharper.Entities.Destiny;
using DestinyInfocardsDatabase.ORM.Xur;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using System.Web;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        public async Task<string?> ParseXurLocationAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://xur.wiki/");

            var location = htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[1]/div/div/h1")?.InnerText.Trim();

            if (location is not null)
                return HttpUtility.HtmlEncode(location);

            return null;
        }

        public async Task<XurInventory> ParseXurInventoryAsync(DateTime weeklyResetBegin, DateTime weeklyResetEnd)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var vendors = await apiClient.Api.Destiny2_GetPublicVendors(new DestinyComponentType[] { DestinyComponentType.VendorSales });

            if (!vendors.Sales.Data.ContainsKey(2190858386))
            {
                return new XurInventory
                {
                    WeeklyResetBegin = weeklyResetBegin,
                    WeeklyResetEnd = weeklyResetEnd,
                    XurItems = new List<XurItem>()
                };
            }

            var tasks = vendors.Sales.Data[2190858386]
                .SaleItems.Skip(1).Take(4).Reverse()
                .Select(x => Task.Run(async () =>
                {
                    //var item = await apiClient.Api.Destiny2_GetDestinyEntityDefinition("DestinyInventoryItemDefinition", x.Value.ItemHash);

                    return new XurItem
                    {
                        ItemName = "", //rawItemDetails.displayProperties.name
                        ItemClass = "", //rawItemDetails.itemTypeAndTierDisplayName;
                        ItemIconURL = "", //BungieNetApiClient.BUNGIE_NET_URL.AppendPathSegment(rawItemDetails.displayProperties.icon);
                    };
                }));

            await Task.WhenAll(tasks);

            return new XurInventory
            {
                WeeklyResetBegin = weeklyResetBegin,
                WeeklyResetEnd = weeklyResetEnd,
                XurItems = new List<XurItem>() //tasks.Select(x => x.Result).ToList()
            };
        }
    }
}
