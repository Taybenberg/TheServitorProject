using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
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

            if (message.Content == "біп")
            {
                await message.Channel.SendMessageAsync("біп…");
            }
            else if (message.Content == "допомога")
            {
                await message.Channel.SendMessageAsync($"Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого клану." +
                    $"\n**Зараз я вмію виконувати наступні функції:**" +
                    $"\n***біп*** - *запит на перевірку моєї працездатності*" +
                    $"\n***мої активності*** - *кількість активностей ґардіана у цьому році*" +
                    $"\n***мої партнери*** - *список партнерів ґардіана*" +
                    $"\n***реєстрація*** - *прив'язати акаунт Destiny 2 до профілю в Discord*" +
                    $"\n***відступники*** - *виявити потенційно небезпечні активності*");
            }
            else if (message.Content == "мої активності")
            {

            }
            else if (message.Content == "мої партнери")
            {

            }
            else if (message.Content == "реєстрація")
            {
            }
            else if (message.Content == "відступники")
            {
                var notification = await message.Channel.SendMessageAsync("Шукаю інформацію, на це мені знадобиться трохи часу…");

                ConcurrentDictionary<DateTime, string> activityDetails = new();

                var activities = await _database.GetSuspiciousActivitiesAsync(DateTime.Now.AddDays(-7));

                Parallel.ForEach(activities, (activity) =>
                {
                    string details = $" {activity.ActivityType}";

                    foreach (var u in activity.GetActivityAdditionalDetailsAsync(_apiClient).Result.ActivityUserStats)
                    {
                        var clans = _apiClient.GetUserClansAsync(u.MembershipType, u.MembershipId).Result;

                        details += $"\n{u.DisplayName} {string.Join(",", clans)}";
                    }

                    activityDetails.TryAdd(activity.Period, details);
                });

                await message.Channel.SendMessageAsync($"За останні 7 днів знайдено активностей: {activityDetails.Count}. Увага, чутливим не читати!\n" +
                    $"||{string.Join("\n\n", activityDetails.OrderByDescending(x => x.Key))}||");

                await notification.DeleteAsync();
            }
            else if (message.MentionedUsers.Where(x => x.Username == _client.CurrentUser.Username).Count() > 0 && message.Content.ToLower().Contains("привітайся"))
            {
                await message.Channel.SendMessageAsync($"Ах, точно, перепрошую за незручності. Культурні цінності моїх творців відрізняються від ваших. Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого клану.");
            }
        }
    }
}
