using Discord;
using Microsoft.Extensions.Logging;
using System;
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
                    _logger.LogInformation($"{DateTime.Now} Server bumped");

                    var mention = Regex.Match(embed.Description, "(?<=\\<@)\\D?(\\d+)(?=\\>)").Groups[1].Value;

                    _bumper.AddUser(mention);

                    var builder = GetBuilder(MessagesEnum.Bumped, message, false);

                    builder.Description = $":alarm_clock: :ok_hand:\n:fast_forward: {_bumper.NextBump.ToString("HH:mm:ss")}";

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
            }
        }
    }
}
