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

                        builder.Color = GetColor(MessagesEnum.Bumped);

                        builder.Description = $":alarm_clock: :ok_hand:\n:fast_forward: {_bumper.NextBump.ToString("HH:mm:ss")}";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                }
            }

            if (message.Author.Id == _client.CurrentUser.Id || message.Author.IsBot || !_channelId.Any(x => x == message.Channel.Id))
                return;

            var command = message.Content.ToLower();

            switch (command)
            {
                case string c when GetCommand(MessagesEnum.Bip).Contains(c):
                    await GetBipMessageAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.Help).Contains(c):
                    await GetHelpMessageAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.Weekly).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, GetWeeklyMilestoneAsync); break;

                case string c when GetCommand(MessagesEnum.Sectors).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, GetLostSectorsLootAsync); break;

                case string c when GetCommand(MessagesEnum.Resources).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, GetResourcesPoolAsync); break;

                case string c when GetCommand(MessagesEnum.Modes).Contains(c):
                    await GetModesAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.ClanActivities).Contains(c):
                    await GetClanActivitiesAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.MyActivities).Contains(c):
                    await GetUserActivitiesAsync(message); break;

                case string c when GetCommand(MessagesEnum.MyPartners).Contains(c):
                    await GetUserPartnersAsync(message); break;

                case string c when GetCommand(MessagesEnum.Register).Contains(c):
                    await RegisterMessageAsync(message); break;

                case string c when GetCommand(MessagesEnum._100K).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, FindSuspiciousAsync, true); break;

                case string c when GetCommand(MessagesEnum.Apostates).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, FindSuspiciousAsync, false); break;

                case string c when GetCommand(MessagesEnum.Xur).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, XurNotificationAsync); break;

                case string c when GetCommand(MessagesEnum.Osiris).Contains(c):
                    await ExecuteWaitMessageAsync(message.Channel, GetOsirisInventoryAsync); break;

                case "my_id":
                    var mid = await message.Channel.SendMessageAsync(message.Author.Id.ToString());
                    await Task.Delay(5000);
                    await mid.DeleteAsync();
                    break;

                case "channel_id":
                    var cid = await message.Channel.SendMessageAsync(message.Channel.Id.ToString());
                    await Task.Delay(5000);
                    await cid.DeleteAsync();
                    break;

                case string c when GetCommand(MessagesEnum.Eververse).Any(x => c.IndexOf(x) == 0):
                    var week = c.Replace(GetCommand(MessagesEnum.Eververse)
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message.Channel, GetEververseInventoryAsync, week);
                    break;

                case string c when GetCommand(MessagesEnum.NotRegistered).Any(x => c.IndexOf(x) == 0):
                    var nickname = c.Replace(GetCommand(MessagesEnum.NotRegistered)
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await TryRegisterUserAsync(message, nickname);
                    break;

                case string c when GetCommand(MessagesEnum.ClanStats).Any(x => c.IndexOf(x) == 0):
                    var csMode = c.Replace(GetCommand(MessagesEnum.ClanStats)
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message.Channel, ClanStatsAsync, csMode);
                    break;

                case string c when GetCommand(MessagesEnum.Leaderboard).Any(x => c.IndexOf(x) == 0):
                    var lbMode = c.Replace(GetCommand(MessagesEnum.Leaderboard)
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message, LeaderboardAsync, lbMode);
                    break;
            }
        }
    }
}
