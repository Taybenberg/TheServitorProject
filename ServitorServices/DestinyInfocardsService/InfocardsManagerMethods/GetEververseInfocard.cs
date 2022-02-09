using DestinyInfocardsDatabase;
using DestinyInfocardsService.Infocards;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<EververseInfocard> GetEververseInfocardAsync(int? week = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var infocardsDB = scope.ServiceProvider.GetRequiredService<IInfocardsDB>();

            int weekNumber = week ?? GetWeekNumber();

            (var resetBegin, var resetEnd) = GetWeeklyResetInterval(weekNumber);

            var eververseItems = await infocardsDB.GetEververseInventoryAsync(resetBegin, resetEnd);
            var imageLink = eververseItems?.InfocardImageURL;

            if (imageLink is null)
            {
                var dataParser = new DataParser(_scopeFactory);

                eververseItems = await dataParser.ParseEververseAsync(weekNumber);

                using var image = await ImageGenerator.GetEververseImageAsync(eververseItems);

                imageLink = await UploadImageAsync(image);

                if (eververseItems.EververseItems.Any())
                {
                    await infocardsDB.AddEververseInventoryAsync(eververseItems with
                    {
                        WeeklyResetBegin = resetBegin,
                        WeeklyResetEnd = resetEnd,
                        SeasonNumber = _seasonNumber,
                        InfocardImageURL = imageLink
                    });
                }
            }

            return new EververseInfocard
            {
                WeekNumber = weekNumber,
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime(),
                InfocardImageURL = imageLink
            };
        }
    }
}
