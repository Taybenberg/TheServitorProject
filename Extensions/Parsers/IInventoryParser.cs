using System.IO;
using System.Threading.Tasks;

namespace Extensions.Parsers
{
    public interface IInventoryParser
    {
        Task<Stream> GetImageAsync();
    }
}
