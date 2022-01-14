using CommonData.DiscordEmoji;
using CommonData.Localization;
using CommonData.RaidManager;
using Discord;
using Discord.WebSocket;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivitySelectMenuExecutedAsync(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "QuickRaidSelector":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xE8A427))
                            .WithDescription("Оберіть зручний час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть час")
                            .WithCustomId(string.Join(',', component.Data.Values))
                            .WithMinValues(1).WithMaxValues(1);

                        var startDate = DateTime.Now;
                        var endDate = startDate.AddHours(12);
                        var tmpDate = startDate.AddMinutes(30);

                        while (tmpDate < endDate)
                        {
                            Console.WriteLine(tmpDate);
                            menuBuilder = menuBuilder.AddOption(tmpDate.ToString("dd.MM HH:mm"), tmpDate.ToString("dd.MM_HH:mm"));
                            tmpDate = tmpDate.AddMinutes(30);
                        }

                        var componentBuilder = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await component.RespondAsync(embed: builder.Build(), components: componentBuilder.Build(), ephemeral: true);
                    }
                    break;

                case string c
                when c.StartsWith("QuickRaid_"):
                    {
                        var raid = new RaidContainer();

                        raid.RaidType = GetRaidType(c.Split('_')[1]);

                        raid.PlannedDate = DateTime.ParseExact(string.Join(',', component.Data.Values), "dd.MM_HH:mm", CultureInfo.CurrentCulture);

                        raid.AddUser(component.User.Id);

                        await component.DeferAsync();

                        var builder = GetBuilder(MessagesEnum.Raid, null, false);

                        raid.DecorateBuilder(builder);

                        var componentBuilder = new ComponentBuilder()
                            .WithButton("Підписатися", "ActivitierSubscribe", ButtonStyle.Secondary, Emote.Parse(EmojiContainer.Check))
                            .WithButton("Відписатися", "ActivitierUnsubscribe", ButtonStyle.Secondary, Emote.Parse(EmojiContainer.UnCheck));

                        var msg = await component.Channel.SendMessageAsync($"<@&{_destinyRoleId}>", embed: builder.Build(), components: componentBuilder.Build());

                        await _raidManager.AddRaidAsync(msg.Id, raid);
                    }
                    break;

                default: break;
            }
        }
    }
}