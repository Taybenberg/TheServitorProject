using BungieSharper.Entities;
using ClanActivitiesDatabase;
using ClanActivitiesService.Containers;
using F23.StringSimilarity;
using Microsoft.Extensions.DependencyInjection;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task<RegisterUserContainer> TryRegisterUserAsync(ulong userID, string userName)
        {
            using var scope = _scopeFactory.CreateScope();

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            if (activitiesDB.IsDiscordUserRegistered(userID))
                return null;

            var jw = new JaroWinkler();

            var users = await activitiesDB.GetUsersAsync();

            var mostSimilar = users
                .Select(x => (jw.Similarity(userName.ToLower(), x.UserName.ToLower()), x))
                .MaxBy(x => x.Item1);

            if (mostSimilar.x.DiscordUserID is null && mostSimilar.Item1 >= 0.9)
            {
                if (await activitiesDB.RegisterUserAsync(mostSimilar.x.UserID, userID))
                    return new RegisterUserContainer
                    {
                        IsSuccessful = true,
                        UserName = mostSimilar.x.UserName,
                        Platform = ((BungieMembershipType)mostSimilar.x.MembershipType).ToString().Replace("Tiger", string.Empty)
                    };
            }

            return new RegisterUserContainer
            {
                IsSuccessful = false
            };
        }
    }
}
