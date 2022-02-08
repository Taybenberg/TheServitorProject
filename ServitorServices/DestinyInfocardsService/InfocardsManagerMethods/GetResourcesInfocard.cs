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

            var resources = await infocardsDB.GetVendorsInventoryAsync(resetBegin, resetEnd);
            var imageLink = resources?.InfocardImageURL;

            if (imageLink is null)
            {
                var dataParser = new DataParser(_scopeFactory);

                resources = await dataParser.ParseResourcesAsync();

                using var image = await ImageGenerator.GetResourcesImageAsync(resources);

                imageLink = await UploadImageAsync(image);

                if (resources.ResourceItems.Any())
                {
                    await infocardsDB.AddVendorsInventoryAsync(resources with
                    {
                        DailyResetBegin = resetBegin,
                        DailyResetEnd = resetEnd,
                        SeasonNumber = _seasonNumber,
                        InfocardImageURL = imageLink
                    });
                }
            }

            return new ResourcesInfocard
            {
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime(),
                InfocardImageURL = imageLink
            };
        }
    }
}
