using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetMyPartnersAsync(IMessage message, string period)
        {
            /*
            (var date, var title) = GetPeriod(period);

            var partners = await getWrapperFactory().GetMyPartnersAsync(message.Author.Id, date);

            if (!partners.IsUserRegistered)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var builder = GetBuilder(MessagesEnum.MyPartners, message, userName: partners.UserName);

            builder.Title += title;

            builder.ThumbnailUrl = message.Author.GetAvatarUrl();

            if (!partners.Partners.Any())
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Не можу знайти інформацію про ваші активності. Можливо ви новачок у клані, або ще ні з ким не грали за вказаний період.";
            }
            else
            {
                builder.Description = $"Всього кооперативних активностей: **{partners.CoopCount}/{partners.AllCount} ({Math.Round(partners.CoopCount * 100.0 / partners.AllCount, 2)}%)**\n" +
                    $"\n{string.Join("\n", partners.Partners.Select(x => $"**{x.UserName}** – **{x.Count}**"))}";

                builder.ImageUrl = partners.QuickChartURL;
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
            */
        }
    }
}
