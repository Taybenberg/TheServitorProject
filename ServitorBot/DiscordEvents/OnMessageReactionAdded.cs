using CommonData.DiscordEmoji;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnMessageReactionAddedAsync(Cacheable<IUserMessage, ulong> message, IMessageChannel channel, SocketReaction reaction)
        {

            if (channel.Id != _raidChannelId || _client.GetUser(reaction.UserId).IsBot)
                return;

            var emote = reaction.Emote.ToString();

            if (emote == EmojiContainer.Check)
            {
                var raid = _raidManager[message.Id];

                if (raid is not null)
                {
                    var user = await _client.Rest.GetGuildUserAsync((channel as IGuildChannel).GuildId, reaction.UserId);

                    if (user.RoleIds.Any(id => id == _destinyRoleId))
                        raid.AddUser(reaction.UserId);

                    await RemoveReaction(reaction.Emote, channel, message.Id);
                }
            }
            else if (emote == EmojiContainer.UnCheck)
            {
                var raid = _raidManager[message.Id];

                if (raid is not null)
                {
                    raid.RemoveUser(reaction.UserId);

                    await RemoveReaction(reaction.Emote, channel, message.Id);
                }
            }
        }

        private async Task RemoveReaction(IEmote emote, IMessageChannel channel, ulong messageID)
        {
            try
            {
                var msg = await channel.GetMessageAsync(messageID);

                if (msg is not null)
                {
                    await msg.RemoveAllReactionsForEmoteAsync(emote);

                    await msg.AddReactionAsync(emote);
                }
            }
            catch { }
        }
    }
}
