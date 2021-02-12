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
                if (_database.IsDiscordUserRegistered(message.Author.Id))
                {
                    await GetUserActivitiesAsync(message);
                }
                else
                {
                    await message.Channel.SendMessageAsync("Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***");
                }
            }
            else if (command == "мої партнери")
            {
                if (_database.IsDiscordUserRegistered(message.Author.Id))
                {
                    await GetUserPartnersAsync(message);
                }
                else
                {
                    await message.Channel.SendMessageAsync("Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***");
                }
            }
            else if (command == "реєстрація")
            {
                if (_database.IsDiscordUserRegistered(message.Author.Id))
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

                if (!_database.IsDiscordUserRegistered(message.Author.Id) && nickname.Length > 0)
                {
                    await RegisterAsync(message, nickname);
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
