using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitPlaybackAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case string c
                when c is "допомога":
                    await GetHelpOnCommandAsync(message, "музика");
                    break;

                case string c
                when c is "next":
                    _player.Next();
                    break;

                case string c
                when c is "prev":
                    _player.Prev();
                    break;

                case string c
                when c is "queue":
                    await _player.GetQueueAsync(message.Channel);
                    break;

                case string c
                when c is "pause":
                    {
                        await message.Channel.SendMessageAsync("Призупиняю відтворення…");

                        _player.Pause();
                    }
                    break;

                case string c
                when c is "continue":
                    {
                        await message.Channel.SendMessageAsync("Продовжую відтворення…");

                        _player.Continue();
                    }
                    break;

                case string c
                when c is "stop":
                    {
                        await message.Channel.SendMessageAsync("Зупиняю відтворення…");

                        _player.Stop();
                    }
                    break;

                case string c
                when c is "shuffle":
                    {
                        await message.Channel.SendMessageAsync("Перемішую відео…");

                        _player.Shuffle();
                    }
                    break;

                case string c
                when c.StartsWith("add"):
                    {
                        await message.Channel.SendMessageAsync("Доповнюю список…");

                        await _player.AddAsync(message.Content[4..]);
                    }
                    break;

                case string c
                when c.StartsWith("play"):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                            await message.Channel.SendMessageAsync("Спершу приєднайтеся до голосового каналу.");
                        else if (_player.TryReserve())
                            await _player.PlayAsync(message.Content[5..], voiceChannel, message.Channel);
                        else
                            await message.Channel.SendMessageAsync("Наразі відтворення вже виконується. Дочекайтесь його закінчення, або ж скористайтесь командою **stop**, якщо впевнені, що не перервете прослуховування іншого користувача.");
                    }
                    break;
            }
        }
    }
}
