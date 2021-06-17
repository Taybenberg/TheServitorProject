using BungieNetApi;
using Discord;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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

            builder.Color = GetColor(MessagesEnum.BumpNotification);

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

            builder.Color = GetColor(MessagesEnum.Reset);

            builder.Title = $"Тиждень {currWeek}";

            builder.Description = "Відбувся денний ресет";

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendMessageAsync(embed: builder.Build());

            await GetResourcesPoolAsync();

            await GetLostSectorsLootAsync();

            var roadmap = RoadmapParser.GetRoadmap();

            if (roadmap is not null)
                await channel.SendFileAsync(roadmap, "Roadmap.png");
        }

        public async Task GetWeeklyMilestoneAsync(IMessageChannel channel = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            var milestone = await apiCient.GetMilestonesAsync();

            int currWeek = (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Reset);

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

            if (channel is null)
            {
                _logger.LogInformation($"{DateTime.Now} Weekly reset");

                builder.Description = $"Відбувся тижневий ресет\n{additionalDescription}";

                channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

                await channel.SendMessageAsync(embed: builder.Build());

                await GetEververseInventoryAsync(week: currWeek.ToString());
            }
            else
            {
                builder.Description = additionalDescription;

                await channel.SendMessageAsync(embed: builder.Build());
            }
        }

        private ConcurrentDictionary<ulong, ulong> xurInventory = new();
        public async Task XurNotificationAsync(IMessageChannel channel = null)
        {
            using var scope = _scopeFactory.CreateScope();

            var apiCient = scope.ServiceProvider.GetRequiredService<BungieNetApiClient>();

            using var inventory = await apiCient.GetXurInventoryAsync(channel is not null);

            if (channel is null)
            {
                _logger.LogInformation($"{DateTime.Now} Xur arrived");

                channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

                var builder = new EmbedBuilder();

                builder.Color = GetColor(MessagesEnum.Xur);

                builder.Title = $"Зур привіз свіжий крам";

                await channel.SendMessageAsync(embed: builder.Build());
            }

            var message = await channel.SendFileAsync(inventory, "XurInventory.png");

            if (!xurInventory.TryAdd(channel.Id, message.Id))
            {
                var ch = _client.GetChannel(channel.Id) as IMessageChannel;

                var msg = await ch.GetMessageAsync(xurInventory[channel.Id]);
                await msg.DeleteAsync();

                xurInventory[channel.Id] = message.Id;
            }
        }
    }
}
