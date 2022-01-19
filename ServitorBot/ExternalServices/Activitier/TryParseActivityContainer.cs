﻿using ActivityService;
using CommonData.Localization;
using Discord;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private ActivityContainer TryParseActivityContainer(IMessage message)
        {
            try
            {
                string activityName = null, description = null;
                DateTime plannedDate;

                var command = message.Content;

                var users = new ulong[] { message.Author.Id }
                    .Concat(message.MentionedUserIds);

                var index = command.IndexOf(' ');
                var activityType = Translation.GetActivityType(command[1..index].Replace('_', ' ').ToLower());
                command = command[index..].TrimStart();

                index = command.IndexOf(' ');
                if (index < 0)
                    plannedDate = DateTime.ParseExact(command, "d.M-H:m", CultureInfo.CurrentCulture);
                else
                {
                    plannedDate = DateTime.ParseExact(command[..index], "d.M-H:m", CultureInfo.CurrentCulture);

                    command = command[index..].TrimStart();
                    command = Regex.Replace(command, "<@\\D?\\d+>", string.Empty).TrimStart();

                    index = command.IndexOf(' ');

                    if (index < 0 && command.Length > 0)
                        activityName = command.Replace('_', ' ');
                    if (index > 0)
                    {
                        activityName = command[..index].Replace('_', ' ');
                        description = command[index..].TrimStart();
                    }
                }

                if (plannedDate < DateTime.Now)
                    plannedDate = plannedDate.AddYears(1);

                return new ActivityContainer
                {
                    ChannelID = message.Channel.Id,
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
