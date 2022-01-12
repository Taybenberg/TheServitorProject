using DataProcessor.DiscordEmoji;
using DataProcessor.RaidManager;
using Discord;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataProcessor.DiscordEmoji;
using DataProcessor.Localization;
using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task RaidSelectMenuExecutedAsync(SocketMessageComponent component)
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

                        await component.RespondAsync(embed: builder.Build(), components: componentBuilder.Build());
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

                        var msg = await component.Channel.SendMessageAsync($"<@&{_destinyRoleId}>", embed: builder.Build());

                        await _raidManager.AddRaidAsync(msg.Id, raid);

                        await msg.AddReactionsAsync(new string[]
                            { EmojiContainer.Check, EmojiContainer.UnCheck }
                            .Select(x => Emote.Parse(x)).ToArray());
                    }
                    break;

                default: break;
            }
        }

        private async Task OnRaidChannelMessageAsync(IMessage message)
        {
            var command = message.Content.ToLower();

            switch (command)
            {
                case "допомога":
                    await GetHelpOnCommandAsync(message, "рейд");
                    break;

                case "конструктор рейду":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xE8A427))
                            .WithDescription("Швидко зберіть рейд на найближчий час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть рейд")
                            .WithCustomId("QuickRaidSelector")
                            .WithMinValues(1).WithMaxValues(1)
                            .AddOption(TranslationDictionaries.RaidTypes[RaidType.LW], "QuickRaid_LW", emote: Emote.Parse(EmojiContainer.GetRaidEmoji(RaidType.LW)))
                            .AddOption(TranslationDictionaries.RaidTypes[RaidType.GOS], "QuickRaid_GOS", emote: Emote.Parse(EmojiContainer.GetRaidEmoji(RaidType.GOS)))
                            .AddOption(TranslationDictionaries.RaidTypes[RaidType.DSC], "QuickRaid_DSC", emote: Emote.Parse(EmojiContainer.GetRaidEmoji(RaidType.DSC)))
                            .AddOption(TranslationDictionaries.RaidTypes[RaidType.VOG_L], "QuickRaid_VOGL", emote: Emote.Parse(EmojiContainer.GetRaidEmoji(RaidType.VOG_L)))
                            .AddOption(TranslationDictionaries.RaidTypes[RaidType.VOG_M], "QuickRaid_VOGM", emote: Emote.Parse(EmojiContainer.GetRaidEmoji(RaidType.VOG_M)));

                        var component = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await message.Channel.SendMessageAsync(embed: builder.Build(), components: component.Build());
                    }
                    break;

                case string c
                when c.StartsWith("рейд") || c.StartsWith("!рейд"):
                    {
                        await InitRaidAsync(message);

                        await DeleteMessageAsync(message);
                    }
                    break;

                case string c
                when c is "скасувати":
                    {
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                            {
                                raid.TryStop(message.Author.Id);

                                await DeleteMessageAsync(message);
                            }
                        }
                    }
                    break;

                case string c
                when c.StartsWith("передати"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null && message.MentionedUserIds.Count == 2)
                        {
                            var userID = message.MentionedUserIds.Last();

                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                                raid.TransferPlace(message.Author.Id, userID);
                        }

                        await DeleteMessageAsync(message);
                    }
                    break;

                case string c
                when c.StartsWith("перенести"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                            {
                                try
                                {
                                    var date = DateTime.ParseExact(command.Replace("перенести ", string.Empty), "dd.MM-HH:mm", CultureInfo.CurrentCulture);

                                    if (date < DateTime.Now)
                                        date = date.AddYears(1);

                                    if (DateTime.Now.AddMonths(1) < date)
                                        throw new Exception();

                                    raid.UpdateDate(message.Author.Id, date);
                                }
                                catch { }
                            }
                        }

                        await DeleteMessageAsync(message);
                    }
                    break;

                case string c
                when c.StartsWith("зарезервувати"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                                raid.AddUsers(message.Author.Id, message.MentionedUserIds.Skip(1));
                        }

                        await DeleteMessageAsync(message);
                    }
                    break;
            }
        }

        private async Task Event_Update(RaidContainer container)
        {
            IMessageChannel channel = _client.GetChannel(_raidChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            container.DecorateBuilder(builder);

            try
            {
                await channel.ModifyMessageAsync(container.ID, msg => msg.Embed = builder.Build());
            }
            catch { }
        }

        private async Task Event_Notify(RaidContainer container)
        {
            IMessageChannel channel = _client.GetChannel(_raidChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            builder.ThumbnailUrl = container.RaidIcon;

            builder.Description = $"За 10 хвилин почнеться рейд {container.RaidName}!";

            var users = container.ReservationsOrdered.Take(6);

            builder.Fields = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder
                {
                    Name = "Бойова група",
                    Value = string.Join("\n", users.Select(x => $"<@{x.ID}>"))
                }
            };

            var builded = builder.Build();

            foreach (var user in users)
            {
                try
                {
                    var u = await _client.Rest.GetUserAsync(user.ID);

                    await u.SendMessageAsync(embed: builded);
                }
                catch { }
            }
        }

        private async Task Event_Delete(ulong messageID)
        {
            IMessageChannel channel = await _client.Rest.GetChannelAsync(_raidChannelId) as IMessageChannel;

            try
            {
                await channel.DeleteMessageAsync(messageID);
            }
            catch { }
        }

        private RaidType GetRaidType(string raidType) =>
            raidType.ToLower() switch
            {
                "lw" or "лв" or "об" => RaidType.LW,
                "gos" or "сп" or "сс" => RaidType.GOS,
                "dsc" or "сгк" => RaidType.DSC,
                "vog" or "вог" or "кс" => RaidType.VOG_L,
                "vogm" or "вогм" or "ксм" => RaidType.VOG_M,
                _ => throw new Exception()
            };

        private async Task InitRaidAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            try
            {
                var raid = new RaidContainer();

                var command = message.Content.Substring(5).TrimStart();

                var raidType = command.Substring(0, command.IndexOf(' '));

                raid.RaidType = GetRaidType(raidType);

                command = command.Remove(0, command.IndexOf(' ') + 1);

                int index = command.IndexOf(' ');

                var date = DateTime.ParseExact((index > 0 ? command.Substring(0, index) : command), "d.M-H:m", CultureInfo.CurrentCulture);

                if (date < DateTime.Now)
                    date = date.AddYears(1);

                if (DateTime.Now.AddMonths(1) < date)
                    throw new Exception();

                raid.PlannedDate = date;

                raid.AddUser(message.Author.Id);

                raid.AddUsers(message.Author.Id, message.MentionedUserIds);

                command = Regex.Replace(command, "<@\\D?\\d+>", string.Empty);

                if (index > 0)
                    raid.Description = command.Remove(0, index + 1);

                raid.DecorateBuilder(builder);

                var msg = await message.Channel.SendMessageAsync($"<@&{_destinyRoleId}>", embed: builder.Build());

                await _raidManager.AddRaidAsync(msg.Id, raid);

                await msg.AddReactionsAsync(new string[]
                    { EmojiContainer.Check, EmojiContainer.UnCheck }
                    .Select(x => Emote.Parse(x)).ToArray());
            }
            catch
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = $"Сталася помилка під час створення рейду. Перевірте, чи формат команди коректний.\nЩоби переглянути довідку, скористайтеся командою **допомога**.";

                await SendTemporaryMessageAsync(message, builder);
            }
        }
    }
}