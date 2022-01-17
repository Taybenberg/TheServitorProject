using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnMessageReceivedAsync(IMessage message)
        {
            if (message.Channel.Id == _bumpChannelId && message.Author.IsBot && message.Embeds.Count > 0)
            {
                await InitBumpAsync(message);
                return;
            }

            if (message.Author.IsBot)
                return;

            if (await ServiceMessagesAsync(message))
                return;

            if (message.Channel.Id == _lulzChannelId)
            {
                await LulzChannelManagerAsync(message);
                return;
            }

            if (message.Channel.Id == _musicChannelId)
            {
                await MusicPlayerMessageReceivedAsync(message);
                return;
            }

            if (_activityChannelId.Any(x => x == message.Channel.Id))
            {
                await ActivityMessageReceivedAsync(message);
                return;
            }

            if (_channelId.Any(x => x == message.Channel.Id))
                switch (message.Content.ToLower())
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
                    .Any(x => c.StartsWith(x)):
                        var helpCommand = c
                            .Replace(messageCommands[MessagesEnum.Help]
                            .Where(x => c.IndexOf(x) == 0).First() + " ", string.Empty);
                        await GetHelpOnCommandAsync(message, helpCommand);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.Register]
                    .Contains(c):
                        await ExecuteWaitMessageAsync<string>(message, TryRegisterUserAsync, arg: null);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.NotRegistered]
                    .Any(x => c.StartsWith(x)):
                        var nickname = c
                            .Replace(messageCommands[MessagesEnum.NotRegistered]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, TryRegisterUserAsync, nickname);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.Modes]
                    .Contains(c):
                        await GetModesAsync(message);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.Weekly]
                    .Contains(c):
                        await ExecuteWaitMessageAsync(message, GetWeeklyMilestoneAsync);
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
                    .Any(x => c.StartsWith(x)):
                        var cPeriod = c
                            .Replace(messageCommands[MessagesEnum.ClanActivities]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, GetClanActivitiesAsync, cPeriod);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.ClanStats]
                    .Any(x => c.StartsWith(x)):
                        var csMode = c
                            .Replace(messageCommands[MessagesEnum.ClanStats]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, GetClanStatsAsync, csMode);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.Leaderboard]
                    .Any(x => c.StartsWith(x)):
                        var lbMode = c
                            .Replace(messageCommands[MessagesEnum.Leaderboard]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, GetLeaderboardAsync, lbMode);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.MyGrandmasters]
                    .Contains(c):
                        await ExecuteWaitMessageAsync(message, GetMyGrandmastersAsync);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.MyRaids]
                    .Contains(c):
                        await ExecuteWaitMessageAsync(message, GetMyRaidsAsync);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.MyActivities]
                    .Any(x => c.StartsWith(x)):
                        var aPeriod = c
                            .Replace(messageCommands[MessagesEnum.MyActivities]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, GetMyActivitiesAsync, aPeriod);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.MyPartners]
                    .Any(x => c.StartsWith(x)):
                        var pPeriod = c
                            .Replace(messageCommands[MessagesEnum.MyPartners]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, GetMyPartnersAsync, pPeriod);
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
                    when messageCommands[MessagesEnum.EververseAll]
                    .Contains(c):
                        await ExecuteWaitMessageAsync(message, GetEververseFullInventoryAsync);
                        break;

                    case string c
                    when messageCommands[MessagesEnum.Eververse]
                    .Any(x => c.StartsWith(x)):
                        var week = c
                            .Replace(messageCommands[MessagesEnum.Eververse]
                            .Where(x => c.StartsWith(x)).First(), string.Empty)
                            .TrimStart();
                        await ExecuteWaitMessageAsync(message, GetEververseInventoryAsync, week);
                        break;
                }
        }
    }
}
