using Discord;
using Discord.Audio;
using MusicService;
using ServitorDiscordBot.BotCommands.TextCommands;
using System.Text;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MusicPlayerMessageReceivedAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case "!help":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(0xBE5BEF)
                            .WithTitle("Допомога \"музика\"")
                            .WithDescription("Відтворює аудіодоріжку із вказаного аудіо, відео чи плейлисту " +
                                "YouTube, YouTube Music, Soundcloud або ж прямий аудіофайл з іншого ресурсу.\n" +
                                "Перед використанням команд відтворення ви маєте бути під'єднані до голосового каналу, щоби бот знав, куди під'єднуватися.\n" +
                                "Усіма іншими командами ви можете скористатися лише після того, як бот розпочне відтворення.\n" +
                                "\nНаявні наступні команди:\n" +
                                "**!play** ***%URL%*** – під'єднується до голосового каналу та відтворює вказане відео, аудіо чи плейлист.\n" +
                                "**!playdirect** ***%URL%*** – під'єднується до голосового каналу та відтворює аудіо за прямим посиланням у обмеженому режимі.\n" +
                                "**!add** ***%URL%*** – додає до черги відтворення вказане відео, аудіо чи плейлист.\n" +
                                "**!queue** – виводить список черги відтворення.\n" +
                                "\nВідтворення зупиняється автоматично після досягнення кінця черги.\n" +
                                "Також ви можете зупинити відтворення перетягнувши бота у інший канал.\n" +
                                "Зверніть увагу, що бот може відтворювати лише одне відео та перебувати лише у одному голосовому каналі за раз.\n" +
                                "Не підтримуються живі етери YouTube та відео, які вимагають авторизації для підтвердження віку.");

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    break;

                case "!queue":
                    {
                        var queue = _musicPlayer.Queue?.Select((x, i) =>
                        {
                            if (x.isCurrent)
                                return $"\n**{i + 1})** [{x.audio.Duration.GetAudioDuration()}] ***{x.audio.Title}***";
                            return $"\n{i + 1}) [{x.audio.Duration.GetAudioDuration()}] *{x.audio.Title}*";
                        });

                        var sb = new StringBuilder(1950);
                        if (queue is not null)
                            foreach (var q in queue)
                            {
                                if (sb.Length + q.Length > 1950)
                                    break;
                                sb.Append(q);
                            }

                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0x3BA55D))
                            .WithDescription($"У черзі {queue?.Count() ?? 0} аудіо:{sb.ToString()}");

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    break;

                case string c
                when c.StartsWith("!add "):
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0x3BA55D))
                            .WithDescription("Доповнюю список…");

                        await message.Channel.SendMessageAsync(embed: builder.Build());

                        await _musicPlayer.AddAsync(message.Content[5..]);
                    }
                    break;

                case string c
                when c.StartsWith("!play "):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                        {
                            var builder = new EmbedBuilder()
                                .WithColor(new Color(0xFF8C67))
                                .WithDescription("Спершу приєднайтеся до голосового каналу.");

                            await ServiceCommandsManager.SendTemporaryMessageAsync(message, builder);
                        }
                        else if (!_musicPlayer.Init())
                        {
                            var builder = new EmbedBuilder()
                               .WithColor(new Color(0xFF8C67))
                               .WithDescription("Наразі відтворення вже виконується. " +
                               "Дочекайтесь його закінчення, або ж зупиніть його, якщо впевнені, що не перервете прослуховування іншого користувача.");

                            await ServiceCommandsManager.SendTemporaryMessageAsync(message, builder);
                        }
                        else
                        {
                            try
                            {
                                var builder = new EmbedBuilder()
                                    .WithColor(new Color(0x3BA55D))
                                    .WithDescription("Приготування сесії, відтворення незабаром розпочнеться…");
                                var msg = await message.Channel.SendMessageAsync(embed: builder.Build());

                                using (var audioClient = await voiceChannel.ConnectAsync())
                                using (var voiceStream = audioClient.CreatePCMStream(AudioApplication.Music))
                                {
                                    await _musicPlayer.PlayAsync(message.Content[6..], voiceStream, msg.Id);
                                }
                            }
                            finally
                            {
                                await voiceChannel.DisconnectAsync();
                                _musicPlayer.Stop();
                            }
                        }
                    }
                    break;

                case string c
                when c.StartsWith("!playdirect "):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                        {
                            var builder = new EmbedBuilder()
                                .WithColor(new Color(0xFF8C67))
                                .WithDescription("Спершу приєднайтеся до голосового каналу.");

                            await ServiceCommandsManager.SendTemporaryMessageAsync(message, builder);
                        }
                        else if (!_musicPlayer.Init())
                        {
                            var builder = new EmbedBuilder()
                               .WithColor(new Color(0xFF8C67))
                               .WithDescription("Наразі відтворення вже виконується. " +
                               "Дочекайтесь його закінчення, або ж зупиніть його, якщо впевнені, що не перервете прослуховування іншого користувача.");

                            await ServiceCommandsManager.SendTemporaryMessageAsync(message, builder);
                        }
                        else
                        {
                            try
                            {
                                var builder = new EmbedBuilder()
                                    .WithColor(new Color(0x3BA55D))
                                    .WithDescription("Відтворення з прямого аудіопосилання…");
                                var msg = await message.Channel.SendMessageAsync(embed: builder.Build());

                                using (var audioClient = await voiceChannel.ConnectAsync())
                                using (var voiceStream = audioClient.CreatePCMStream(AudioApplication.Music))
                                {
                                    await _musicPlayer.PlayDirectAsync(message.Content[12..], voiceStream);
                                }
                            }
                            finally
                            {
                                await voiceChannel.DisconnectAsync();
                                _musicPlayer.Stop();
                            }
                        }
                    }
                    break;

                default: break;
            }
        }
    }
}