﻿using Discord;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task<bool> ServiceMessagesAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case "!donate":
                    await SendDonateMessageAsync(message.Channel);
                    return true;

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
                        await SendTemporaryMessageAsync(message, c.Replace("!echo ", string.Empty));
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!random"):
                    try
                    {
                        if (message.MentionedRoleIds.Count == 1)
                        {
                            var members = _client.GetGuild((message.Channel as IGuildChannel).GuildId).GetRole(message.MentionedRoleIds.First()).Members.ToArray();

                            var count = members.Length;

                            await message.Channel.SendMessageAsync($"Серед {count} користувачів моє око побачило <@{members[new Random().Next(count)].Id}> <:Illuminati:891705606631727114>");
                        }
                        else
                        {
                            var strs = c.Split(' ');

                            if (strs.Length == 2 && uint.TryParse(strs[1], out var next))
                                await message.Channel.SendMessageAsync($"Віщую вам число **{new Random().Next((int)next)}** <:Illuminati:891705606631727114>");
                            else
                                await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат. Скористайтеся командою **допомога рандом**.");
                        }
                    }
                    catch { }

                    return true;

                case string c when c.StartsWith("!delete_this"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    var msgId = message?.Reference?.MessageId.Value;

                    if (msgId is not null)
                    {
                        var msg = await message.Channel.GetMessageAsync((ulong)msgId);

                        await DeleteMessageAsync(msg);
                    }
                    else
                    {
                        var gid = (message.Channel as IGuildChannel).GuildId;

                        (var gl, _, var ms) = await GetChannelMessageAsync(c);

                        if (gl.Id == gid && ms is not null)
                            await DeleteMessageAsync(ms);
                        else
                            await SendTemporaryMessageAsync(message, $"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
                    }

                    await DeleteMessageAsync(message);

                    return true;

                case string c when c.StartsWith("!delete_before"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    await DeleteInDirectonAsync(message, c, Direction.Before, true);

                    return true;

                case string c when c.StartsWith("!delete_slow_before"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    await DeleteInDirectonAsync(message, c, Direction.Before, false);

                    return true;

                case string c when c.StartsWith("!delete_after"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    await DeleteInDirectonAsync(message, c, Direction.After, true);

                    return true;

                case string c when c.StartsWith("!delete_slow_after"):
                    if (!CheckModerationRole(message.Author))
                        return await NoDeletePermissionsAsync(message);

                    await DeleteInDirectonAsync(message, c, Direction.After, false);

                    return true;

                default: return false;
            }
        }

        private async Task<bool> NoDeletePermissionsAsync(IMessage message)
        {
            await SendTemporaryMessageAsync(message, "У вас відсутні права на видалення повідомлень.");

            return true;
        }

        private async Task DeleteMessagesAsync(IMessage userMessage, IMessageChannel chDel, IMessage msDel, int limit, Direction dir, bool isFast)
        {
            var messages = await chDel.GetMessagesAsync(msDel, dir, limit).Flatten().ToListAsync();

            var notification = await userMessage.Channel.SendMessageAsync($"Чистка {messages.Count} повідомлень...");

            if (isFast)
            {
                try
                {
                    await ((ITextChannel)chDel).DeleteMessagesAsync(messages);
                }
                catch { }
            }
            else
            {
                foreach (var m in messages)
                    await DeleteMessageAsync(m);
            }

            await DeleteMessageAsync(notification);

            await DeleteMessageAsync(userMessage);
        }

        private async Task DeleteInDirectonAsync(IMessage message, string str, Direction dir, bool isFast)
        {
            var strs = str.Split(' ');

            var msgId = message?.Reference?.MessageId.Value;

            if (msgId is not null)
            {
                if (strs.Length < 2)
                {
                    await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат. Скористайтеся командою **допомога адмін**.");
                    return;
                }

                int limit = -1;
                int.TryParse(strs[1], out limit);

                if (limit > 0)
                {
                    var msg = await message.Channel.GetMessageAsync((ulong)msgId);

                    await DeleteMessagesAsync(message, msg.Channel, msg, limit, dir, isFast);
                }
                else
                    await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат. Скористайтеся командою **допомога адмін**.");
            }
            else
            {
                var gid = (message.Channel as IGuildChannel).GuildId;

                if (strs.Length < 3)
                {
                    await SendTemporaryMessageAsync(message, "Ви ввели команду в хибному форматі. Перевірте формат. Скористайтеся командою **допомога адмін**.");
                    return;
                }

                int limit = -1;
                int.TryParse(strs[1], out limit);

                (var gl, var ch, var ms) = await GetChannelMessageAsync(strs[2]);

                if (limit > 0 && gl.Id == gid && ms is not null && ch is not null)
                {
                    await DeleteMessagesAsync(message, ch, ms, limit, dir, isFast);
                }
                else
                    await SendTemporaryMessageAsync(message, $"Сталася помилка під час виконання команди. Можливо вказане повідомлення більше не існує вбо формат команди хибний.");
            }
        }
    }
}
