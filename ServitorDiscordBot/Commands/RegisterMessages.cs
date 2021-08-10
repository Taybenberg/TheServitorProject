using Discord;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task UserIsNotRegisteredAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.NotRegistered, message);

            builder.Description = "Ґардіане, спершу вас необідно ідентифікувати у базі даних Авангарду.\n" +
                "Це здійснюється шляхом виконання процедури реєстрації.\nДля цього скористайтеся командою **реєстрація**";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task UserAlreadyRegisteredAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.AlreadyRegistered, message);

            builder.Description = "Ґардіане, ви вже зареєстровані, а отже перед вами розкрито весь потенціал моїх обчислювальних потужностей.\n" +
                "Переглянути список моїх команд можна за допомогою команди **допомога**.";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task TryRegisterUserAsync(IMessage message, string nickname = null)
        {
            var wrapper = getWrapperFactory();

            var similarUsers = await wrapper.GetUserWithSimilarUserNameAsync(message.Author.Id, nickname ?? message.Author.Username);

            if (similarUsers.UserRegistered)
                await UserAlreadyRegisteredAsync(message);
            else
            {
                var builder = GetBuilder(MessagesEnum.Register, message);

                var userSimilarity = similarUsers.UserSimilarities.FirstOrDefault();

                if ((nickname is not null || nickname?.Length == 0) && !similarUsers.UserSimilarities.Any())
                {
                    builder.Color = GetColor(MessagesEnum.RegisterNeedMoreInfo);

                    builder.Description = "Не вдалося знайти користувача. Уточніть, будь ласка, нікнейм тієї платформи, з якої ви вступали до клану.\n" +
                        "Потім введіть цей нікнейм у команді **зареєструватися %нікнейм%**";
                }
                else if ((nickname is not null || nickname?.Length == 0) && similarUsers.UserSimilarities.Count() > 1)
                {
                    builder.Color = GetColor(MessagesEnum.RegisterNeedMoreInfo);

                    builder.Description = $"Уточніть, будь ласка, нікнейм, бо за цим шаблоном знайдено кілька гравців: " +
                        $"{string.Join(", ", similarUsers.UserSimilarities.Select(x => x.UserName))}";
                }
                else if (userSimilarity is not null)
                {
                    if (await wrapper.RegisterUserAsync(userSimilarity.UserId, message.Author.Id))
                    {
                        builder.Color = GetColor(MessagesEnum.RegisterSuccessful);

                        builder.Description = $"Зареєстровано {message.Author.Mention} як гравця платформи {userSimilarity.MembershipType} **{userSimilarity.UserName}**";
                    }
                    else
                    {
                        builder.Color = GetColor(MessagesEnum.Error);

                        builder.Description = $"Сталася помилка під час реєстрації. Спробуйте пізніше.";
                    }
                }
                else
                {
                    builder.Description = $"Добре, давайте ж запишемо вас. Важливо, аби ви були учасником клану **хоча б один день**.\n" +
                        $"Введіть команду **зареєструватися %ваш нікнейм у Steam%** (або іншій платформі, з якої ви вступили до клану)\n" +
                        $"Приклад: **зареєструватися {message.Author.Username}**\n" +
                        $"Якщо не пам'ятаєте нікнейм повністю, не проблема, я можу знайти його за частковим збігом.";
                }

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }
    }
}
