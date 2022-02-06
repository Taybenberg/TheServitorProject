using Discord;
using MusicService;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task OnMusicPlayerUpdateAsync(ulong? messageID)
        {
            if (messageID is null)
                return;

            IMessageChannel channel = _client.GetChannel(_musicChannelIDs[0]) as IMessageChannel;

            var queue = _musicPlayer.Queue.SkipWhile(x => !x.isCurrent).Take(4).Select(x => x.audio);

            var first = queue.FirstOrDefault();
            if (first is null)
                return;

            var thumbnail = first.CoverURL;

            var fields = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Зараз відтворюється",
                    Value = $"[{first.Duration.GetAudioDuration()}] *{first.Title}*",
                }
            };

            var next = queue.Skip(1);
            if (next.Count() > 0)
            {
                fields.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Далі у черзі",
                    Value = $"{string.Join('\n', next.Select(x => $"[{x.Duration.GetAudioDuration()}] *{x.Title}*"))}"
                });
            }

            (var custId, var emoji) = _musicPlayer.IsPlaying ?
                ("MusicPlayerPause", Emote.Parse(CommonData.DiscordEmoji.Emoji.MusicPause)) :
                ("MusicPlayerContinue", Emote.Parse(CommonData.DiscordEmoji.Emoji.MusicContinue));

            var builder = new EmbedBuilder()
                .WithColor(0x3BA55D)
                .WithThumbnailUrl(thumbnail)
                .WithFields(fields);

            var component = new ComponentBuilder()
                .WithButton(customId: "MusicPlayerShuffle", style: ButtonStyle.Success, emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.MusicShuffle))
                .WithButton(customId: "MusicPlayerPrevious", style: ButtonStyle.Success, emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.MusicPrevious))
                .WithButton(customId: custId, style: ButtonStyle.Success, emote: emoji)
                .WithButton(customId: "MusicPlayerNext", style: ButtonStyle.Success, emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.MusicNext))
                .WithButton(customId: "MusicPlayerStop", style: ButtonStyle.Success, emote: Emote.Parse(CommonData.DiscordEmoji.Emoji.MusicStop));

            try
            {
                await channel.ModifyMessageAsync(messageID.Value, msg =>
                {
                    msg.Embed = builder.Build();
                    msg.Components = component.Build();
                });
            }
            catch { }
        }
    }
}
