using DestinyInfocardsDatabase;
using DestinyInfocardsService.Infocards;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<XurInfocard> GetXurInfocardAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var infocardsDB = scope.ServiceProvider.GetRequiredService<IInfocardsDB>();

            (var resetBegin, var resetEnd) = GetWeeklyResetInterval();

            //var xurItems = await infocardsDB.GetXurInventoryAsync(resetBegin, resetEnd);

            var dataParser = new DataParser(_scopeFactory);

            var xurItems = await dataParser.ParseXurInventoryAsync(resetBegin, resetEnd);

            using var image = await ImageGenerator.GetXurImageAsync(xurItems);

            var imageLink = await UploadImageAsync(image);

            var xurLocation = await dataParser.ParseXurLocationAsync();

            /*
            await infocardsDB.AddXurInventoryAsync(xurItems with
            {
                XurLocation = xurLocation,
                InfocardImageURL = imageLink
            });
            */

            return new XurInfocard
            {
                XurLocation = xurLocation,
                InfocardImageURL = imageLink
            };
        }
    }
}
