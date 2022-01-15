using Discord.WebSocket;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivityButtonExecutedAsync(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "ActivitierSubscribe":
                    {
                        await _activityManager.UsersSubscribedAsync(component.Message.Id, new ulong[] { component.User.Id });

                        await component.DeferAsync();
                    }
                    break;

                case "ActivitierUnsubscribe":
                    {
                        await _activityManager.UsersUnSubscribedAsync(component.Message.Id, new ulong[] { component.User.Id });

                        await component.DeferAsync();
                    }
                    break;

                default: break;
            }
        }
    }
}
