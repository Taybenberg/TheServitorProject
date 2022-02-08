using DestinyInfocardsDatabase.ORM.Resources;
using HtmlAgilityPack;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        public async Task<VendorsDailyReset> ParseResourcesAsync()
        {
            var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://www.todayindestiny.com/vendors");

            int[][] vendorsIntervals = { new int[] { 3, 1, 1 }, new int[] { 7, 2, 2 } };
            string[] vendorIDs = { "vendors_863940356_content", "vendors_350061650_content", "vendors_672118013_content" };

            List<ResourceItem> resourceItems = new();

            for (int i = 0; i < 3; i++)
            {
                var vendor = htmlDoc.GetElementbyId(vendorIDs[i]);

                if (vendor is not null)
                {
                    var materialsExchange = vendor.SelectSingleNode(".//div[@identifier='category_materials_exchange']");

                    for (int j = vendorsIntervals[0][i]; j <= vendorsIntervals[1][i]; j++)
                    {
                        var vendorContainer = materialsExchange.SelectSingleNode($".//div[@class='vendorInventoryItemContainer'][{j}]");

                        var resourceName = vendorContainer.SelectSingleNode($".//*[@class='itemTooltip_itemName']").InnerText.Replace("Purchase ", string.Empty);

                        var resourceIconURL = vendorContainer.SelectSingleNode($".//*[@class='manifestItemIcon']").Attributes["src"].Value;

                        var sesourceCurrencyQuantity = vendorContainer.SelectSingleNode($".//*[@class='tooltipCostQuantity']").InnerText;

                        var resourceCurrencyIconURL = vendorContainer.SelectSingleNode($".//*[@class='tooltipCostImage']").Attributes["src"].Value;

                        resourceItems.Add(new ResourceItem
                        {
                            DestinyVendor = (DestinyVendor)i,
                            ResourceName = resourceName,
                            ResourceIconURL = resourceIconURL,
                            ResourceCurrencyQuantity = sesourceCurrencyQuantity,
                            ResourceCurrencyIconURL = resourceCurrencyIconURL
                        });
                    }
                }
            }

            return new VendorsDailyReset
            {
                ResourceItems = resourceItems
            };
        }
    }
}
