using System.IO;
using System.Threading.Tasks;

namespace Extensions.Parsers
{
    interface IInventoryParser
    {
        Task<Stream> GetImageAsync();
    }
}
