using ActivityService;
using CommonData.Localization;
using Discord.WebSocket;
using System.Globalization;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        private ActivityContainer TryParseActivityContainer(SocketSlashCommand command)
        {
            try
            {
                var options = command.Data.Options;

                var modeOption = options.First(x => x.Name is "режим");
                var activityType = Translation.GetActivityType(((string)modeOption.Value).ToLower());

                var dateOption = options.First(x => x.Name is "дата");
                DateTime plannedDate = DateTime.ParseExact((string)dateOption.Value, "d.M-H:m", CultureInfo.CurrentCulture);

                var nameOption = options.FirstOrDefault(x => x.Name is "назва");
                var activityName = nameOption?.Value.ToString();

                var descriptionOption = options.FirstOrDefault(x => x.Name is "опис");
                var description = descriptionOption?.Value.ToString();

                var users = new ulong[] { command.User.Id };

                return new ActivityContainer
                {
                    ChannelID = command.Channel.Id,
                    ActivityType = activityType,
                    ActivityName = activityName,
                    Description = description,
                    PlannedDate = plannedDate,
                    Users = users
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
