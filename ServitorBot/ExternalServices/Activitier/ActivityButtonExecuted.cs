using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivityButtonExecutedAsync(SocketMessageComponent component)
        {
            var user = await _client.Rest.GetGuildUserAsync((component.Channel as IGuildChannel).GuildId, component.User.Id);

            if (!user.RoleIds.Any(id => id == _destinyRoleID))
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
