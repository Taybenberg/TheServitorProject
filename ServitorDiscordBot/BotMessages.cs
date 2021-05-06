using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Channel.Id == _bumpChannelId && message.Author.IsBot && message.Embeds.Count > 0)
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

                        builder.Color = GetColor(MessageColors.Bumped);

                        builder.Description = $":alarm_clock: :ok_hand:\n:fast_forward: {_bumper.NextBump.ToString("HH:mm:ss")}";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                }
            }

            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || !_channelId.Any(x => x == message.Channel.Id))
                return;

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
                builder.Author.IconUrl = _serverIconUrl;
                builder.Author.Name = $"На варті серверу {_serverName}";

                builder.Title = "Допомога";
                builder.Description = $"Я **{_client.CurrentUser.Username}**, " +
                    $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                    $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                    $"Наразі Авангард надав мені роль обчислювальної машини для збору статистичних даних про діяльність вашого [клану]({_clanUrl})." +
                    $"\n**Зараз я вмію виконувати наступні функції:**\n" +
                    $"\n***біп*** - *запит на перевірку моєї працездатності*\n" +
                    $"\n***тиждень*** - *переглянути інформацію про поточний тиждень*\n" +
                    $"\n***сектори*** - *переглянути лутпул сьогоднішніх загублених секторів*\n" +
                    $"\n***ресурси*** - *переглянути поточний асортимент вендорів*\n" +
                    $"\n***зур*** - *переглянути інвентар Зура*\n" +
                    $"\n***осіріс*** - *переглянути нагороди за випробування Осіріса*\n" +
                    $"\n***еверверс*** - *переглянути поточний асортимент Тесс Еверіс*\n" +
                    $"\n***еверверс %тиждень%*** - *переглянути асортимент Тесс Еверіс за визначений тиждень (1-13)*\n" +
                    $"\n***мої активності*** - *кількість активностей ґардіана у цьому році*\n" +
                    $"\n***мої побратими*** - *список побратимів ґардіана*\n" +
                    $"\n***кланові активності*** - *кількість активностей клану в цьому році*\n" +
                    $"\n***режими*** - *список типів активностей*\n" +
                    $"\n***статистика клану %режим%*** - *агрегована статистика клану в типі активності*\n" +
                    $"\n***дошка лідерів %режим%*** - *список лідерів у типі активності*\n" +
                    $"\n***відступники*** - *виявити потенційно небезпечні активності окрім нальотів*\n" +
                    $"\n***100K*** - *виявити потенційно небезпечні нальоти з сумою очок більше 100К*\n" +
                    $"\n***реєстрація*** - *прив'язати акаунт Destiny 2 до профілю в Discord*";

                builder.Footer = GetFooter();

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
            else if (command is "тиждень" or "weekly")
            {
                await ExecuteWaitMessageAsync(message, GetWeeklyMilestoneAsync);
            }
            else if (command is "сектори" or "sectors")
            {
                await ExecuteWaitMessageAsync(message, GetLostSectorsLootAsync);
            }
            else if (command is "ресурси" or "resources")
            {
                await ExecuteWaitMessageAsync(message, GetResourcesPoolAsync);
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
            else if (command is "мої побратими" or "my partners")
            {
                await GetUserPartnersAsync(message);
            }
            else if (command is "реєстрація" or "register")
            {
                await RegisterMessageAsync(message);
            }
            else if (command is "100k" or "100к")
            {
                await ExecuteWaitMessageAsync(message, FindSuspiciousAsync, true);
            }
            else if (command is "відступники" or "apostates")
            {
                await ExecuteWaitMessageAsync(message, FindSuspiciousAsync, false);
            }
            else if (command is "зур" or "xur")
            {
                await ExecuteWaitMessageAsync(message, XurNotificationAsync);
            }
            else if (command is "осіріс" or "osiris")
            {
                await ExecuteWaitMessageAsync(message, GetOsirisInventoryAsync);
            }
            else if (command.Contains("еверверс"))
            {
                var week = command.Replace("еверверс", "").TrimStart();

                await ExecuteWaitMessageAsync(message, GetEververseInventoryAsync, week);
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

                await ExecuteWaitMessageAsync(message, ClanStatsAsync, mode);
            }
            else if (command.Contains("дошка лідерів"))
            {
                var mode = command.Replace("дошка лідерів ", "");

                await ExecuteWaitMessageAsync(message, LeaderboardAsync, mode);
            }
        }
    }
}
