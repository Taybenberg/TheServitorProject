using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataProcessor
{
    public interface IImageFactory
    {
        Task<Stream> GetEververseAsync(string seasonName, DateTime seasonStart, int weekNumber);

        Task<Stream> GetLostSectorsAsync();

        Task<Stream> GetResourcesAsync();

        Task<Stream> GetOsirisAsync();

        Task<Stream> GetRoadmapAsync();

        Task<Stream> GetXurAsync(bool getLocation);
    }
}
