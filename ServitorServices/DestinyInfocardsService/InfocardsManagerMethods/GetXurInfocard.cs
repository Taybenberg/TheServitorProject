﻿using DestinyInfocardsDatabase;
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

            var weekNumber = GetWeekNumber();

            (var resetBegin, var resetEnd) = GetWeeklyResetInterval(weekNumber);

            //var xurItems = await infocardsDB.GetXurInventoryAsync(resetBegin, resetEnd);

            var dataParser = new DataParser(_scopeFactory);

            var xurItems = await dataParser.ParseXurInventoryAsync();

            using var image = await ImageGenerator.GetXurImageAsync(xurItems);

            var imageLink = await UploadImageAsync(image);

            var xurLocation = await dataParser.ParseXurLocationAsync();

            if (!xurItems.XurItems.Any())
            {
                //await infocardsDB.AddXurInventoryAsync(xurItems with
                //{
                //    WeeklyResetBegin = resetBegin,
                //    WeeklyResetEnd = resetEnd,
                //    SeasonNumber = _seasonNumber,
                //    XurLocation = xurLocation,
                //    InfocardImageURL = imageLink
                //});
            }

            return new XurInfocard
            {
                WeekNumber = weekNumber,
                XurLocation = xurLocation,
                InfocardImageURL = imageLink
            };
        }
    }
}
