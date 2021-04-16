using BungieNetApi;
using Discord;
using Discord.WebSocket;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task Bumper_Notify(Dictionary<string, DateTime> users)
        {
            _logger.LogInformation($"{DateTime.Now} Bump notification");

            IMessageChannel channel = _client.GetChannel(_bumpChannelId) as IMessageChannel;

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessageColors.BumpNotification);

            builder.Description = "Саме час **!bump**-нути :alarm_clock:";

            if (users.Count > 0)
            {
                builder.Description += "\nКулдаун до:";

                foreach (var user in users)
                    builder.Description += $"\n<@{user.Key}> – *{user.Value.ToString("HH:mm:ss")}*";
            }

            string mentions = string.Empty;

            foreach (var id in _bumpPingUsers.Where(x => !users.ContainsKey(x)))
                mentions += $"<@{id}> ";

            await channel.SendMessageAsync(mentions, embed: builder.Build());
        }

        public async Task DailyResetNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Daily reset");

            int currWeek = (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessageColors.Reset);

            builder.Title = $"Тиждень {currWeek}";

            builder.Description = "Відбувся денний ресет";

            builder.Footer = GetFooter();

            var channel = _client.GetChannel(_channelId) as IMessageChannel;

            await channel.SendMessageAsync(embed: builder.Build());

            await GetResourcesPoolAsync();

            await GetLostSectorsLootAsync();
        }

        public async Task GetWeeklyMilestoneAsync(SocketMessage message = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            var milestone = await apiCient.GetMilestonesAsync();

            int currWeek = (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessageColors.Reset);

            builder.Title = $"Тиждень {currWeek}";

            string additionalDescription = string.Empty;

            if (await ExtensionMethods.IsIronBannerAvailableAsync())
            {
                builder.ThumbnailUrl = "https://bungie.net/common/destiny2_content/icons/0ee91b79ba1366243832cf810afc3b75.jpg";

                additionalDescription = $"Доступний **{Localization.StatsActivityNames[BungieNetApi.ActivityType.IronBannerControl][0]}**!";
            }
                
            var mode = Localization.StatsActivityNames.FirstOrDefault(x => x.Value[1].ToLower() == milestone.CrucibleRotationModeName.ToLower()).Value;

            builder.Fields = new()
            {
                new EmbedFieldBuilder
                {
                    Name = "Найтфол",
                    Value = milestone.NightfallTheOrdealName,
                    IsInline = true
                },
                new EmbedFieldBuilder
                {
                    Name = "Ротація горнила",
                    Value = $"{mode[0]} | {mode[1]}",
                    IsInline = true
                }
            };

            builder.ImageUrl = milestone.NightfallTheOrdealImage;

            builder.Footer = GetFooter();

            if (message is null)
            {
                _logger.LogInformation($"{DateTime.Now} Weekly reset");

                builder.Description = $"Відбувся тижневий ресет\n{additionalDescription}";

                var channel = _client.GetChannel(_channelId) as IMessageChannel;

                await channel.SendMessageAsync(embed: builder.Build());

                await GetEververseInventoryAsync(week: currWeek.ToString());
            }
            else
            {
                builder.Description = additionalDescription;

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }

        public async Task XurNotificationAsync(SocketMessage message = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            using var inventory = await apiCient.GetXurInventoryAsync(message is not null);

            IMessageChannel channel;

            if (message is null)
            {
                _logger.LogInformation($"{DateTime.Now} Xur arrived");

                channel = _client.GetChannel(_channelId) as IMessageChannel;

                var builder = new EmbedBuilder();

                builder.Color = GetColor(MessageColors.Xur);

                builder.Title = $"Зур привіз свіжий крам";

                builder.Footer = GetFooter();

                await channel.SendMessageAsync(embed: builder.Build());
            }
            else
                channel = message.Channel;

            await channel.SendFileAsync(inventory, "XurInventory.png");
        }
    }
}
