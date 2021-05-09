using BungieNetApi;
using Database;
using Discord;
using Discord.WebSocket;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitBumpAsync(SocketMessage message)
        {
            var embed = message.Embeds.FirstOrDefault();

            if (embed is not null)
            {
                if (embed.Description?.Contains("Server bumped by") ?? false)
                {
                    _logger.LogInformation($"{DateTime.Now} Server bumped");

                    var mention = Regex.Match(embed.Description, "(?<=\\<@)\\D?(\\d+)(?=\\>)").Groups[1].Value;

                    _bumper.AddUser(mention);

                    var builder = new EmbedBuilder();

                    builder.Color = GetColor(MessagesEnum.Bumped);

                    builder.Description = $":alarm_clock: :ok_hand:\n:fast_forward: {_bumper.NextBump.ToString("HH:mm:ss")}";

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
            }
        }

        private async Task LeaderboardAsync(SocketMessage message, string mode)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

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
                var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

                builder.Title += $" | { pair.Value[0]}";

                var leaderboard = await apiClient.GetClanLeaderboardAsync(pair.Key, Localization.StatNames.Keys.ToArray());

                if (leaderboard.Any())
                {
                    builder.Fields = new();

                    builder.Color = GetColor(MessagesEnum.Leaderboard);

                    var users = await database.GetUsersAsync();

                    foreach (var entry in leaderboard)
                    {
                        if (entry.users.Count() == 0)
                            continue;

                        string usrs = string.Empty;

                        bool userFound = false;

                        foreach (var user in entry.users.Take(3))
                        {
                            var u = users.FirstOrDefault(x => x.UserID == user.userId);

                            if (u is null)
                                continue;

                            if (u.UserID == currUser.UserID)
                            {
                                usrs += $"***{user.rank}, {u.UserName}, {Localization.ClassStrNames[user.className]}, {user.value}***\n";

                                userFound = true;
                            }
                            else
                                usrs += $"{user.rank}, {u.UserName}, {Localization.ClassStrNames[user.className]}, {user.value}\n";
                        }

                        if (!userFound)
                        {
                            var u = entry.users.FirstOrDefault(x => x.userId == currUser.UserID);

                            if (!u.Equals(default))
                                usrs += $"***{u.rank}, {currUser.UserName}, {Localization.ClassStrNames[u.className]}, {u.value}***\n";
                        }

                        builder.Fields.Add(new EmbedFieldBuilder
                        {
                            Name = Localization.StatNames[entry.stat],
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

            builder.Title = $"БЕТА | Статистика клану {_serverName}";

            builder.Footer = GetFooter();

            if (pair.Value is not null)
            {
                using var scope = _scopeFactory.CreateScope();

                var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

                builder.Title += $" | { pair.Value[0]}";

                var clanStats = await apiClient.GetClanStatsAsync(pair.Key);

                if (clanStats.Count() > 0)
                {
                    builder.Color = GetColor(MessagesEnum.ClanStats);

                    builder.Fields = new();

                    foreach (var clanStat in clanStats)
                    {
                        builder.Fields.Add(new EmbedFieldBuilder
                        {
                            Name = Localization.StatNames[clanStat.stat],
                            Value = clanStat.value,
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
