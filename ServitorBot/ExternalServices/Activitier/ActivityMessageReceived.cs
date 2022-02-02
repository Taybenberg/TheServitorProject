using CommonData.Activities;
using CommonData.Localization;
using Discord;
using System.Globalization;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task ActivityMessageReceivedAsync(IMessage message)
        {
            var command = message.Content.ToLower();

            switch (command)
            {
                case "!швидка активність":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xFFFFFF))
                            .WithDescription("Швидко зберіться у активність на найближчий час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть активність")
                            .WithCustomId("QuickActivitySelector")
                            .WithMinValues(1).WithMaxValues(1)
                            .WithOptions(Activity.QuickActivityTypes.Select(x =>
                                new SelectMenuOptionBuilder
                                {
                                    Label = Translation.StatsActivityNames[x][0],
                                    Value = $"QuickActivity_{x}",
                                    Emote = Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityEmoji(x))
                                }).ToList());

                        var component = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await message.Channel.SendMessageAsync(embed: builder.Build(), components: component.Build());

                        await DeleteMessageAsync(message);
                    }
                    break;

                case "!швидкий рейд":
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(new Color(0xFFFFFF))
                            .WithDescription("Швидко зберіть рейд на найближчий час");

                        var menuBuilder = new SelectMenuBuilder()
                            .WithPlaceholder("Оберіть рейд")
                            .WithCustomId("QuickActivitySelector")
                            .WithMinValues(1).WithMaxValues(1)
                            .WithOptions(Activity.ActivityRaidTypes.Select(x =>
                                new SelectMenuOptionBuilder
                                {
                                    Label = Translation.ActivityRaidTypes[x],
                                    Value = $"QuickRaid_{x}",
                                    Emote = Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(x))
                                }).ToList());

                        var component = new ComponentBuilder()
                            .WithSelectMenu(menuBuilder);

                        await message.Channel.SendMessageAsync(embed: builder.Build(), components: component.Build());

                        await DeleteMessageAsync(message);
                    }
                    break;

                case "допомога":
                    await GetHelpOnCommandAsync(message, "рейд");
                    break;

                case "скасувати":
                    {
                        var msgId = message?.Reference?.MessageId.Value;
                        if (msgId is not null)
                        {
                            await _activityManager.DisableActivityAsync(msgId.Value, message.Author.Id);
                            await DeleteMessageAsync(message);
                        }
                    }
                    break;

                case string c
                when c.StartsWith("передати"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;
                        if (msgId is not null && message.MentionedUserIds.Count == 2)
                        {
                            var receiverID = message.MentionedUserIds.Last();
                            await _activityManager.UserTransferPlaceAsync(msgId.Value, message.Author.Id, receiverID);
                            await DeleteMessageAsync(message);
                        }
                    }
                    break;

                case string c
                when c.StartsWith("перенести"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;
                        if (msgId is not null)
                        {
                            try
                            {
                                var date = DateTime.ParseExact(command.Replace("перенести ", string.Empty), "d.M-H:m", CultureInfo.CurrentCulture);
                                if (date < DateTime.Now)
                                    date = date.AddYears(1);

                                await _activityManager.RescheduleActivityAsync(msgId.Value, message.Author.Id, date.ToUniversalTime());
                                await DeleteMessageAsync(message);
                            }
                            catch { }
                        }
                    }
                    break;

                case string c
                when c.StartsWith("зарезервувати"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;
                        if (msgId is not null)
                        {
                            await _activityManager.UsersSubscribeAsync(msgId.Value, message.Author.Id, message.MentionedUserIds.Skip(1));
                            await DeleteMessageAsync(message);
                        }
                    }
                    break;

                case string c
                when c.StartsWith("виключити"):
                    {
                        var msgId = message?.Reference?.MessageId.Value;
                        if (msgId is not null)
                        {
                            await _activityManager.UsersUnSubscribeAsync(msgId.Value, message.Author.Id, message.MentionedUserIds.Skip(1));
                            await DeleteMessageAsync(message);
                        }
                    }
                    break;

                case string c
                when c.StartsWith("змінити "):
                    {
                        var msgId = message?.Reference?.MessageId.Value;
                        if (msgId is null)
                            return;

                        try
                        {
                            var action = message.Content[8..];
                            switch (action.ToLower())
                            {
                                case string s
                                when s.StartsWith("опис "):
                                    {
                                        var activity = await _activityManager.GetActivityAsync(msgId.Value);
                                        if (activity is not null)
                                        {
                                            activity.Description = action[5..];
                                            await _activityManager.UpdateActivityAsync(activity, message.Author.Id);
                                        }
                                    }
                                    break;

                                case string s
                                when s.StartsWith("заголовок "):
                                    {
                                        var activity = await _activityManager.GetActivityAsync(msgId.Value);
                                        if (activity is not null)
                                        {
                                            activity.ActivityName = action[10..];
                                            await _activityManager.UpdateActivityAsync(activity, message.Author.Id);
                                        }
                                    }
                                    break;

                                case string s
                                when s.StartsWith("активність "):
                                    {
                                        var activity = await _activityManager.GetActivityAsync(msgId.Value);
                                        if (activity is not null)
                                        {
                                            activity.ActivityType = Translation.GetActivityType(action[11..].ToLower());
                                            await _activityManager.UpdateActivityAsync(activity, message.Author.Id);
                                        }
                                    }
                                    break;

                                default: break;
                            }
                            await DeleteMessageAsync(message);
                        }
                        catch { }
                    }
                    break;

                case string c
                when c.StartsWith('!'):
                    {
                        var container = TryParseActivityContainer(message);

                        if (container is not null)
                        {
                            await InitActivityAsync(container);
                            await DeleteMessageAsync(message);
                        }
                        else
                        {
                            var builder = new EmbedBuilder()
                                .WithColor(new Color(0xD50000))
                                .WithTitle("Збір у активність")
                                .WithDescription($"Сталася помилка під час створення активності. Перевірте, чи формат команди коректний.\nЩоби переглянути довідку, скористайтеся командою **допомога**.");

                            await SendTemporaryMessageAsync(message, builder);
                        }
                    }
                    break;

                default: break;
            }
        }
    }
}