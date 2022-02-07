using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager : IDestinyInfocards
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly int _seasonNumber;
        private readonly DateTime _seasonStart;

        public DestinyInfocardsManager(ILogger<DestinyInfocardsManager> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);

            _seasonNumber = configuration.GetSection("Destiny2:SeasonNumber").Get<int>();
            _seasonStart = configuration.GetSection("Destiny2:SeasonStart").Get<DateTime>();
        }

        private int GetWeekNumber() =>
            (int)(DateTime.UtcNow - _seasonStart).TotalDays / 7 + 1;

        private (DateTime DailyResetBegin, DateTime DailyResetEnd) GetDailyResetInterval()
        {
            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17);

            return currDate < resetTime ?
                (resetTime.AddDays(-1), resetTime) :
                (resetTime, resetTime.AddDays(1));
        }

        private (DateTime WeeklyResetBegin, DateTime WeeklyResetEnd) GetWeeklyResetInterval()
        {
            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17);

            var weeklyReset = resetTime.AddDays(2 - (int)currDate.DayOfWeek);

            return currDate < weeklyReset ?
                (weeklyReset.AddDays(-7), weeklyReset) :
                (weeklyReset, weeklyReset.AddDays(7));
        }

        private async Task<string> UploadImageAsync(Image image)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<ApiClient>();

            using var ms = new MemoryStream();

            await image.SaveAsPngAsync(ms);

            ms.Position = 0;

            var imageEndpoint = new ImageEndpoint(apiClient, new HttpClient());

            var imageUpload = await imageEndpoint.UploadImageAsync(ms);

            return imageUpload.Link;
        }
    }
}