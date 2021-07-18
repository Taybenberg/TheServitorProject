using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    internal interface IWrapper
    {
        Task InitAsync();
    }
}
