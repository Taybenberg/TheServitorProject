using System;
using System.IO;
using System.Threading.Tasks;

namespace DataProcessor
{
    public interface IImageFactory
    {
        Task<Stream> GetDailyResetAsync(string seasonName, int weekNumber);

        Task<Stream> GetWeeklyResetAsync(string seasonName, DateTime seasonStart, int weekNumber);

        Task<Stream> GetEververseAsync(string seasonName, DateTime seasonStart, int weekNumber);

        Task<Stream> GetEververseFullAsync(string seasonName, DateTime seasonStart, DateTime seasonEnd);

        Task<Stream> GetLostSectorsAsync();

        Task<Stream> GetResourcesAsync();

        Task<Stream> GetXurAsync(bool getLocation);
    }
}
