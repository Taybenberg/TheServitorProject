using System.IO;
using System.Threading.Tasks;

namespace Extensions.Parsers
{
    public interface IInventoryParser<T>
    {
        Task<T> GetInventoryAsync();

        Task<Stream> GetImageAsync();
    }
}
