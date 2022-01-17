using Discord.WebSocket;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task MusicPlayerButtonExecutedAsync(SocketMessageComponent component)
        {
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
