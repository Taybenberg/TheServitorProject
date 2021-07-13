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

                case "!delete_this":
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    try
                    {
                        var msg = await message.Channel.GetMessageAsync(message.Reference.MessageId.Value);
                        await msg.DeleteAsync();
                        await message.DeleteAsync();
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!delete_after"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    try
                    {
                        var strs = c.Split(' ');

                        if (strs.Length < 2)
                        {
                            await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат.");
                            return true;
                        }

                        int limit = int.Parse(strs[1]);

                        if (limit > 0)
                        {
                            var msg = await message.Channel.GetMessageAsync(message.Reference.MessageId.Value);
                            var messages = await message.Channel.GetMessagesAsync(msg, Direction.After, limit).Flatten().ToListAsync();
                            var notification = await message.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

                            foreach (var m in messages)
                                await m.DeleteAsync();

                            await notification.DeleteAsync();
                            await message.DeleteAsync();
                        }
                        else
                            await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат.");
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!delete_messages"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    try
                    {
                        var gid = (message.Channel as IGuildChannel).GuildId;
                        var strs = c.Split(' ');

                        if (strs.Length < 3)
                        {
                            await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат.");
                            return true;
                        }

                        int limit = int.Parse(strs[1]);

                        (var gl, var ch, var ms) = await GetChannelMessageAsync(c);

                        if (limit > 0 && gl.Id == gid && ms is not null && ch is not null)
                        {
                            var messages = await ch.GetMessagesAsync(ms, Direction.After, limit).Flatten().ToListAsync();
                            var notification = await message.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

                            foreach (var m in messages)
                                await m.DeleteAsync();

                            await notification.DeleteAsync();
                            await message.DeleteAsync();
                        }
                        else
                            await SendTemporaryMessageAsync(message, $"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!delete_message"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    try
                    {
                        var gid = (message.Channel as IGuildChannel).GuildId;

                        (var gl, _, var ms) = await GetChannelMessageAsync(c);

                        if (gl.Id == gid && ms is not null)
                        {
                            await ms.DeleteAsync();
                            await message.DeleteAsync();
                        }
                        else
                            await SendTemporaryMessageAsync(message, $"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                    }
                    catch { }

                    return true;

                default: return false;
            }
        }

        private async Task<bool> NoDeletePermissionsAsync(IMessage message)
        {
            await SendTemporaryMessageAsync(message, "У вас відсутні права на видалення повідомлень.");

            return true;
        }
    }
}
