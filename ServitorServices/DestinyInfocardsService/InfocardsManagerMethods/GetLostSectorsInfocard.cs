using DestinyInfocardsDatabase;
using DestinyInfocardsService.Infocards;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<LostSectorsInfocard> GetLostSectorsInfocardAsync()
        {
            (var resetBegin, var resetEnd) = GetDailyResetInterval();

            using var scope = _scopeFactory.CreateScope();

            var infocardsDB = scope.ServiceProvider.GetRequiredService<IInfocardsDB>();

            using var image = await ImageGenerator.GetLostSectorsImageAsync();

            var imageLink = await UploadImageAsync(image);

            return new LostSectorsInfocard
            {
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime(),
                InfocardImageURL = imageLink
            };
        }
    }
}
