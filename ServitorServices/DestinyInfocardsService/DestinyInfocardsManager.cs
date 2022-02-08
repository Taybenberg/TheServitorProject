using Imgur.API.Authentication;
using Imgur.API.Endpoints;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System.Globalization;

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
            _seasonStart = DateTime.Parse(configuration["Destiny2:SeasonStart"], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal);
        }

        private int GetWeekNumber() =>
            (int)(DateTime.UtcNow - _seasonStart).TotalDays / 7 + 1;

        private (DateTime WeeklyResetBegin, DateTime WeeklyResetEnd) GetWeeklyResetInterval(int weekNumber) =>
            (_seasonStart.AddDays((weekNumber - 1) * 7), _seasonStart.AddDays(weekNumber * 7));

        private (DateTime DailyResetBegin, DateTime DailyResetEnd) GetDailyResetInterval()
        {
            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17);

            return currDate < resetTime ?
                (resetTime.AddDays(-1), resetTime) :
                (resetTime, resetTime.AddDays(1));
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