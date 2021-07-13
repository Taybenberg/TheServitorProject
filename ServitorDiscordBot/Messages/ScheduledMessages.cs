using DataProcessor;
using DataProcessor.Localization;
using Discord;
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

            var builder = GetBuilder(MessagesEnum.BumpNotification, null);

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

            var builder = GetBuilder(MessagesEnum.Reset, null);

            builder.Description = "Відбувся денний ресет";

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendMessageAsync(embed: builder.Build());

            await GetLostSectorsLootAsync(channel);

            await GetResourcesPoolAsync(channel);

            using var roadmap = await getFactory().GetRoadmapAsync();

            if (roadmap is not null)
                await channel.SendFileAsync(roadmap, "Roadmap.png");
        }

        public async Task GetWeeklyMilestoneAsync(IMessageChannel channel = null)
        {
            var apiCient = getApiClient();

            var milestone = await apiCient.GetMilestonesAsync();

            var builder = GetBuilder(MessagesEnum.Reset, null);

            string additionalDescription = string.Empty;

            if (await ExtensionMethods.IsIronBannerAvailableAsync())
            {
                builder.ThumbnailUrl = "https://bungie.net/common/destiny2_content/icons/0ee91b79ba1366243832cf810afc3b75.jpg";

                additionalDescription = $"Доступний **{TranslationDictionaries.StatsActivityNames[BungieNetApi.Enums.ActivityType.IronBannerControl][0]}**!";
            }

            var mode = TranslationDictionaries.StatsActivityNames.FirstOrDefault(x => x.Value[1].ToLower() == milestone.CrucibleRotationModeName.ToLower()).Value;

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

            if (channel is null)
            {
                _logger.LogInformation($"{DateTime.Now} Weekly reset");

                builder.Description = $"Відбувся тижневий ресет\n{additionalDescription}";

                channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

                await channel.SendMessageAsync(embed: builder.Build());

                await GetEververseInventoryAsync(channel);
            }
            else
            {
                builder.Description = additionalDescription;

                await channel.SendMessageAsync(embed: builder.Build());
            }
        }

        public async Task XurNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Xur arrived");

            var channel = _client.GetChannel(_channelId[0]) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Xur, null, false);

            await channel.SendMessageAsync(embed: builder.Build());

            await GetXurInventoryAsync(channel, false);
        }
    }
}
