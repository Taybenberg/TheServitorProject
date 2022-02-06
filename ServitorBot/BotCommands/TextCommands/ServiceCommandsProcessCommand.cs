using Discord;

namespace ServitorDiscordBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        public async Task<bool> ProcessServiceCommandAsync(IMessage message)
        {
            switch (message.Content.ToLower())
            {
                case "!secret_help":
                    await message.Channel.SendMessageAsync(embeds: HelpEmbeds);
                    return true;

                case "!donate":
                    await message.Channel.SendMessageAsync(embed: DonateEmbedBuilder.Build());
                    return true;

                case string c when c.StartsWith("!random"):
                    await CommandRandom(message);
                    return true;
            }

            return false;
        }
    }
}
