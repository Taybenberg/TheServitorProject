using BumperService;
using CommonData.DiscordEmoji;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
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
                        .WithDescription($"{CommonData.DiscordEmoji.Emoji.BumpTimer} {nextBump.ToString("HH:mm")}");

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
            }
        }
    }
}
