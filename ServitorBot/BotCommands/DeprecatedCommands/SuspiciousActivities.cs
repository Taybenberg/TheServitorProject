using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetSuspiciousActivitiesAsync(IMessage message, bool nigthfalls)
        {
            /*
            var builder = GetBuilder(MessagesEnum.Suspicious, message, false);

            var acts = await getWrapperFactory().GetSuspiciousActivitiesAsync(nigthfalls, false);

            string sus = string.Empty;

            foreach (var act in acts.Activities)
            {
                var s = $"**{act.Period}, {act.Type} {act.Score}**\n" + string
                    .Join("\n", act.Users
                    .Select(y => y.IsClanMember ?
                    $"**{y.UserName} [{y.ClanSign}]**" :
                    $"{y.UserName} [{y.ClanSign}] {y.ClanName}"));

                if ((sus + s).Length > 2000)
                    break;

                sus += $"{s}\n\n";
            }

            builder.Description = $"||{sus}||";

            await message.Channel.SendMessageAsync(embed: builder.Build());
            */
        }
    }
}
