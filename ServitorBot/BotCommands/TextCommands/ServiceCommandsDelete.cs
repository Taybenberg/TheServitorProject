using Discord;

namespace ServitorBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private async Task CommandDeleteAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case "!delete_this":
                    {
                        var msgId = message.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var msg = await message.Channel.GetMessageAsync(msgId.Value);

                            if (msg is not null)
                                await DeleteMessageAsync(msg);
                        }
                    }
                    break;

                case string c when c.StartsWith("!delete_before "):
                    {
                        if (int.TryParse(c.Split(' ')[1], out var limit))
                        {
                            var msgId = message.Reference?.MessageId.Value;

                            if (msgId is not null)
                                await DeleteMessagesInDirectonAsync(message.Channel, msgId.Value, limit, Direction.Before, true);
                        }
                    }
                    break;

                case string c when c.StartsWith("!delete_after "):
                    {
                        if (int.TryParse(c.Split(' ')[1], out var limit))
                        {
                            var msgId = message.Reference?.MessageId.Value;

                            if (msgId is not null)
                                await DeleteMessagesInDirectonAsync(message.Channel, msgId.Value, limit, Direction.After, true);
                        }
                    }
                    break;

                case string c when c.StartsWith("!delete_slow_before "):
                    {
                        if (int.TryParse(c.Split(' ')[1], out var limit))
                        {
                            var msgId = message.Reference?.MessageId.Value;

                            if (msgId is not null)
                                await DeleteMessagesInDirectonAsync(message.Channel, msgId.Value, limit, Direction.Before, false);
                        }
                    }
                    break;

                case string c when c.StartsWith("!delete_slow_after "):
                    {
                        if (int.TryParse(c.Split(' ')[1], out var limit))
                        {
                            var msgId = message.Reference?.MessageId.Value;

                            if (msgId is not null)
                                await DeleteMessagesInDirectonAsync(message.Channel, msgId.Value, limit, Direction.After, false);
                        }
                    }
                    break;

                default: break;
            }
        }

        private async Task DeleteMessagesInDirectonAsync(IMessageChannel channel, ulong messageReferenceID, int limit, Direction direction, bool isFast)
        {
            var messages = await channel.GetMessagesAsync(messageReferenceID, direction, limit).FlattenAsync();

            var builder = new EmbedBuilder()
                .WithColor(0xC75B39)
                .WithDescription($"Видалення {messages.Count()} повідомлень…");
            var notification = await channel.SendMessageAsync(embed: builder.Build());

            if (isFast)
            {
                try
                {
                    await ((ITextChannel)channel).DeleteMessagesAsync(messages);
                }
                catch { }
            }
            else
            {
                foreach (var msg in messages)
                    await DeleteMessageAsync(msg);
            }

            await DeleteMessageAsync(notification);
        }
    }
}
