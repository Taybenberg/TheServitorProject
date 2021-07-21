using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetMyGrandmastersAsync(IMessage message)
        {
            var gms = await getWrapperFactory().GetMyGrandmastersAsync(message.Author.Id, _seasonStart);

            if (!gms.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyGrandmasters, message);

            builder.ThumbnailUrl = message.Author.GetAvatarUrl();

            builder.Fields = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder
                {
                    Name = $"Сезон {_seasonName}",
                    Value = gms.Seasonal.Any() ? string.Join("\n", gms.Seasonal) : "Немає",
                    IsInline = false
                },
                new EmbedFieldBuilder
                {
                    Name = $"Весь час",
                    Value = gms.AllTime.Any() ? string.Join("\n", gms.AllTime) : "Немає",
                    IsInline = false
                }
            };

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
