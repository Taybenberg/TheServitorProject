using Discord;

namespace ServitorBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        public async Task LulzChannelProcessMessageAsync(IMessage message)
        {
            await Task.Delay(1000);

            try
            {
                var channel = await _client.Rest.GetChannelAsync(message.Channel.Id) as Discord.Rest.IRestMessageChannel;

                var msg = await channel.GetMessageAsync(message.Id);

                if (msg.Source != MessageSource.User || msg.Attachments.Count > 0 || msg.Embeds.Count > 0)
                    return;

                await msg.DeleteAsync();
            }
            catch { }
        }
    }
}
