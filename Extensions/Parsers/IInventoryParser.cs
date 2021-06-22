using System.IO;
using System.Threading.Tasks;

namespace Extensions
{
    interface IInventoryParser
    {
        Task<Stream> GetImageAsync();
    }
}
