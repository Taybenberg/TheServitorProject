using DestinyInfocardsDatabase.ORM.Xur;
using HtmlAgilityPack;
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

        public static async Task<XurInventory> ParseXurInventoryAsync(DateTime weeklyResetBegin, DateTime weeklyResetEnd)
        {
            return new XurInventory();
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

//namespace DataProcessor.Parsers
//{
//    public class XurParser : IInventoryParser<XurInventory>
//    {
//        private readonly IApiClient _apiClient;

//        private readonly bool _getLocation;

//        public XurParser(IApiClient apiClient, bool getLocation) =>
//            (_apiClient, _getLocation) = (apiClient, getLocation);

//        public async Task<XurInventory> GetInventoryAsync()
//        {
//            var inventory = new XurInventory();

//            var items = await _apiClient.GetXurItemsAsync();

//            string location = string.Empty;

//            if (_getLocation)
//            {
//                var htmlDoc = await new HtmlWeb().LoadFromWebAsync("https://xur.wiki/");
//                location = HttpUtility.HtmlEncode(htmlDoc.DocumentNode.SelectSingleNode("/html/body/div[1]/div/div/div[1]/div/div/h1")?.InnerText.Trim() ?? string.Empty);
//            }

//            if (string.IsNullOrWhiteSpace(location))
//                location = "Невизначено";

//            inventory.Location = location;

//            try
//            {
//                foreach (var item in items.Reverse())
//                {
//                    inventory.XurItems.Add(new XurItem
//                    {
//                        ItemName = item.ItemName,
//                        ItemClass = Translation.ItemNames[item.ItemTypeAndTier],
//                        ItemIconURL = item.ItemIconUrl
//                    });
//                }
//            }
//            catch { }

//            return inventory;
//        }