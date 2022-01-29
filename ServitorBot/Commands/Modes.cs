using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetModesAsync(IMessage message)
        {
            /*
            var builder = GetBuilder(MessagesEnum.Modes, message);

            var modes = await getWrapperFactory().GetModesAsync();

            builder.Description = string.Join("\n", modes.Select(x => $"{x.Item1} **{x.Item2[0]}** | {x.Item2[1]}"));

            await message.Channel.SendMessageAsync(embed: builder.Build());
            */
        }
    }
}
