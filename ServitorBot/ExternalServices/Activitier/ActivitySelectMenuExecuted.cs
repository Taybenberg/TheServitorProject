using ActivityService;
using Discord;
using Discord.WebSocket;
using System;
using System.Globalization;
using System.Threading.Tasks;


namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivitySelectMenuExecutedAsync(SocketMessageComponent component)
        {
            switch (component.Data.CustomId)
            {
                case "QuickActivitySelector":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xFFFFFF))
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
                            menuBuilder = menuBuilder.AddOption(tmpDate.ToString("dd.MM HH:mm"), tmpDate.ToString("dd.MM_HH:mm"));
                            tmpDate = tmpDate.AddMinutes(30);
                        }

                        var componentBuilder = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await component.RespondAsync(embed: builder.Build(), components: componentBuilder.Build(), ephemeral: true);
                    }
                    break;

                case string c
                when c.StartsWith("QuickActivity_"):
                    {
                        var raid = new ActivityContainer()
                        {
                            ChannelID = component.Channel.Id,
                            ActivityType = Enum.Parse<BungieNetApi.Enums.ActivityType>(c.Split('_')[1]),
                            PlannedDate = DateTime.ParseExact(string.Join(',', component.Data.Values), "dd.MM_HH:mm", CultureInfo.CurrentCulture),
                            ActivityName = null,
                            Description = null,
                            Users = new ulong[] { component.User.Id }
                        };

                        await component.DeferAsync();

                        await InitActivityAsync(raid);
                    }
                    break;

                case string c
                when c.StartsWith("QuickRaid_"):
                    {
                        var raid = new ActivityContainer()
                        {
                            ChannelID = component.Channel.Id,
                            ActivityType = BungieNetApi.Enums.ActivityType.Raid,
                            PlannedDate = DateTime.ParseExact(string.Join(',', component.Data.Values), "dd.MM_HH:mm", CultureInfo.CurrentCulture),
                            ActivityName = c.Split('_')[1],
                            Description = null,
                            Users = new ulong[] { component.User.Id }
                        };

                        await component.DeferAsync();

                        await InitActivityAsync(raid);
                    }
                    break;

                default: break;
            }
        }
    }
}