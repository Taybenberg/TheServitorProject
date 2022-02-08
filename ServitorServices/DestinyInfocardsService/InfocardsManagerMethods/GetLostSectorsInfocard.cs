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
                var dataParser = new DataParser(_scopeFactory);

                sectors = await dataParser.ParseLostSectorsAsync();

                using var image = await ImageGenerator.GetLostSectorsImageAsync(sectors);

                imageLink = await UploadImageAsync(image);

                if (sectors.LostSectors.Any())
                {
                    await infocardsDB.AddLostSectorAsync(sectors with
                    {
                        DailyResetBegin = resetBegin,
                        DailyResetEnd = resetEnd,
                        SeasonNumber = _seasonNumber,
                        InfocardImageURL = imageLink
                    });
                }
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
