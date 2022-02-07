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

            var dbSectors = await infocardsDB.GetLostSectorAsync(resetBegin, resetEnd);
            var imageLink = dbSectors?.InfocardImageURL;

            if (imageLink is null)
            {
                var sectors = await DataParser.ParseLostSectorsAsync(resetBegin, resetEnd);

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
