using System;
using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    public class RoadmapParser : IInventoryParser
    {
        public async Task<Stream> GetImageAsync() =>
            DateTime.Now.ToString("dd.MM") switch
            {
                "11.05" => new MemoryStream(ExtensionsRes.RoadmapMay11),
                "14.05" => new MemoryStream(ExtensionsRes.RoadmapMay14),
                "18.05" => new MemoryStream(ExtensionsRes.RoadmapMay18),
                "22.05" => new MemoryStream(ExtensionsRes.RoadmapMay22),
                "25.05" => new MemoryStream(ExtensionsRes.RoadmapMay25),
                "01.06" => new MemoryStream(ExtensionsRes.RoadmapJun1),
                "08.06" => new MemoryStream(ExtensionsRes.RoadmapJun8),
                "15.06" => new MemoryStream(ExtensionsRes.RoadmapJun15),
                "22.06" => new MemoryStream(ExtensionsRes.RoadmapJun22),
                "29.06" => new MemoryStream(ExtensionsRes.RoadmapJun29),
                "06.07" => new MemoryStream(ExtensionsRes.RoadmapJul6),
                "03.08" => new MemoryStream(ExtensionsRes.RoadmapAug3),
                "10.08" => new MemoryStream(ExtensionsRes.RoadmapAug10),
                _ => null
            };
    }
}
