using Discord;
using System;
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
                    {
                        await SendTemporaryMessageAsync(message, message.Author.Id.ToString());

                        return true;
                    }

                case "!channel_id":
                    {
                        await SendTemporaryMessageAsync(message, message.Channel.Id.ToString());

                        return true;
                    }

                case "!message_id":
                    {
                        try
                        {
                            var m = await message.Channel.GetMessageAsync(message.Reference.MessageId.Value);

                            await message.Channel.SendMessageAsync(m.GetJumpUrl());
                        }
                        catch (Exception) { }

                        return true;
                    }

                case string c when c.StartsWith("!delete_messages"):
                    {
                        if (!CheckModerationRole(message.Author))
                        {
                            await message.Channel.SendMessageAsync($"У вас відсутні права на видалення повідомлень.");

                            return true;
                        }

                        try
                        {
                            var gid = (message.Channel as IGuildChannel).GuildId;

                            var strs = c.Split(' ');

                            if (strs.Length < 3)
                            {
                                await message.Channel.SendMessageAsync($"Ви ввели команду в хибному форматі. Перевірте формат.");

                                return true;
                            }

                            int limit = int.Parse(strs[1]);

                            (var gl, var ch, var ms) = await GetChannelMessageAsync(c);

                            if (limit > 0 && gl.Id == gid && ms is not null && ch is not null)
                            {
                                var messages = await ch.GetMessagesAsync(ms, Direction.After, limit).Flatten().ToListAsync();

                                var notification = await message.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

                                foreach (var m in messages)
                                {
                                    await m.DeleteAsync();
                                }

                                await notification.DeleteAsync();

                                await message.DeleteAsync();
                            }
                            else
                            {
                                await message.Channel.SendMessageAsync($"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                            }
                        }
                        catch (Exception) { }

                        return true;
                    }

                case string c when c.StartsWith("!delete_message"):
                    {
                        if (!CheckModerationRole(message.Author))
                        {
                            await message.Channel.SendMessageAsync($"У вас відсутні права на видалення повідомлень.");

                            return true;
                        }

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
                            {
                                await message.Channel.SendMessageAsync($"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                            }
                        }
                        catch (Exception) { }

                        return true;
                    }

                default: return false;
            }
        }

    }
}
