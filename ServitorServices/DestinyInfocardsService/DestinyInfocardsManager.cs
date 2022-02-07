using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;

namespace DestinyInfocardsService
{
    public partial class DestinyInfocardsManager : IDestinyInfocards
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public DestinyInfocardsManager(ILogger<DestinyInfocardsManager> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            (_logger, _scopeFactory) = (logger, scopeFactory);
        }

        private (DateTime DailyRestBegin, DateTime DailyResetEnd) GetDailyResetInterval()
        {
            var currDate = DateTime.UtcNow;
            var resetTime = currDate.Date.AddHours(17);

            return currDate.Hour < 17 ?
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