using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetMyPartnersAsync(IMessage message)
        {
            var database = getDatabase();

            if (!database.IsDiscordUserRegistered(message.Author.Id))
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var partners = await database.GetUserPartnersAsync(message.Author.Id);

            var builder = GetBuilder(MessagesEnum.MyPartners, message);

            if (!partners.Any())
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Я не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали у цьому році.";
            }
            else
            {
                builder.Description = string.Empty;

                foreach (var p in partners)
                    builder.Description += $"**{p.UserName}** – ***{p.Count}***\n";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
