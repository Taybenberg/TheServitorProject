using Discord;
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

        private (DateTime?, string) GetPeriod(string period) =>
            period switch
            {
                "тиждень" => (DateTime.UtcNow.AddDays(-7), " за останній тиждень"),
                "місяць" => (DateTime.UtcNow.AddMonths(-1), " за останній місяць"),
                _ => (null, " за весь час")
            };

        private bool CheckModerationRole(IUser user)
        {
            var sUser = user as SocketGuildUser;

            return sUser.Roles.Any(x =>
            x.Name.ToLower() is "administrator" ||
            x.Name.ToLower() is "moderator" ||
            x.Name.ToLower() is "raid lead") ||
            sUser.Guild.OwnerId == user.Id;
        }

        private string GetActivityCountImpression(int count, string username) =>
            new Random().Next(10) switch
            {
                0 => $"Неймовірно! **{count}** активностей на рахунку {username}! Так тримати!",
                1 => $"Оце так! **{count}** активностей на рахунку {username}! Куди там залізним лордам до вас!",
                2 => $"Надзвичайно! **{count}** активностей на рахунку {username}! Та ви скажені!",
                3 => $"Непосильно! **{count}** активностей на рахунку {username}! Вас варто боятися!",
                4 => $"Парадоксально! **{count}** активностей на рахунку {username}! Марі Сов варто повернутися лишень заради того, щоб побачити вас!",
                5 => $"Фантастика! **{count}** активностей на рахунку {username}! Це ви вбиваєте богів? Тоді я не здивований.",
                6 => $"Немислимо! **{count}** активностей на рахунку {username}! Від вашого світла можна осліпнути!",
                7 => $"Незрівнянно! **{count}** активностей на рахунку {username}! Ви виняткові!",
                8 => $"Непомірно! **{count}** активностей на рахунку {username}! Ви ще не розтрощили якесь небесне тіло? Саме пора!",
                _ => $"Нічого собі! **{count}** активностей на рахунку {username}! Авангард шокований!"
            };

        private async Task<IUserMessage> GetWaitMessageAsync(IMessage message)
        {
            var command = message.Content.ToLower();

            var builder = GetBuilder(MessagesEnum.Wait, null, false);

            builder.Description = new Random().Next(30) switch
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
                11 => $"Ніхто:\nАбсолютно ніхто:\nВи: \"{command}\"",
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

        private async Task LulzChannelManagerAsync(IMessage message)
        {
            await Task.Delay(1000);

            try
            {
                var channel = await _client.Rest.GetChannelAsync(message.Channel.Id) as Discord.Rest.IRestMessageChannel;

                var msg = await channel.GetMessageAsync(message.Id);

                if (msg.Source != MessageSource.User || msg.Attachments.Count > 0 || msg.Embeds.Count > 0)
                    return;

                await msg.DeleteAsync();
            }
            catch { }
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessage, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message);

            await DeleteMessageAsync(wait);
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessage, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message, arg);

            await DeleteMessageAsync(wait);
        }

        private async Task ExecuteWaitMessageAsync(IMessage message, Func<IMessageChannel, Task> method, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message.Channel);

            await DeleteMessageAsync(wait);
        }

        private async Task ExecuteWaitMessageAsync<T>(IMessage message, Func<IMessageChannel, T, Task> method, T arg, bool deleteSenderMessage = false)
        {
            var wait = await GetWaitMessageAsync(message);

            await DeleteMessageAsync(message, deleteSenderMessage);

            await method(message.Channel, arg);

            await DeleteMessageAsync(wait);
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

            await DeleteMessageAsync(message);

            await Task.Delay(5000);

            await DeleteMessageAsync(msg);
        }

        private async Task SendTemporaryMessageAsync(IMessage message, EmbedBuilder builder)
        {
            var msg = await message.Channel.SendMessageAsync(embed: builder.Build());

            await DeleteMessageAsync(message);

            await Task.Delay(5000);

            await DeleteMessageAsync(msg);
        }

        private async Task DeleteMessageAsync(IMessage message, bool confirmation = true)
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

        private EmbedBuilder GetBuilder(MessagesEnum messagesEnum, IMessage message, bool getFooter = true, string userName = null)
        {
            var builder = new EmbedBuilder();

            builder.Title = GetTitle(messagesEnum, message, userName);

            builder.Color = GetColor(messagesEnum);

            if (getFooter)
            {
                var footer = new EmbedFooterBuilder();

                footer.IconUrl = _client.CurrentUser.GetAvatarUrl();
                footer.Text = $"Ваш відданий {_client.CurrentUser.Username} | !donate – підтримати автора";

                builder.Footer = footer;
            }

            return builder;
        }

        private async Task SendDonateMessageAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = Color.Gold;

            builder.ThumbnailUrl = _client.GetUser(228896926991515649).GetAvatarUrl();

            builder.Title = "Підтримати автора";

            builder.Description = $"Ви завжди можете підтримати <@228896926991515649> чашкою кави на сервісі [Buy Me a Coffee](https://www.buymeacoffee.com/servitor).";

            await channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
