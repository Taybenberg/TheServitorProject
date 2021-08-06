using System;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor
{
    public interface IImageFactory
    {
        Task<Stream> GetDailyResetAsync(string seasonName, int weekNumber);

        Task<Stream> GetEververseAsync(string seasonName, DateTime seasonStart, int weekNumber);

        Task<Stream> GetLostSectorsAsync();

        Task<Stream> GetResourcesAsync();

        Task<Stream> GetOsirisAsync();

        Task<Stream> GetRoadmapAsync();

        Task<Stream> GetXurAsync(bool getLocation);
    }
}
