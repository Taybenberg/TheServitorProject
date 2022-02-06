using Discord;

namespace ServitorBot.BotCommands.TextCommands
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
                    await CommandRandomAsync(message);
                    return true;

                case string c when c.StartsWith("!delete_"):
                    {
                        await DeleteMessageAsync(message);

                        if (!CheckDeleteMessagesPermission(message.Author))
                            await SendTemporaryMessageAsync(message, NoPermissonForDeletionEmbedBuilder);
                        else
                            await CommandDeleteAsync(message);                    
                    }
                    return true;
            }

            return false;
        }
    }
}
