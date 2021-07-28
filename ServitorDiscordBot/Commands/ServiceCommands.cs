using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task<bool> ServiceMessagesAsync(IMessage message, string command)
        {
            switch (command)
            {
                case "!my_id":
                    await SendTemporaryMessageAsync(message, message.Author.Id.ToString());
                    return true;

                case "!channel_id":
                    await SendTemporaryMessageAsync(message, message.Channel.Id.ToString());
                    return true;

                case "!message_id":
                    try
                    {
                        var m = await message.Channel.GetMessageAsync(message.Reference.MessageId.Value);
                        await SendTemporaryMessageAsync(message, m.GetJumpUrl());
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!echo"):
                    try
                    {
                        await message.Channel.SendMessageAsync(c.Split(' ')[1]);
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!delete_this"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    try
                    {
                        var msgId = message?.Reference?.MessageId.Value;

                        if (msgId is not null)
                        {
                            var msg = await message.Channel.GetMessageAsync((ulong)msgId);
                            await msg.DeleteAsync();
                        }
                        else
                        {
                            var gid = (message.Channel as IGuildChannel).GuildId;

                            (var gl, _, var ms) = await GetChannelMessageAsync(c);

                            if (gl.Id == gid && ms is not null)
                                await ms.DeleteAsync();
                            else
                                await SendTemporaryMessageAsync(message, $"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                        }

                        await message.DeleteAsync();
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!delete_before"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    await DeleteInDirectonAsync(message, c, Direction.Before);

                    return true;

                case string c when c.StartsWith("!delete_after"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    await DeleteInDirectonAsync(message, c, Direction.After);

                    return true;

                default: return false;
            }
        }

        private async Task<bool> NoDeletePermissionsAsync(IMessage message)
        {
            await SendTemporaryMessageAsync(message, "У вас відсутні права на видалення повідомлень.");

            return true;
        }

        private async Task DeleteMessagesAsync(IMessage userMessage, IMessageChannel chDel, IMessage msDel, int limit, Direction dir)
        {
            var messages = await chDel.GetMessagesAsync(msDel, dir, limit).Flatten().ToListAsync();

            var notification = await userMessage.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

            foreach (var m in messages)
                await m.DeleteAsync();

            await notification.DeleteAsync();

            await userMessage.DeleteAsync();
        }

        private async Task DeleteInDirectonAsync(IMessage message, string str, Direction dir)
        {
            try
            {
                var strs = str.Split(' ');

                var msgId = message?.Reference?.MessageId.Value;

                if (msgId is not null)
                {
                    if (strs.Length < 2)
                    {
                        await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат.");
                        return;
                    }

                    int limit = int.Parse(strs[1]);

                    if (limit > 0)
                    {
                        var msg = await message.Channel.GetMessageAsync((ulong)msgId);

                        await DeleteMessagesAsync(message, msg.Channel, msg, limit, dir);
                    }
                    else
                        await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат.");
                }
                else
                {
                    var gid = (message.Channel as IGuildChannel).GuildId;

                    if (strs.Length < 3)
                    {
                        await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат.");
                        return;
                    }

                    int limit = int.Parse(strs[1]);

                    (var gl, var ch, var ms) = await GetChannelMessageAsync(str);

                    if (limit > 0 && gl.Id == gid && ms is not null && ch is not null)
                    {
                        await DeleteMessagesAsync(message, ch, ms, limit, dir);
                    }
                    else
                        await SendTemporaryMessageAsync(message, $"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                }
            }
            catch { }
        }
    }
}
