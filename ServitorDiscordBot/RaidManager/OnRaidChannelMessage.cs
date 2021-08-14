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
                case string c
                    when c is "допомога":
                    await GetHelpOnCommandAsync(message, "рейд");
                    break;

                case string c
                when c.StartsWith("рейд"):
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
    }
}
