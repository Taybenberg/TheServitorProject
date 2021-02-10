using System;
using System.Web;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || (message.Channel.Name.ToLower() != "destiny_bot" && message.Channel.Name.ToLower() != "servitor_beta"))
                return;

            var command = message.Content.ToLower();

            if (command == "біп")
            {
                await message.Channel.SendMessageAsync("біп…");
            }
            else if (command == "допомога")
            {
                await message.Channel.SendMessageAsync($"Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого клану." +
                    $"\n\n**Зараз я вмію виконувати наступні функції:**" +
                    $"\n***біп*** - *запит на перевірку моєї працездатності*" +
                    $"\n***мої активності*** - *кількість активностей ґардіана у цьому році*" +
                    $"\n***мої партнери*** - *список партнерів ґардіана*" +
                    $"\n***реєстрація*** - *прив'язати акаунт Destiny 2 до профілю в Discord*" +
                    $"\n***відступники*** - *виявити потенційно небезпечні активності*");
            }
            else if (command == "мої активності")
            {
                if (_database.IsDiscordUserRegisteredAsync(message.Author.Id))
                {
                    var user = await _database.GetUserByDiscordIdAsync(message.Author.Id);

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
            else if (command == "мої партнери")
            {
                if (_database.IsDiscordUserRegisteredAsync(message.Author.Id))
                {
                    var user = await _database.GetUserByDiscordIdAsync(message.Author.Id);

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
                                var user2 = relation.GetUser2Async(_database);

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
            else if (command == "реєстрація")
            {
                if (_database.IsDiscordUserRegisteredAsync(message.Author.Id))
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
            else if (command == "відступники")
            {
                var notification = await message.Channel.SendMessageAsync("Шукаю інформацію, на це мені знадобиться трохи часу…");

                ConcurrentDictionary<DateTime, string> activityDetails = new();

                var activities = await _database.GetSuspiciousActivitiesAsync(DateTime.Now.AddDays(-7));

                Parallel.ForEach(activities, (activity) =>
                {
                    string details = $" {activity.ActivityType}";

                    var act = activity.GetActivityAdditionalDetailsAsync(_apiClient).Result;

                    if (activity.ActivityType == BungieNetApi.ActivityType.ScoredNightfall)
                        details += $" {act.ActivityUserStats.First().TeamScore}";

                    foreach (var u in act.ActivityUserStats)
                    {
                        var clans = _apiClient.GetUserClansAsync(u.MembershipType, u.MembershipId).Result;

                        details += $"\n{u.DisplayName} {HttpUtility.HtmlDecode(string.Join(",", clans))}";
                    }

                    activityDetails.TryAdd(activity.Period, details);
                });

                await message.Channel.SendMessageAsync($"За останні 7 днів знайдено активностей: {activityDetails.Count}. Увага, чутливим не читати!\n" +
                    $"||{string.Join("\n\n", activityDetails.OrderByDescending(x => x.Key))}||");

                await notification.DeleteAsync();
            }
            else if (command.Contains("зареєструвати"))
            {
                var nickname = command.Replace("зареєструватися ", "").Replace("зареєструватись ", "").Replace("зареєструвати ", "");

                if (!_database.IsDiscordUserRegisteredAsync(message.Author.Id) && nickname.Length > 0)
                {
                    var users = (await _database.GetUsersByUserNameAsync(nickname)).Where(x => x.DiscordUserID is null);

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

                        _database.Users.Update(user);
                        await _database.SaveChangesAsync();
                        
                        await message.Channel.SendMessageAsync($"Зареєстровано {message.Author.Mention} як гравця {user.UserName}");
                    }
                }
                else
                {
                    await message.Channel.SendMessageAsync("Ґардіане, ви вже зареєстровані…");
                }
            }
            else if (message.MentionedUsers.Where(x => x.Username == _client.CurrentUser.Username).Count() > 0 && command.Contains("привітайся"))
            {
                await message.Channel.SendMessageAsync($"Ах, точно, перепрошую за незручності. Культурні цінності моїх творців відрізняються від ваших. Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого клану.");
            }
        }
    }
}
