using System;
using System.Web;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using BungieNetApi;
using Database;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task FindSuspiciousAsync(SocketMessage message, bool nigthfalls)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            var notification = await message.Channel.SendMessageAsync("Шукаю інформацію, на це мені знадобиться трохи часу…");

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

            string mes = $"Виявлено активностей: {activityDetails.Count}.";

            string list = " Увага, чутливим не читати! Останні активності:\n||";

            foreach (var act in activityDetails.OrderByDescending(x => x.Key))
            {
                if ((mes + list + act + "\n\n||").Length < 2000)
                    list += act + "\n\n";
            }

            list += "||";

            await message.Channel.SendMessageAsync(mes + (activityDetails.Count > 0 ? list : ""));

            await notification.DeleteAsync();
        }

        private async Task RegisterAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (database.IsDiscordUserRegistered(message.Author.Id))
            {
                await message.Channel.SendMessageAsync("Ґардіане, ви вже зареєстровані…");
            }
            else
            {
                await message.Channel.SendMessageAsync($"Добре, давайте ж запишемо вас. Важливо, аби ви були учасником клану **хоча б один день**. " +
                    $"Якщо це так, можемо продовжити.\nВведіть команду ***зареєструватися [ваш ігровий нікнейм]***\n" +
                    $"Приклад команди: ***зареєструватися {_client.CurrentUser.Username}***\n" +
                    $"Регістр літер не має значення, можете написати лише фрагмент нікнейму, але він має містити достатню кількіть символів для точної ідентифікації профілю.");
            }
        }

        private async Task RegisterUserAsync(SocketMessage message, string nickname)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (!database.IsDiscordUserRegistered(message.Author.Id) && nickname.Length > 0)
            {
                var users = (await database.GetUsersByUserNameAsync(nickname)).Where(x => x.DiscordUserID is null);

                if (users.Count() < 1)
                {
                    await message.Channel.SendMessageAsync("Не можу знайти гравця. Перевірте запит.");
                }
                else if (users.Count() > 1)
                {
                    await message.Channel.SendMessageAsync($"Уточніть нікнейм, бо за цим шаблоном знайдено кілька гравців: {string.Join(", ", users.Select(x => x.UserName))}");
                }
                else
                {
                    var user = users.First();

                    user.DiscordUserID = message.Author.Id;

                    database.Users.Update(user);
                    await database.SaveChangesAsync();

                    await message.Channel.SendMessageAsync($"Зареєстровано {message.Author.Mention} як гравця {user.UserName}");
                }
            }
            else
            {
                await message.Channel.SendMessageAsync("Ґардіане, ви вже зареєстровані…");
            }
        }

        private async Task GetUserPartnersAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (database.IsDiscordUserRegistered(message.Author.Id))
            {
                var user = await database.GetUserByDiscordIdAsync(message.Author.Id);

                if (user is null)
                {
                    await message.Channel.SendMessageAsync("Сталася помилка. Можливо ви не зареєстровані.");
                }
                else
                {
                    var relations = user.UserRelations.Where(x => x.User2ID is not null && x.Count > 0);

                    if (!relations.Any())
                    {
                        await message.Channel.SendMessageAsync("Я не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали у цьому році.");
                    }
                    else
                    {
                        string list = $"Отже, {message.Author.Mention} грає у активності з наступними ґардіанами:";

                        foreach (var relation in relations.OrderByDescending(x => x.Count))
                        {
                            var user2 = relation.GetUser2(database);

                            if (user2 is not null)
                                list += $"\n{user2.UserName} - {relation.Count}";
                        }

                        await message.Channel.SendMessageAsync(list);
                    }
                }
            }
            else
            {
                await message.Channel.SendMessageAsync("Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***");
            }
        }

        private async Task GetUserActivitiesAsync(SocketMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (database.IsDiscordUserRegistered(message.Author.Id))
            {
                var user = await database.GetUserByDiscordIdAsync(message.Author.Id);

                if (user is null)
                {
                    await message.Channel.SendMessageAsync("Сталася помилка. Можливо ви не зареєстровані.");
                }
                else
                {
                    var relation = user.UserRelations.Where(x => x.User2ID is null).FirstOrDefault();

                    if (relation is null)
                    {
                        await message.Channel.SendMessageAsync("Я не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ж не грали у цьому році.");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync($"Неймовірно! {relation.Count} активностей на рахунку {message.Author.Mention}! Так тримати!");
                    }
                }
            }
            else
            {
                await message.Channel.SendMessageAsync("Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***");
            }
        }
    }
}
