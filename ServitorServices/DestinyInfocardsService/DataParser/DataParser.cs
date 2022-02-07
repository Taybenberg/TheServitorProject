using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    internal partial class DataParser
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DataParser(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;
    }
}
