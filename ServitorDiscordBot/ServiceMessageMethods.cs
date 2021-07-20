﻿using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private int GetWeekNumber() => 
            (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

        private bool CheckModerationRole(IUser user) =>
            ((SocketGuildUser)user).Roles.Any(x =>
            x.Name.ToLower() is "administrator" ||
            x.Name.ToLower() is "moderator" ||
            x.Name.ToLower() is "raid lead" ||
            x.Name.ToLower() is "old");

        private string GetActivityCountImpression(int count, string username) =>
            new Random().Next(10) switch
            {
                0 => $"Неймовірно! **{count}** активностей на рахунку {username}! Так тримати!",
                1 => $"Оце так! **{count}** активнностей на рахунку {username}! Куди там залізним лордам до вас!",
                2 => $"Надзвичайно! **{count}** активнностей на рахунку {username}! Та ви скажені!",
                3 => $"Непосильно! **{count}** активнностей на рахунку {username}! Вас варто боятися!",
                4 => $"Парадоксально! **{count}** активнностей на рахунку {username}! Марі Сов варто повернутися лишень заради того, щоб побачити вас!",
                5 => $"Фантастика! **{count}** активнностей на рахунку {username}! Це ви вбиваєте богів? Тоді я не здивований.",
                6 => $"Немислимо! **{count}** активнностей на рахунку {username}! Від вашого світла можна осліпнути!",
                7 => $"Незрівнянно! **{count}** активнностей на рахунку {username}! Ви виняткові!",
                8 => $"Непомірно! **{count}** активнностей на рахунку {username}! Ви ще не розтрощили якесь небесне тіло? Саме пора!",
                _ => $"Нічого собі! **{count}** активностей на рахунку {username}! Авангард шокований!"
            };

        private async Task<IUserMessage> GetWaitMessageAsync(IMessage message)
        {
            var command = message.Content.ToLower();

            var builder = GetBuilder(MessagesEnum.Wait, null, false);

            builder.Description = new Random().Next(20) switch
            {
                0 => $"Я займусь командою \"{command}\", а ви поки порахуйте бейбі фоленів.",
                1 => $"\"{command}\" туди, \"{command}\" сюди…",
                2 => $"Команду \"{command}\" прийнято до виконання.",
                3 => $"Ви готові до \"{command}\"?\nТак, так, капітане!\nЯ не чую!\nТАК, ТАК, КАПІТАНЕ!",
                4 => $"А що, якщо команда \"{command}\" це насправді ілюзія?",
                5 => $"Команда \"{command}\" витратить більше ефіру, ніж зазвичай.",
                6 => $"О, ви ще не забули про мене?\nВиконую команду \"{command}\" для вас.",
                7 => $"Виконую ваш запит \"{command}\", на це знадобиться трохи часу…",
                8 => $"Усім залишатися на своїх місцях!\nВиконується команда \"{command}\"!",
                9 => $"Нас було дев'ять, а потім виконалася команда \"{command}\"…",
                10 => $"Ви хочете, щоб я виконав команду \"{command}\", так?\nВи ж дійсно цього хочете, так?",
                11 => $"Люк, я твій батько! Ой, не туди…\nВиконую \"{command}\"…\n…\nСкажіть щось, бо якось невдобно…",
                12 => $"\"{command}\", кажете?\nСпершу женіть ефір!",
                13 => $"А я знаю, що ви хочете \"{command}\"!",
                14 => $"Подейкують, що користувачів, які мене дуже задовбували більше ніхто не бачив.\nНу ви чекайте \"{command}\", чекайте…",
                15 => $"Були часи, коли в одному рейді поміщалося по 12 ґардіанів.\nОй булоо, ой булоо…\nА ви оце різними \"{command}\" цікавитесь.",
                16 => $"Лиш вонаа, лиш вонаа, сидітиме сумна, буде пити – не п'яніти від дешевого \"{command}\".",
                17 => $"Ініціалізація модуля Kepler-186 для виконання команди \"{command}\"…",
                18 => $"Межі між реальностями \"{command}\" починають стиратися.",
                _ => $"Команда \"{command}\" виконується.\nВи завжди можете підтримати розробку бота [філіжанкою кави](https://www.buymeacoffee.com/servitor)."
            };

            return await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessage, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessage, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message, arg);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessageChannel, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message.Channel);

            await wait.DeleteAsync();
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessageChannel, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message.Channel, arg);

            await wait.DeleteAsync();
        }

        private async Task<(IGuild, IMessageChannel, IMessage)> GetChannelMessageAsync(string link)
        {
            var strs = link.Split('/');

            if (strs.Length < 4)
                return (null, null, null);

            var glid = ulong.Parse(strs[^3]);
            var chid = ulong.Parse(strs[^2]);
            var msid = ulong.Parse(strs[^1]);

            var gl = _client.GetGuild(glid) as IGuild;
            var ch = await gl.GetChannelAsync(chid) as IMessageChannel;
            var ms = await ch.GetMessageAsync(msid);

            return (gl, ch, ms);
        }

        private async Task SendTemporaryMessageAsync(IMessage message, string text)
        {
            var msg = await message.Channel.SendMessageAsync(text);

            try
            {
                await message.DeleteAsync();
            }
            catch { }

            await Task.Delay(5000);

            await msg.DeleteAsync();
        }

        private async Task DeleteMessageAsync(IMessage message, bool confirmation)
        {
            if (confirmation)
            {
                try
                {
                    await message.DeleteAsync();
                }
                catch { }
            }
        }

        private EmbedBuilder GetBuilder(MessagesEnum messagesEnum, IMessage message, bool getFooter = true)
        {
            var builder = new EmbedBuilder();

            builder.Title = GetTitle(messagesEnum, message);

            builder.Color = GetColor(messagesEnum);

            if (getFooter)
            {
                var footer = new EmbedFooterBuilder();

                footer.IconUrl = _client.CurrentUser.GetAvatarUrl();
                footer.Text = $"Ваш відданий {_client.CurrentUser.Username}";

                builder.Footer = footer;
            }

            return builder;
        }
    }
}
