﻿using BungieNetApi;
using Database;
using Discord;
using Discord.WebSocket;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetModesAsync(SocketMessage message)
        {
            var builder = new EmbedBuilder();

            builder.Color = Color.DarkBlue;

            builder.Title = $"Режими";

            builder.Fields = new List<EmbedFieldBuilder>();

            builder.Description = string.Join("\n", Localization.StatsActivityNames.Select(x => x.Value).OrderBy(y => y));

            builder.Footer = GetFooter();

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task ClanStatsAsync(SocketMessage message, string mode)
        {
            var pair = Localization.StatsActivityNames.FirstOrDefault(x => mode.ToLower() == x.Value.ToLower());

            var builder = new EmbedBuilder();

            builder.Title = $"БЕТА | Статистика клану {clanName}";

            builder.Footer = GetFooter();

            if (!pair.Equals(default(KeyValuePair<BungieNetApi.ActivityType, string>)))
            {
                using var scope = _scopeFactory.CreateScope();

                var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

                builder.Title += $". { pair.Value}";

                builder.Fields = new List<EmbedFieldBuilder>();

                var clanStats = await apiClient.GetClanStatsAsync(pair.Key);

                if (clanStats.Count() > 0)
                {
                    builder.Color = Color.Blue;

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
                    builder.Color = Color.Red;

                    builder.Description = "Сталася помилка при обробці вашого запиту сервером Bungie.net. Спробуйте пізніше.";
                }
            }
            else
            {
                builder.Color = Color.Red;

                builder.Description = "Сталася помилка при обробці вашого запиту, переконайтеся, що ви правильно вказали тип активності.\nДля цього введіть команду ***режими***.";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task FindSuspiciousAsync(SocketMessage message, bool nigthfalls)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            var builder = new EmbedBuilder();

            builder.Color = Color.DarkerGrey;

            builder.Description = "Шукаю інформацію, на це мені знадобиться трохи часу…";

            var notification = await message.Channel.SendMessageAsync(embed: builder.Build());

            ConcurrentDictionary<DateTime, string> activityDetails = new();

            var activities = nigthfalls ? await database.GetSuspiciousNightfallsOnlyAsync(DateTime.Now.AddDays(-7)) : await database.GetSuspiciousActivitiesWithoutNightfallsAsync(DateTime.Now.AddDays(-7));

            Parallel.ForEach(activities, (activity) =>
            {
                string details = $" {activity.ActivityType}";

                var act = activity.GetActivityAdditionalDetailsAsync(apiClient).Result;

                if (activity.ActivityType == BungieNetApi.ActivityType.ScoredNightfall)
                {
                    var u = act.ActivityUserStats.FirstOrDefault();

                    if (u is not null)
                        details += $" {u.TeamScore}";
                }

                foreach (var u in act.ActivityUserStats)
                {
                    var clans = apiClient.GetUserClansAsync(u.MembershipType, u.MembershipId).Result;

                    details += $"\n{u.DisplayName} {HttpUtility.HtmlDecode(string.Join(",", clans))}";
                }

                activityDetails.TryAdd(activity.Period, details);
            });

            builder.Title = $"Виявлено активностей: {activityDetails.Count}";

            string list = "Увага, чутливим не читати! Останні активності:\n||";

            foreach (var act in activityDetails.OrderByDescending(x => x.Key))
            {
                if ((list + act + "\n\n||").Length < 2000)
                    list += act + "\n\n";
            }

            list += "||";

            builder.Description = list;

            builder.Footer = GetFooter();

            await message.Channel.SendMessageAsync(embed: builder.Build());

            await notification.DeleteAsync();
        }

        private async Task RegisterMessageAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (database.IsDiscordUserRegistered(message.Author.Id))
                await UserAlreadyRegisteredAsync(message);
            else
            {
                var builder = new EmbedBuilder();

                builder.Color = Color.Teal;

                builder.Title = "Реєстрація";
                builder.Description = $"Добре, давайте ж запишемо вас. Важливо, аби ви були учасником клану **хоча б один день**. " +
                    $"Якщо це так, можемо продовжити.\nВведіть команду ***зареєструватися [ваш нікнейм у Steam]*** (або іншій платформі, з якої ви вступили до клану)\n" +
                    $"Приклад команди: ***зареєструватися {message.Author.Username}***\n" +
                    $"Регістр літер не має значення, можете написати лише фрагмент нікнейму, але він має містити достатню кількіть символів для точної ідентифікації профілю.";

                builder.Footer = GetFooter();

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }

        private async Task TryRegisterUserAsync(SocketMessage message, string nickname)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (!database.IsDiscordUserRegistered(message.Author.Id) && nickname.Length > 0)
            {
                var users = (await database.GetUsersByUserNameAsync(nickname)).Where(x => x.DiscordUserID is null);

                var builder = new EmbedBuilder();

                builder.Title = "Реєстрація";

                builder.Footer = GetFooter();

                if (users.Count() < 1)
                {
                    builder.Color = Color.DarkMagenta;

                    builder.Description = "Не можу знайти гравця. Перевірте запит.";
                }
                else if (users.Count() > 1)
                {
                    builder.Color = Color.DarkMagenta;

                    builder.Description = $"Уточніть нікнейм, бо за цим шаблоном знайдено кілька гравців: {string.Join(", ", users.Select(x => x.UserName))}";
                }
                else
                {
                    var user = users.First();

                    await database.RegisterUserAsync(user.UserID, message.Author.Id);

                    builder.Color = Color.DarkGreen;

                    builder.Description = $"Зареєстровано {message.Author.Mention} як гравця {user.UserName}";
                }

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
            else
                await UserAlreadyRegisteredAsync(message);
        }

        private async Task GetUserPartnersAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (!database.IsDiscordUserRegistered(message.Author.Id))
                await UserIsNotRegisteredAsync(message);
            else
            {
                var partners = await database.GetUserPartnersAsync(message.Author.Id);

                var builder = new EmbedBuilder();

                builder.Title = $"Партнери {message.Author.Username}";

                builder.Footer = GetFooter();

                if (!partners.Any())
                {
                    builder.Color = Color.Red;

                    builder.Description = "Я не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали у цьому році.";
                }
                else
                {
                    builder.Color = Color.Green;

                    builder.Description = string.Empty;

                    foreach (var p in partners)
                        builder.Description += $"{p.UserName} - {p.Count}\n";
                }

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }

        private async Task GetUserActivitiesAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            var user = await database.GetUserActivitiesAsync(message.Author.Id);

            if (user is null)
                await UserIsNotRegisteredAsync(message);
            else
            {
                var builder = new EmbedBuilder();

                builder.Color = Color.Gold;

                builder.Title = $"Активності {message.Author.Username}";

                var acts = user.Characters.SelectMany(c => c.ActivityUserStats);

                builder.Description = $"Неймовірно! **{acts.Count()}** активностей на рахунку {message.Author.Mention}! Так тримати!\n\n***По класах:***";

                foreach (var c in user.Characters.OrderByDescending(x => x.ActivityUserStats.Count))
                    builder.Description += $"\n{Localization.ClassNames[c.Class]} - {c.ActivityUserStats.Count}";

                builder.Description += "\n\n***По типу активності:***";

                List<(BungieNetApi.ActivityType ActivityType, int Count)> counter = new();

                foreach (var type in acts.Select(x => x.Activity.ActivityType).Distinct())
                    counter.Add((type, acts.Count(x => x.Activity.ActivityType == type)));

                foreach (var count in counter.OrderByDescending(x => x.Count))
                    builder.Description += $"\n{count.ActivityType} - {count.Count}";

                builder.Footer = GetFooter();

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }

        private async Task GetClanActivitiesAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            var builder = new EmbedBuilder();

            builder.Color = Color.Magenta;

            builder.Title = $"Активності клану {clanName}";

            var acts = await database.Activities.ToListAsync();

            builder.Description = $"Нічого собі! **{acts.Count}** активностей на рахунку клану!\n\n***По типу активності:***";

            List<(BungieNetApi.ActivityType ActivityType, int Count)> counter = new();

            foreach (var type in acts.Select(x => x.ActivityType).Distinct())
                counter.Add((type, acts.Count(x => x.ActivityType == type)));

            foreach (var count in counter.OrderByDescending(x => x.Count))
                builder.Description += $"\n{count.ActivityType} - {count.Count}";

            builder.Footer = GetFooter();

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }


        private async Task UserIsNotRegisteredAsync(SocketMessage message)
        {
            var builder = new EmbedBuilder();

            builder.Color = Color.Orange;

            builder.Title = "Реєстрація";
            builder.Description = "Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***";

            builder.Footer = GetFooter();

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task UserAlreadyRegisteredAsync(SocketMessage message)
        {
            var builder = new EmbedBuilder();

            builder.Color = Color.Orange;

            builder.Title = "Реєстрація";
            builder.Description = "Ґардіане, ви вже зареєстровані…";

            builder.Footer = GetFooter();

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
