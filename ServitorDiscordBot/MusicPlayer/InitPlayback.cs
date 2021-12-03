using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitPlaybackAsync(IMessage message)
        {
            var command = message.Content.ToLower();

            switch (command)
            {
                case string c
                    when c is "допомога":
                    await GetHelpOnCommandAsync(message, "музика");
                    break;

                case string c
                    when c is "stop":
                    {
                        await message.Channel.SendMessageAsync("Зупиняю відтворення…");

                        _player.Stop();
                    }
                    break;

                case string c
                when c.StartsWith("playlist"):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                            await message.Channel.SendMessageAsync("Спершу приєднайтеся до голосового каналу.");
                        else if (_player.TryReserve())
                        {
                            await message.Channel.SendMessageAsync("Буферизую плейлист, відтворення незабаром розпочнеться…");

                            await _player.Playlist(message.Content[9..], voiceChannel);
                        }
                        else
                            await message.Channel.SendMessageAsync("Наразі відтворення вже виконується. Дочекайтесь його закінчення, або ж скористайтесь командою **stop**, якщо впевнені, що не перервете прослуховування іншого користувача.");
                    }
                    break;

                case string c
                when c.StartsWith("play"):
                    {
                        var voiceChannel = (message.Author as IGuildUser).VoiceChannel;

                        if (voiceChannel is null)
                            await message.Channel.SendMessageAsync("Спершу приєднайтеся до голосового каналу.");
                        else if (_player.TryReserve())
                        {
                            await message.Channel.SendMessageAsync("Буферизую відео, відтворення незабаром розпочнеться…");

                            await _player.Play(message.Content[5..], voiceChannel);
                        }
                        else
                            await message.Channel.SendMessageAsync("Наразі відтворення вже виконується. Дочекайтесь його закінчення, або ж скористайтесь командою **stop**, якщо впевнені, що не перервете прослуховування іншого користувача.");
                    }
                    break;
            }
        }
    }
}
