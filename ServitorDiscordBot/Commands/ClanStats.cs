using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetClanStatsAsync(IMessage message, string mode)
        {
            var builder = GetBuilder(MessagesEnum.ClanStats, message);

            var stats = await getStatsFactory().GetClanStatsAsync(mode);

            if (!stats.IsSuccessful)
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Ви вказали хибний тип активності.\nЩоб переглянути список типів активностей, введіть команду ***режими***.";
            }
            else
            {
                builder.Title += $" | { stats.Mode }";

                builder.ThumbnailUrl = Emote.Parse(stats.Emoji).Url;

                if (stats.Stats.Count() > 0)
                {
                    builder.Fields = stats.Stats.Select(x =>
                    new EmbedFieldBuilder
                    {
                        Name = x.Name,
                        Value = x.Value,
                        IsInline = false
                    }).ToList();
                }
                else
                {
                    builder.Color = GetColor(MessagesEnum.Error);

                    builder.Description = "Сталася помилка при обробці вашого запиту сервером Bungie.net. Спробуйте пізніше.";
                }
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
