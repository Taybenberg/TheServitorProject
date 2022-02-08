﻿using DestinyInfocardsDatabase;
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

            return new EververseInfocard
            {
                WeekNumber = weekNumber,
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime()
            };
        }
    }
}
