using Discord;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetHelpOnCommandAsync(IMessage message, string command)
        {
            var builder = GetBuilder(MessagesEnum.Help, message);

            builder.Description = $"Тут буде довідка на команду {command}";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
