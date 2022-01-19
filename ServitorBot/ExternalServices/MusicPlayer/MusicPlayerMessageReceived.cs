using Discord;
using Discord.Audio;
using MusicService;
using System.Text;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MusicPlayerMessageReceivedAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case "допомога":
                    await GetHelpOnCommandAsync(message, "музика");
                    break;

                case "next":
                    _musicPlayer.Next();
                    break;

                case "prev":
                    _musicPlayer.Previous();
                    break;

                case "pause":
                    _musicPlayer.Pause();
                    break;

                case "continue" or "resume":
                    _musicPlayer.Continue();
                    break;

                case "stop":
                    _musicPlayer.Stop();
                    break;

                case "shuffle":
                    _musicPlayer.Shuffle();
                    break;

                case "queue":
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
                when c.StartsWith("add "):
                    {
                        await SendTemporaryMessageAsync(message, "Доповнюю список…");
                        await _musicPlayer.AddAsync(message.Content[4..]);
                    }
                    break;

                case string c
                when c.StartsWith("play "):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                            await SendTemporaryMessageAsync(message, "Спершу приєднайтеся до голосового каналу.");
                        else if (!_musicPlayer.Init())
                            await SendTemporaryMessageAsync(message, "Наразі відтворення вже виконується. Дочекайтесь його закінчення, або ж скористайтесь командою **stop**, якщо впевнені, що не перервете прослуховування іншого користувача.");
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
                                    await _musicPlayer.PlayAsync(message.Content[5..], voiceStream, msg.Id);
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
                when c.StartsWith("playdirect "):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                            await SendTemporaryMessageAsync(message, "Спершу приєднайтеся до голосового каналу.");
                        else if (!_musicPlayer.Init())
                            await SendTemporaryMessageAsync(message, "Наразі відтворення вже виконується. Дочекайтесь його закінчення, або ж скористайтесь командою **stop**, якщо впевнені, що не перервете прослуховування іншого користувача.");
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
                                    await _musicPlayer.PlayDirectAsync(message.Content[11..], voiceStream);
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