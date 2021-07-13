using DataProcessor.Localization;
using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetLeaderboardAsync(IMessage message, string mode)
        {
            var database = getDatabase();

            var currUser = await database.GetUserActivitiesAsync(message.Author.Id);

            if (currUser is null)
            {
                await UserIsNotRegisteredAsync(message);

                return;
            }

            var pair = TranslationDictionaries.StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == mode));

            var builder = GetBuilder(MessagesEnum.Leaderboard, message);

            if (pair.Value is not null)
            {
                var apiClient = getApiClient();

                builder.Title += $" | { pair.Value[0]}";

                var leaderboard = await apiClient.Clan.GetClanLeaderboardAsync(pair.Key, TranslationDictionaries.StatNames.Keys.ToArray());

                if (leaderboard.Any())
                {
                    builder.Fields = new();

                    var users = await database.GetUsersAsync();

                    foreach (var entry in leaderboard)
                    {
                        if (entry.Leaders.Count() == 0)
                            continue;

                        string usrs = string.Empty;

                        bool userFound = false;

                        foreach (var user in entry.Leaders.Take(3))
                        {
                            var u = users.FirstOrDefault(x => x.UserID == user.UserID);

                            if (u is null)
                                continue;

                            if (u.UserID == currUser.UserID)
                            {
                                usrs += $"***{user.Rank}, {u.UserName}, {TranslationDictionaries.ClassNames[user.Class]}, {user.Value}***\n";

                                userFound = true;
                            }
                            else
                                usrs += $"{user.Rank}, {u.UserName}, {TranslationDictionaries.ClassNames[user.Class]}, {user.Value}\n";
                        }

                        if (!userFound)
                        {
                            var u = entry.Leaders.FirstOrDefault(x => x.UserID == currUser.UserID);

                            if (!u.Equals(default))
                                usrs += $"***{u.Rank}, {currUser.UserName}, {TranslationDictionaries.ClassNames[u.Class]}, {u.Value}***\n";
                        }

                        builder.Fields.Add(new EmbedFieldBuilder
                        {
                            Name = TranslationDictionaries.StatNames[entry.Stat],
                            Value = usrs,
                            IsInline = false
                        });
                    }
                }
                else
                {
                    builder.Color = GetColor(MessagesEnum.Error);

                    builder.Description = "Сталася помилка при обробці вашого запиту сервером Bungie.net. Спробуйте пізніше.";
                }
            }
            else
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = "Сталася помилка при обробці вашого запиту, переконайтеся, що ви правильно вказали тип активності.\nДля цього введіть команду ***режими***.";
            }

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
