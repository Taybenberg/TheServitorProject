using Discord;
using DataProcessor;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task LeaderboardAsync(IMessage message, string mode)
        {
            var database = getDatabase();

            var currUser = await database.GetUserActivitiesAsync(message.Author.Id);

            if (currUser is null)
            {
                await UserIsNotRegisteredAsync(message.Channel);

                return;
            }

            var pair = Localization.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == mode));

            var builder = new EmbedBuilder();

            builder.Title = $"БЕТА | Дошка лідерів";

            builder.Footer = GetFooter();

            if (pair.Value is not null)
            {
                var apiClient = getApiClient();

                builder.Title += $" | { pair.Value[0]}";

                var leaderboard = await apiClient.Clan.GetClanLeaderboardAsync(pair.Key, Localization.StatNames.Keys.ToArray());

                if (leaderboard.Any())
                {
                    builder.Fields = new();

                    builder.Color = GetColor(MessagesEnum.Leaderboard);

                    var users = await database.GetUsersAsync();

                    foreach (var entry in leaderboard)
                    {
                        if (entry.Leaders.Count() == 0)
                            continue;

                        string usrs = string.Empty;

                        bool userFound = false;

                        foreach (var user in entry.Leaders.Take(3))
                        {
                            var u = users.FirstOrDefault(x => x.UserID == user.UserID);

                            if (u is null)
                                continue;

                            if (u.UserID == currUser.UserID)
                            {
                                usrs += $"***{user.Rank}, {u.UserName}, {Localization.ClassNames[user.Class]}, {user.Value}***\n";

                                userFound = true;
                            }
                            else
                                usrs += $"{user.Rank}, {u.UserName}, {Localization.ClassNames[user.Class]}, {user.Value}\n";
                        }

                        if (!userFound)
                        {
                            var u = entry.Leaders.FirstOrDefault(x => x.UserID == currUser.UserID);

                            if (!u.Equals(default))
                                usrs += $"***{u.Rank}, {currUser.UserName}, {Localization.ClassNames[u.Class]}, {u.Value}***\n";
                        }

                        builder.Fields.Add(new EmbedFieldBuilder
                        {
                            Name = Localization.StatNames[entry.Stat],
                            Value = usrs,
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

        private async Task ClanStatsAsync(IMessageChannel channel, string mode)
        {
            var pair = Localization.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == mode));

            var builder = new EmbedBuilder();

            builder.Title = $"БЕТА | Статистика клану {(channel as IGuildChannel).Guild.Name}";

            builder.Footer = GetFooter();

            if (pair.Value is not null)
            {
                var apiClient = getApiClient();

                builder.Title += $" | { pair.Value[0]}";

                var clanStats = await apiClient.Clan.GetClanStatsAsync(pair.Key);

                if (clanStats.Count() > 0)
                {
                    builder.Color = GetColor(MessagesEnum.ClanStats);

                    builder.Fields = new();

                    foreach (var clanStat in clanStats)
                    {
                        builder.Fields.Add(new EmbedFieldBuilder
                        {
                            Name = Localization.StatNames[clanStat.Stat],
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

            await channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
