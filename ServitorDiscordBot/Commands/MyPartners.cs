using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetMyPartnersAsync(IMessage message)
        {
            var partners = await getWrapperFactory().GetMyPartnersAsync(message.Author.Id);

            if (!partners.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyPartners, message, userName: partners.UserName);

            builder.ThumbnailUrl = message.Author.GetAvatarUrl();

            if (!partners.Partners.Any())
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали цього року.";
            }
            else
            {
                builder.Description = string.Join("\n", partners.Partners.Select(x => $"**{x.UserName}** – **{x.Count}**"));

                builder.ImageUrl = partners.QuickChartURL;
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
