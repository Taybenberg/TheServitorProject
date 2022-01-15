using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task BipAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.Bip, null, false);

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
