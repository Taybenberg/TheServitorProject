using DestinyInfocardsDatabase;
using DestinyInfocardsService.Infocards;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<LostSectorsInfocard> GetLostSectorsInfocardAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var infocardsDB = scope.ServiceProvider.GetRequiredService<IInfocardsDB>();

            (var resetBegin, var resetEnd) = GetDailyResetInterval();

            var sectors = await infocardsDB.GetLostSectorAsync(resetBegin, resetEnd);
            var imageLink = sectors?.InfocardImageURL;

            if (imageLink is null)
            {
                sectors = await DataParser.ParseLostSectorsAsync(resetBegin, resetEnd);

                using var image = await ImageGenerator.GetLostSectorsImageAsync(sectors);

                imageLink = await UploadImageAsync(image);

                await infocardsDB.AddLostSectorAsync(sectors with
                {
                    InfocardImageURL = imageLink
                });
            }

            return new LostSectorsInfocard
            {
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime(),
                InfocardImageURL = imageLink
            };
        }
    }
}
