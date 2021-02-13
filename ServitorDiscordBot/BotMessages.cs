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
#if DEBUG
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || message.Channel.Name.ToLower() != "servitor_beta")
                return;
#else
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || message.Channel.Name.ToLower() != "destiny_bot")
                return;
#endif

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
                    $"\n***відступники*** - *виявити потенційно небезпечні активності окрім нальотів*" +
                    $"\n***100K*** - *виявити потенційно небезпечні нальоти з сумою очок більше 100К*");
            }
            else if (command == "мої активності")
            {
                await GetUserActivitiesAsync(message);
            }
            else if (command == "мої партнери")
            {
                await GetUserPartnersAsync(message);
            }
            else if (command == "реєстрація")
            {
                await RegisterAsync(message);
            }
            else if (command is "100k" or "100к")
            {
                await FindSuspiciousAsync(message, true);
            }    
            else if (command == "відступники")
            {
                await FindSuspiciousAsync(message, false);
            }
            else if (command.Contains("зареєструвати"))
            {
                var nickname = command.Replace("зареєструватися ", "").Replace("зареєструватись ", "").Replace("зареєструвати ", "");

                await RegisterUserAsync(message, nickname);
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
