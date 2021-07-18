using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetMyPartnersAsync(IMessage message)
        {
            var activities = await getStatsFactory().GetMyPartnersAsync(message.Author.Id);

            if (!activities.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyPartners, message);

            builder.ThumbnailUrl = message.Author.GetAvatarUrl();

            if (!activities.Partners.Any())
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали цього року.";
            }
            else
                builder.Description = string.Join("\n", activities.Partners.Select(x => $"**{x.UserName}** – **{x.Count}**"));

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
