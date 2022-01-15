﻿using CommonData.DiscordEmoji;
using CommonData.Localization;
using CommonData.RaidManager;
using ActivityService;
using Discord;
using Discord.WebSocket;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitActivityAsync(ActivityContainer container)
        {
            string icon, activityName;
            var activityType = container.ActivityType;

            if (activityType is BungieNetApi.Enums.ActivityType.Raid && container.ActivityName is not null)
            {
                var raid = GetRaidType(container.ActivityName);

                icon = Emote.Parse(EmojiContainer.GetRaidEmoji(raid)).Url;
                activityName = TranslationDictionaries.RaidTypes[raid];

                if (activityName is null)
                    activityName = container.ActivityName;
            }
            else
            {
                icon = Emote.Parse(EmojiContainer.GetActivityEmoji(activityType)).Url;

                if (container.ActivityName is null)
                    activityName = TranslationDictionaries.ActivityNames[activityType][0];
                else
                    activityName = container.ActivityName;
            }

            var users = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Організатор збору",
                    Value = $"<@{container.Users.FirstOrDefault()}>"
                }
            };

            var fireteam = container.Users.Skip(1).Take(5);
            if (fireteam.Count() > 0)
                users.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Бойова група",
                    Value = string.Join("\n", fireteam.Select(x => $"<@{x}>"))
                });

            var reserve = container.Users.Skip(6);
            if (reserve.Count() > 0)
                users.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Лава запасних",
                    Value = string.Join("\n", reserve.Select(x => $"<@{x}>"))
                });

            var builder = new EmbedBuilder()
                .WithColor(new Color(0xFFFFFF))
                .WithThumbnailUrl(icon)
                .WithTitle($"{activityName} @ {container.PlannedDate.ToString("dd.MM.yyyy HH:mm")}")
                .WithDescription(container.Description)
                .WithFields(users);

            IMessageChannel channel = _client.GetChannel(container.ChannelID) as IMessageChannel;

            var componentBuilder = new ComponentBuilder()
                .WithButton("Підписатися", "ActivitierSubscribe", ButtonStyle.Secondary, Emote.Parse(EmojiContainer.Check))
                .WithButton("Відписатися", "ActivitierUnsubscribe", ButtonStyle.Secondary, Emote.Parse(EmojiContainer.UnCheck));

            await channel.SendMessageAsync($"<@&{_destinyRoleId}>", embed: builder.Build(), components: componentBuilder.Build());
        }
    }
}