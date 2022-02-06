using Discord;
using Discord.WebSocket;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task ActivityButtonExecutedAsync(SocketMessageComponent component)
        {
            var user = await _client.Rest.GetGuildUserAsync((component.Channel as IGuildChannel).GuildId, component.User.Id);

            if (!user.RoleIds.Any(id => _destinyRoleIDs.Any(x => x == id)))
                return;

            switch (component.Data.CustomId)
            {
                case "ActivitierSubscribe":
                    {
                        await component.DeferAsync();

                        await _activityManager.UserSubscribeOrUnsubscribeAsync(component.Message.Id, component.User.Id);
                    }
                    break;

                default: break;
            }
        }
    }
}
