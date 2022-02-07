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

            string imageLink = null;//infocardsDB.Get;

            if (imageLink is null)
            {
                var sectors = await DataParser.ParseLostSectorsAsync(resetBegin, resetEnd);

                using var image = await ImageGenerator.GetLostSectorsImageAsync(sectors);

                imageLink = await UploadImageAsync(image);

                var s = sectors with
                {
                    InfocardImageURL = imageLink
                };

                //infocardsDB.Set;
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
