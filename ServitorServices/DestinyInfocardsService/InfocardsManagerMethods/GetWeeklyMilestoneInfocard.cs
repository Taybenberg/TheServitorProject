using DestinyInfocardsDatabase;
using DestinyInfocardsService.Infocards;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager
    {
        public async Task<WeeklyMilestoneInfocard> GetWeeklyMilestoneInfocardAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var infocardsDB = scope.ServiceProvider.GetRequiredService<IInfocardsDB>();

            var weekNumber = GetWeekNumber();

            (var resetBegin, var resetEnd) = GetWeeklyResetInterval(weekNumber);

            return new WeeklyMilestoneInfocard
            {
                WeekNumber = weekNumber,
                ResetBegin = resetBegin.ToLocalTime(),
                ResetEnd = resetEnd.ToLocalTime()
            };
        }
    }
}
