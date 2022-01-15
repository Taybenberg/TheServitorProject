using CommonData.Activities;
using CommonData.Localization;
using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task OnActivityChannelMessageAsync(IMessage message)
        {
            switch (message.Content)
            {
                case "!швидка активність":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xFFFFFF))
                            .WithDescription("Швидко зберіться у активність на найближчий час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть активність")
                            .WithCustomId("QuickActivitySelector")
                            .WithMinValues(1).WithMaxValues(1)
                            .WithOptions(Activity.ActivityTypes.Select(x =>
                                new SelectMenuOptionBuilder
                                {
                                    Label = Translation.StatsActivityNames[x][0],
                                    Value = $"QuickActivity_{x}",
                                    Emote = Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityEmoji(x))
                                }).ToList());

                        var component = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await message.Channel.SendMessageAsync(embed: builder.Build(), components: component.Build());
                    }
                    break;

                case "!швидкий рейд":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xFFFFFF))
                            .WithDescription("Швидко зберіть рейд на найближчий час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть рейд")
                            .WithCustomId("QuickActivitySelector")
                            .WithMinValues(1).WithMaxValues(1)
                            .WithOptions(Activity.ActivityRaidTypes.Select(x =>
                                new SelectMenuOptionBuilder
                                {
                                    Label = Translation.ActivityRaidTypes[x],
                                    Value = $"QuickRaid_{x}",
                                    Emote = Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(x))
                                }).ToList());

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