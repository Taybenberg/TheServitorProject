using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Channel.Id == _bumpChannelId && message.Author.IsBot && message.Embeds.Count > 0)
            {
                await InitBumpAsync(message);
                return;
            }

            if (message.Author.IsBot)
                return;

            var command = message?.Content?.ToLower();

            if (await ServiceMessagesAsync(message, command) || !_channelId.Any(x => x == message.Channel.Id))
                return;

            switch (command)
            {
                case string c when GetCommand(MessagesEnum.Bip).Contains(c):
                    await GetBipMessageAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.Help).Contains(c):
                    await GetHelpMessageAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.Weekly).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetWeeklyMilestoneAsync); break;

                case string c when GetCommand(MessagesEnum.Sectors).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetLostSectorsLootAsync); break;

                case string c when GetCommand(MessagesEnum.Resources).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetResourcesPoolAsync); break;

                case string c when GetCommand(MessagesEnum.Modes).Contains(c):
                    await GetModesAsync(message.Channel); break;

                case string c when GetCommand(MessagesEnum.ClanActivities).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetClanActivitiesAsync); break;

                case string c when GetCommand(MessagesEnum.MyActivities).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetUserActivitiesAsync); break;

                case string c when GetCommand(MessagesEnum.MyPartners).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetUserPartnersAsync); break;

                case string c when GetCommand(MessagesEnum.Register).Contains(c):
                    await RegisterMessageAsync(message); break;

                case string c when GetCommand(MessagesEnum._100K).Contains(c):
                    await ExecuteWaitMessageAsync(message, FindSuspiciousAsync, arg: true); break;

                case string c when GetCommand(MessagesEnum.Apostates).Contains(c):
                    await ExecuteWaitMessageAsync(message, FindSuspiciousAsync, arg: false); break;

                case string c when GetCommand(MessagesEnum.Xur).Contains(c):
                    await ExecuteWaitMessageAsync(message, XurNotificationAsync, deleteSenderMessage: true); break;

                case string c when GetCommand(MessagesEnum.Osiris).Contains(c):
                    await ExecuteWaitMessageAsync(message, GetOsirisInventoryAsync, deleteSenderMessage: true); break;

                case string c when GetCommand(MessagesEnum.Eververse).Any(x => c.IndexOf(x) == 0):
                    var week = c.Replace(GetCommand(MessagesEnum.Eververse)
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message, GetEververseInventoryAsync, week);
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
                    await ExecuteWaitMessageAsync(message, ClanStatsAsync, csMode);
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
