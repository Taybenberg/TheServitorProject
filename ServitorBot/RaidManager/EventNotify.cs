using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Event_Notify(RaidContainer container)
        {
            IMessageChannel channel = _client.GetChannel(_raidChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            builder.ThumbnailUrl = container.RaidIcon;

            builder.Description = $"За 10 хвилин почнеться рейд {container.RaidName}!";

            var users = container.ReservationsOrdered.Take(6);

            builder.Fields = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder
                {
                    Name = "Бойова група",
                    Value = string.Join("\n", users.Select(x => $"<@{x.ID}>"))
                }
            };

            var builded = builder.Build();

            foreach (var user in users)
            {
                try
                {
                    var u = await _client.Rest.GetUserAsync(user.ID);

                    await u.SendMessageAsync(embed: builded);
                }
                catch { }
            }
        }
    }
}
