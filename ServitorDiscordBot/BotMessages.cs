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
                var builder = new EmbedBuilder();

                builder.Color = Color.DarkPurple;

                builder.Description = "біп…";

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
            else if (command == "colors")
            {
                var builder = new EmbedBuilder();


                builder.Color = Color.Blue;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkBlue;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkerGrey;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkGreen;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkGrey;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkMagenta;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkOrange;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkPurple;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkRed;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.DarkTeal;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Default;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Gold;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Green;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.LighterGrey;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.LightGrey;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.LightOrange;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Magenta;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Orange;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Purple;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Red;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

                builder.Color = Color.Teal;
                builder.Description = builder.Color.ToString();
                await message.Channel.SendMessageAsync(embed: builder.Build());

            }
            else if (command == "допомога")
            {
                var builder = new EmbedBuilder();

                builder.Color = Color.Purple;

                builder.Author = new();
                builder.Author.Url = clanUrl;
                builder.Author.IconUrl = clanIconUrl;
                builder.Author.Name = $"На варті клану {clanName}";

                builder.Title = "Допомога";
                builder.Description = $"Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого [клану]({clanUrl})." +
                    $"\n\n**Зараз я вмію виконувати наступні функції:**" +
                    $"\n***біп*** - *запит на перевірку моєї працездатності*" +
                    $"\n***мої активності*** - *кількість активностей ґардіана у цьому році*" +
                    $"\n***мої партнери*** - *список партнерів ґардіана*" +
                    $"\n***реєстрація*** - *прив'язати акаунт Destiny 2 до профілю в Discord*" +
                    $"\n***відступники*** - *виявити потенційно небезпечні активності окрім нальотів*" +
                    $"\n***100K*** - *виявити потенційно небезпечні нальоти з сумою очок більше 100К*";

                builder.Footer = GetFooter();

                await message.Channel.SendMessageAsync(embed: builder.Build());
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
        }
    }
}
