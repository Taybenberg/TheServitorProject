using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetLeaderboardAsync(IMessage message, string mode)
        {
            var builder = GetBuilder(MessagesEnum.Leaderboard, message);

            var leaderboard = await getStatsFactory().GetLeaderboardAsync(mode, message.Author.Id);

            if (!leaderboard.IsSuccessful)
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Ви вказали хибний тип активності.\nЩоб переглянути список типів активностей, введіть команду ***режими***.";
            }
            else
            {
                builder.Title += $" | { leaderboard.Mode }";

                builder.ThumbnailUrl = Emote.Parse(leaderboard.Emoji).Url;

                if (!leaderboard.Stats.Any())
                {
                    builder.Color = GetColor(MessagesEnum.Error);

                    builder.Description = "Сталася помилка при обробці вашого запиту сервером Bungie.net. Спробуйте пізніше.";
                }
                else
                {
                    if (!leaderboard.UserRegistered)
                        builder.Description = "Зареєструйтеся (команда ***реєстрація***), щоб побачити свої позиції у дошці лідерів.";

                    builder.Fields = leaderboard.Stats.Select(x =>
                    new EmbedFieldBuilder
                    {
                        IsInline = false,
                        Name = x.Name,
                        Value = string.Join("\n", x.Entries.Select(y => y.IsCurrUser ?
                        $"**{y.Rank}, {y.UserName}, {y.Class}, {y.Value}**" :
                        $"{y.Rank}, {y.UserName}, {y.Class}, {y.Value}"))
                    }).ToList();
                }
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
