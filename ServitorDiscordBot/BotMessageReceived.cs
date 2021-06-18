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

            switch (command)
            {
                case "!my_id":
                    var mid = await message.Channel.SendMessageAsync(message.Author.Id.ToString());

                    try
                    {
                        await message.DeleteAsync();
                    }
                    catch (Exception) { }

                    await Task.Delay(5000);

                    await mid.DeleteAsync();
                    return;

                case "!channel_id":
                    var cid = await message.Channel.SendMessageAsync(message.Channel.Id.ToString());

                    try
                    {
                        await message.DeleteAsync();
                    }
                    catch (Exception) { }

                    await Task.Delay(5000);

                    await cid.DeleteAsync();
                    return;

                case string c when c.StartsWith("!delete"):
                    if (!((SocketGuildUser)message.Author).Roles.Any(x => x.Name.ToLower().StartsWith("admin") || x.Name.ToLower().StartsWith("moder")))
                    {
                        await message.Channel.SendMessageAsync($"У вас відсутні права на видалення повідомлень.");
                        return;
                    }

                    var strs = c.Split('/');

                    if (strs.Length < 4)
                        return;

                    try
                    {
                        var chid = ulong.Parse(strs[^2]);
                        var msid = ulong.Parse(strs[^1]);

                        var ch = _client.GetChannel(chid) as IMessageChannel;
                        var ms = await ch.GetMessageAsync(msid);

                        if (ms is not null)
                        {
                            var messages = await ch.GetMessagesAsync(ms, Direction.After).Flatten().ToListAsync();

                            var notification = await message.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

                            foreach (var m in messages)
                            {
                                await m.DeleteAsync();
                            }

                            await notification.DeleteAsync();
                        }
                    }
                    catch (Exception) { }

                    return;
            }

            if (message.Author.Id == _client.CurrentUser.Id || !_channelId.Any(x => x == message.Channel.Id))
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
