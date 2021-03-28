using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Channel.Id == bumpChannelId && message.Author.IsBot && message.Embeds.Count > 0)
            {
                _logger.LogInformation($"{DateTime.Now} Message in Bump channel");

                var mention = message.MentionedUsers.FirstOrDefault();
                var embed = message.Embeds.FirstOrDefault();

                if (mention is not null && embed is not null)
                {
                    if (embed.Description.Contains("Server bumped by"))
                    {
                        _bumper.AddUser(mention.Id, mention.Username);

                        var builder = new EmbedBuilder();

                        builder.Color = GetColor(MessageColors.Bumped);

                        builder.Description = $":alarm_clock: :ok_hand:\n:fast_forward: {_bumper.NextBump.ToString("HH:mm:ss")}";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                }
            }

#if DEBUG
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || message.Channel.Name.ToLower() != "servitor_beta")
                return;
#else
            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || message.Channel.Name.ToLower() != "destiny_bot")
                return;
#endif

            var command = message.Content.ToLower();

            if (command is "біп" or "bip")
            {
                var builder = new EmbedBuilder();

                builder.Color = GetColor(MessageColors.Bip);

                builder.Description = "біп…";

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
            else if (command is "допомога" or "help")
            {
                var builder = new EmbedBuilder();

                builder.Color = GetColor(MessageColors.Help);

                builder.Author = new();
                builder.Author.Url = clanUrl;
                builder.Author.IconUrl = serverIconUrl;
                builder.Author.Name = $"На варті серверу {serverName}";

                builder.Title = "Допомога";
                builder.Description = $"Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого [клану]({clanUrl})." +
                    $"\n\n**Зараз я вмію виконувати наступні функції:**" +
                    $"\n***біп*** - *запит на перевірку моєї працездатності*" +
                    $"\n***зур*** - *переглянути інвентар Зура*" +
                    $"\n***осіріс*** - *переглянути нагороди за випробування Осіріса*" +
                    $"\n***еверверс*** - *переглянути поточний асортимент Тесс Еверіс*" +
                     $"\n***еверверс %тиждень%*** - *переглянути асортимент Тесс Еверіс за визначений тиждень*" +
                    $"\n***мої активності*** - *кількість активностей ґардіана у цьому році*" +
                    $"\n***мої партнери*** - *список партнерів ґардіана*" +
                    $"\n***кланові активності*** - *кількість активностей клану в цьому році*" +
                    $"\n***статистика клану %режим%*** - *агрегована статистика клану в типі активності*" +
                    $"\n***дошка лідерів %режим%*** - *список лідерів у типі активності*" +
                    $"\n***режими*** - *список типів активностей*" +
                    $"\n***відступники*** - *виявити потенційно небезпечні активності окрім нальотів*" +
                    $"\n***100K*** - *виявити потенційно небезпечні нальоти з сумою очок більше 100К*" +
                    $"\n***реєстрація*** - *прив'язати акаунт Destiny 2 до профілю в Discord*";

                builder.Footer = GetFooter();

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
            else if (command is "режими" or "modes")
            {
                await GetModesAsync(message);
            }
            else if (command is "кланові активності" or "clan activities")
            {
                await GetClanActivitiesAsync(message);
            }
            else if (command is "мої активності" or "my activities")
            {
                await GetUserActivitiesAsync(message);
            }
            else if (command is "мої партнери" or "my partners")
            {
                await GetUserPartnersAsync(message);
            }
            else if (command is "реєстрація" or "register")
            {
                await RegisterMessageAsync(message);
            }
            else if (command is "100k" or "100к")
            {
                await FindSuspiciousAsync(message, true);
            }
            else if (command is "відступники" or "apostates")
            {
                await FindSuspiciousAsync(message, false);
            }
            else if (command is "зур" or "xur")
            {
                await XurNotificationAsync(message);
            }
            else if (command is "осіріс" or "osiris")
            {
                await GetOsirisInventoryAsync(message);
            }
            else if (command.Contains("еверверс"))
            {
                var week = command.Replace("еверверс", "").TrimStart();

                await EververseNotificationAsync(message, week);
            }
            else if (command is "my_id")
            {
                var m = await message.Channel.SendMessageAsync(message.Author.Id.ToString());

                await Task.Delay(5000);

                await m.DeleteAsync();
            }
            else if (command is "channel_id")
            {
                var m = await message.Channel.SendMessageAsync(message.Channel.Id.ToString());

                await Task.Delay(5000);

                await m.DeleteAsync();
            }
            else if (command.Contains("зареєструвати"))
            {
                var nickname = command.Replace("зареєструватися ", "").Replace("зареєструватись ", "").Replace("зареєструвати ", "");

                await TryRegisterUserAsync(message, nickname);
            }
            else if (command.Contains("статистика клану"))
            {
                var mode = command.Replace("статистика клану ", "");

                await ClanStatsAsync(message, mode);
            }
            else if (command.Contains("дошка лідерів"))
            {
                var mode = command.Replace("дошка лідерів ", "");

                await LeaderboardAsync(message, mode);
            }
        }
    }
}
