using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnMessageReceivedAsync(SocketMessage message)
        {
            if (message.Channel.Id == _bumpChannelId && message.Author.IsBot && message.Embeds.Count > 0)
            {
                await InitBumpAsync(message);
                return;
            }

            if (message.Author.IsBot)
                return;

            var command = message.Content.ToLower();

            if (await ServiceMessagesAsync(message, command) || !_channelId.Any(x => x == message.Channel.Id))
                return;

            switch (command)
            {
                case string c
                when messageCommands[MessagesEnum.Bip]
                .Contains(c):
                    await BipAsync(message);
                    break;

                case string c
                when messageCommands[MessagesEnum.Help]
                .Contains(c):
                    await GetHelpAsync(message);
                    break;

                case string c
                when messageCommands[MessagesEnum.Help]
                .Any(x => c.IndexOf(x) == 0):
                    var helpCommand = c
                        .Replace(messageCommands[MessagesEnum.Help]
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await GetHelpOnCommandAsync(message, helpCommand);
                    break;

                case string c
                when messageCommands[MessagesEnum.Register]
                .Contains(c):
                    await RegisterMessageAsync(message);
                    break;

                case string c
                when messageCommands[MessagesEnum.NotRegistered]
                .Any(x => c.IndexOf(x) == 0):
                    var nickname = c
                        .Replace(messageCommands[MessagesEnum.NotRegistered]
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await TryRegisterUserAsync(message, nickname);
                    break;

                case string c
                when messageCommands[MessagesEnum.Modes]
                .Contains(c):
                    await GetModesAsync(message);
                    break;

                case string c
                when messageCommands[MessagesEnum.Weekly]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, WeeklyResetNotificationAsync);
                    break;

                case string c
                when messageCommands[MessagesEnum.Sectors]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetLostSectorsLootAsync);
                    break;

                case string c
                when messageCommands[MessagesEnum.Resources]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetResourcesPoolAsync);
                    break;

                case string c
                when messageCommands[MessagesEnum.ClanActivities]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetClanActivitiesAsync);
                    break;

                case string c
                when messageCommands[MessagesEnum.ClanStats]
                .Any(x => c.IndexOf(x) == 0):
                    var csMode = c
                        .Replace(messageCommands[MessagesEnum.ClanStats]
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message, GetClanStatsAsync, csMode);
                    break;

                case string c
                when messageCommands[MessagesEnum.Leaderboard]
                .Any(x => c.IndexOf(x) == 0):
                    var lbMode = c
                        .Replace(messageCommands[MessagesEnum.Leaderboard]
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message, GetLeaderboardAsync, lbMode);
                    break;

                case string c
                when messageCommands[MessagesEnum.MyActivities]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetMyActivitiesAsync);
                    break;

                case string c
                when messageCommands[MessagesEnum.MyPartners]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetMyPartnersAsync);
                    break;

                case string c
                when messageCommands[MessagesEnum._100K]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetSuspiciousActivitiesAsync, arg: true);
                    break;

                case string c
                when messageCommands[MessagesEnum.Apostates]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetSuspiciousActivitiesAsync, arg: false);
                    break;

                case string c
                when messageCommands[MessagesEnum.Xur]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetXurInventoryAsync, true, deleteSenderMessage: true);
                    break;

                case string c
                when messageCommands[MessagesEnum.Osiris]
                .Contains(c):
                    await ExecuteWaitMessageAsync(message, GetOsirisInventoryAsync, deleteSenderMessage: true);
                    break;

                case string c
                when messageCommands[MessagesEnum.Eververse]
                .Any(x => c.IndexOf(x) == 0):
                    var week = c
                        .Replace(messageCommands[MessagesEnum.Eververse]
                        .Where(x => c.IndexOf(x) == 0).First(), string.Empty)
                        .TrimStart();
                    await ExecuteWaitMessageAsync(message, GetEververseInventoryAsync, week);
                    break;
            }
        }
    }
}
