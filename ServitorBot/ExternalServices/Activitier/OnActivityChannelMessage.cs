using ActivityService;
using CommonData.DiscordEmoji;
using CommonData.Localization;
using CommonData.Activities;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnActivityChannelMessageAsync(IMessage message)
        {
            switch (message.Content)
            {
                case "!швидкий рейд":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xFFFFFF))
                            .WithDescription("Швидко зберіть рейд на найближчий час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть рейд")
                            .WithCustomId("QuickRaidSelector")
                            .WithMinValues(1).WithMaxValues(1)
                            .AddOption(Translation.ActivityRaidTypes[ActivityRaidType.LW], "QuickRaid_LW", emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(ActivityRaidType.LW)))
                            .AddOption(Translation.ActivityRaidTypes[ActivityRaidType.GOS], "QuickRaid_GOS", emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(ActivityRaidType.GOS)))
                            .AddOption(Translation.ActivityRaidTypes[ActivityRaidType.DSC], "QuickRaid_DSC", emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(ActivityRaidType.DSC)))
                            .AddOption(Translation.ActivityRaidTypes[ActivityRaidType.VOG_L], "QuickRaid_VOGL", emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(ActivityRaidType.VOG_L)))
                            .AddOption(Translation.ActivityRaidTypes[ActivityRaidType.VOG_M], "QuickRaid_VOGM", emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(ActivityRaidType.VOG_M)));

                        var component = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await message.Channel.SendMessageAsync(embed: builder.Build(), components: component.Build());
                    }
                    break;

                case string c
                when c.StartsWith('!'):
                    {
                        var container = TryParseActivityContainer(c, message);

                        if (container is not null)
                            await InitActivityAsync(container);
                        else
                        {
                            var builder = new EmbedBuilder()
                                .WithColor(new Color(0xD50000))
                                .WithTitle("Збір у активність")
                                .WithDescription($"Сталася помилка під час створення активності. Перевірте, чи формат команди коректний.\nЩоби переглянути довідку, скористайтеся командою **допомога**.");

                            await SendTemporaryMessageAsync(message, builder);
                        }
                    }
                    break;

                default: break;
            }
        }
    }
}