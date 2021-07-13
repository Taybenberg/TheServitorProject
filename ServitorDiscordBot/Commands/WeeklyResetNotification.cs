using DataProcessor;
using DataProcessor.Localization;
using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task WeeklyResetNotificationAsync(IMessageChannel channel = null)
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
    }
}
