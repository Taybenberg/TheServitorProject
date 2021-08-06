using System;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor
{
    public class ImageFactory : IImageFactory
    {
        private readonly IParserFactory _factory;

        public ImageFactory(IParserFactory factory) => _factory = factory;

        public async Task<Stream> GetDailyResetAsync(string seasonName, int weekNumber)
        {
            var parser = _factory.GetDailyResetParser(seasonName, weekNumber);

            return await parser.GetImageAsync();
        }

        public async Task<Stream> GetEververseAsync(string seasonName, DateTime seasonStart, int weekNumber)
        {
            var parser = _factory.GetEververseParser(seasonName, seasonStart, weekNumber);

            return await parser.GetImageAsync();
        }

        public async Task<Stream> GetLostSectorsAsync()
        {
            var parser = _factory.GetLostSectorsParser();

            return await parser.GetImageAsync();
        }

        public async Task<Stream> GetOsirisAsync()
        {
            var parser = _factory.GetOsirisParser();

            return await parser.GetImageAsync();
        }

        public async Task<Stream> GetResourcesAsync()
        {
            var parser = _factory.GetResourcesParser();

            return await parser.GetImageAsync();
        }

        public async Task<Stream> GetRoadmapAsync()
        {
            var parser = _factory.GetRoadmapParser();

            return await parser.GetImageAsync();
        }

        public async Task<Stream> GetXurAsync(bool getLocation)
        {
            var parser = _factory.GetXurParser(getLocation);

            return await parser.GetImageAsync();
        }
    }
}
