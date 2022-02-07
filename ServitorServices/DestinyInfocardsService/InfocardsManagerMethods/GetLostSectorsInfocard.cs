using DestinyInfocardsService.Infocards;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<LostSectorsInfocard> GetLostSectorsInfocardAsync()
        {
            (var resetBegin, var resetEnd) = GetDailyResetInterval();

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
