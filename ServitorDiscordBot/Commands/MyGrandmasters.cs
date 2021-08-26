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
            var grandmasters = await getWrapperFactory().GetMyGrandmastersAsync(message.Author.Id, _seasonStart);

            if (!grandmasters.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyGrandmasters, message, userName: grandmasters.UserName);

            builder.ThumbnailUrl = message.Author.GetAvatarUrl();

            builder.Fields = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder
                {
                    Name = $"Сезон {_seasonName}",
                    Value = grandmasters.Seasonal.Any() ? string.Join("\n", grandmasters.Seasonal) : "Немає",
                    IsInline = false
                },
                new EmbedFieldBuilder
                {
                    Name = $"Весь час",
                    Value = grandmasters.AllTime.Any() ? string.Join("\n", grandmasters.AllTime) : "Немає",
                    IsInline = false
                }
            };

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
