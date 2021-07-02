using DataProcessor.Parsers.Inventory;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor.Parsers
{
    public class RoadmapParser : IInventoryParser<RoadmapInventory>
    {
        public async Task<RoadmapInventory> GetInventoryAsync()
        {
            return new RoadmapInventory
            {
                RoadmapImage = DateTime.Now.ToString("dd.MM") switch
                {
                    "11.05" => ExtensionsRes.RoadmapMay11,
                    "14.05" => ExtensionsRes.RoadmapMay14,
                    "18.05" => ExtensionsRes.RoadmapMay18,
                    "22.05" => ExtensionsRes.RoadmapMay22,
                    "25.05" => ExtensionsRes.RoadmapMay25,
                    "01.06" => ExtensionsRes.RoadmapJun1,
                    "08.06" => ExtensionsRes.RoadmapJun8,
                    "15.06" => ExtensionsRes.RoadmapJun15,
                    "22.06" => ExtensionsRes.RoadmapJun22,
                    "29.06" => ExtensionsRes.RoadmapJun29,
                    "06.07" => ExtensionsRes.RoadmapJul6,
                    "03.08" => ExtensionsRes.RoadmapAug3,
                    "10.08" => ExtensionsRes.RoadmapAug10,
                    _ => null
                }
            };
        }

        public async Task<Stream> GetImageAsync()
        {
            var inventory = await GetInventoryAsync();

            return inventory.RoadmapImage is not null ?
                new MemoryStream(inventory.RoadmapImage) : null;
        }
    }
}
