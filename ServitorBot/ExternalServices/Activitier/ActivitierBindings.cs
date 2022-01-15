using Discord;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
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
                        /*
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                            {
                                raid.TryStop(message.Author.Id);

                                await DeleteMessageAsync(message);
                            }
                        }*/
                    }
                    break;

                case string c
                when c.StartsWith("передати"):
                    {
                        /*
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null && message.MentionedUserIds.Count == 2)
                        {
                            var userID = message.MentionedUserIds.Last();

                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                                raid.TransferPlace(message.Author.Id, userID);
                        }

                        await DeleteMessageAsync(message);
                        */
                    }
                    break;

                case string c
                when c.StartsWith("перенести"):
                    {
                        /*
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
                        */
                    }
                    break;

                case string c
                when c.StartsWith("зарезервувати"):
                    {
                        /*
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var raid = _raidManager[(ulong)msgId];

                            if (raid is not null)
                                raid.AddUsers(message.Author.Id, message.MentionedUserIds.Skip(1));
                        }

                        await DeleteMessageAsync(message);
                        */
                    }
                    break;
            }
        }

        private async Task Event_Update(RaidContainer container)
        {
            /*
            IMessageChannel channel = _client.GetChannel(_raidChannelId) as IMessageChannel;

            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            container.DecorateBuilder(builder);

            try
            {
                await channel.ModifyMessageAsync(container.ID, msg => msg.Embed = builder.Build());
            }
            catch { }*/
        }

        private async Task Event_Notify(RaidContainer container)
        {
            /*
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
            }*/
        }

        private async Task Event_Delete(ulong messageID)
        {
            /*
            IMessageChannel channel = await _client.Rest.GetChannelAsync(_raidChannelId) as IMessageChannel;

            try
            {
                await channel.DeleteMessageAsync(messageID);
            }
            catch { }*/
        }

        private async Task InitRaidAsync(IMessage message)
        {

        }
    }
}