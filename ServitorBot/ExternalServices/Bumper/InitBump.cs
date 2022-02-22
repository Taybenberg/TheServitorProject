using Discord;
using System.Text.RegularExpressions;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private async Task InitBumpAsync(IMessage message)
        {
            var embed = message.Embeds.FirstOrDefault();

            if (embed is not null)
            {
                if (embed.Description?.Contains("Server bumped by") ?? false)
                {
                    var mention = Regex.Match(embed.Description, "(?<=\\<@)\\D?(\\d+)(?=\\>)").Groups[1].Value;

                    var nextBump = await _bumper.RegisterBumpAsync(ulong.Parse(mention));

                    var builder = new EmbedBuilder()
                        .WithColor(0xFF6E00)              
                        .WithDescription($"{CommonData.DiscordEmoji.Emoji.BumpTimer} {TimestampTag.FromDateTime(nextBump, TimestampTagStyles.ShortTime)}");

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
            }
        }
    }
}
