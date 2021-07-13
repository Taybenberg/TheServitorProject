using DataProcessor.Localization;
using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetClanStatsAsync(IMessage message, string mode)
        {
            var pair = TranslationDictionaries.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == mode));

            var builder = GetBuilder(MessagesEnum.ClanStats, message);

            if (pair.Value is not null)
            {
                var apiClient = getApiClient();

                builder.Title += $" | { pair.Value[0]}";

                var clanStats = await apiClient.Clan.GetClanStatsAsync(pair.Key);

                if (clanStats.Count() > 0)
                {
                    builder.Fields = new();

                    foreach (var clanStat in clanStats)
                    {
                        builder.Fields.Add(new EmbedFieldBuilder
                        {
                            Name = TranslationDictionaries.StatNames[clanStat.Stat],
                            Value = clanStat.Value,
                            IsInline = false
                        });
                    }
                }
                else
                {
                    builder.Color = GetColor(MessagesEnum.Error);

                    builder.Description = "Сталася помилка при обробці вашого запиту сервером Bungie.net. Спробуйте пізніше.";
                }
            }
            else
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Сталася помилка при обробці вашого запиту, переконайтеся, що ви правильно вказали тип активності.\nДля цього введіть команду ***режими***.";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
