using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivityButtonExecutedAsync(SocketMessageComponent component)
        {
            var user = await _client.Rest.GetGuildUserAsync((component.Channel as IGuildChannel).GuildId, component.User.Id);

            if (!user.RoleIds.Any(id => id == _destinyRoleId))
                return;

            switch (component.Data.CustomId)
            {
                case "ActivitierSubscribe":
                    {
                        await component.DeferAsync();

                        await _activityManager.UsersSubscribeAsync(component.Message.Id, component.User.Id, new ulong[] { component.User.Id });
                    }
                    break;

                case "ActivitierUnsubscribe":
                    {
                        await component.DeferAsync();

                        await _activityManager.UsersUnSubscribeAsync(component.Message.Id, component.User.Id, new ulong[] { component.User.Id });
                    }
                    break;

                default: break;
            }
        }
    }
}
