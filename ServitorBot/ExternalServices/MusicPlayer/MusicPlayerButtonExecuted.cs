using Discord;
using Discord.WebSocket;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task MusicPlayerButtonExecutedAsync(SocketMessageComponent component)
        {
            var userVoiceChannel = (component.User as IGuildUser).VoiceChannel;
            if (userVoiceChannel is null)
                return;

            var botUser = await userVoiceChannel.GetUserAsync(_client.CurrentUser.Id);
            if (botUser is null)
                return;

            switch (component.Data.CustomId)
            {
                case "MusicPlayerShuffle":
                    _musicPlayer.Shuffle();
                    await component.DeferAsync();
                    break;

                case "MusicPlayerPrevious":
                    _musicPlayer.Previous();
                    await component.DeferAsync();
                    break;

                case "MusicPlayerContinue":
                    _musicPlayer.Continue();
                    await component.DeferAsync();
                    break;

                case "MusicPlayerPause":
                    _musicPlayer.Pause();
                    await component.DeferAsync();
                    break;

                case "MusicPlayerStop":
                    _musicPlayer.Stop();
                    await component.DeferAsync();
                    break;

                case "MusicPlayerNext":
                    _musicPlayer.Next();
                    await component.DeferAsync();
                    break;

                default: break;
            }
        }
    }
}
