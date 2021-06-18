using Discord;
using Discord.WebSocket;
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

                case string c when c.StartsWith("!delete_messages"):
                    {
                        if (!CheckModerationRole(message.Author))
                        {
                            await message.Channel.SendMessageAsync($"У вас відсутні права на видалення повідомлень.");

                            return true;
                        }

                        (var ch, var ms) = await GetChannelMessageAsync(c);

                        if (ms is not null && ch is not null)
                        {
                            try
                            {
                                var messages = await ch.GetMessagesAsync(ms, Direction.After).Flatten().ToListAsync();

                                var notification = await message.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

                                foreach (var m in messages)
                                {
                                    await m.DeleteAsync();
                                }

                                await notification.DeleteAsync();
                            }
                            catch (Exception) { }
                        }

                        return true;
                    }

                case string c when c.StartsWith("!delete_message"):
                    {
                        if (!CheckModerationRole(message.Author))
                        {
                            await message.Channel.SendMessageAsync($"У вас відсутні права на видалення повідомлень.");

                            return true;
                        }

                        (_, var ms) = await GetChannelMessageAsync(c);

                        if (ms is not null)
                        {
                            try
                            {
                                await ms.DeleteAsync();

                                await message.DeleteAsync();
                            }
                            catch (Exception) { }
                        }

                        return true;
                    }

                default: return false;
            }
        }

    }
}
