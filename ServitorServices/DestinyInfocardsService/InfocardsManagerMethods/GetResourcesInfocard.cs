using DestinyInfocardsDatabase;
using DestinyInfocardsService.Infocards;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<ResourcesInfocard> GetResourcesInfocardAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var infocardsDB = scope.ServiceProvider.GetRequiredService<IInfocardsDB>();

            (var resetBegin, var resetEnd) = GetDailyResetInterval();

            return new ResourcesInfocard
            {
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime(),
            };
        }
    }
}
