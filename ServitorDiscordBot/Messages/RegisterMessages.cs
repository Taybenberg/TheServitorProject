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

            builder.Description = "Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task UserAlreadyRegisteredAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.AlreadyRegistered, message);

            builder.Description = "Ґардіане, ви вже зареєстровані…";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task RegisterMessageAsync(IMessage message)
        {
            var database = getDatabase();

            if (database.IsDiscordUserRegistered(message.Author.Id))
                await UserAlreadyRegisteredAsync(message);
            else
            {
                var builder = GetBuilder(MessagesEnum.Register, message);

                builder.Description = $"Добре, давайте ж запишемо вас. Важливо, аби ви були учасником клану **хоча б один день**. " +
                    $"Якщо це так, можемо продовжити.\nВведіть команду ***зареєструватися [ваш нікнейм у Steam]*** (або іншій платформі, з якої ви вступили до клану)\n" +
                    $"Приклад команди: ***зареєструватися {message.Author.Username}***\n" +
                    $"Регістр літер не має значення, можете написати лише фрагмент нікнейму, але він має містити достатню кількіть символів для точної ідентифікації профілю.";

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }

        private async Task TryRegisterUserAsync(IMessage message, string nickname)
        {
            var database = getDatabase();

            if (!database.IsDiscordUserRegistered(message.Author.Id))
            {
                var builder = GetBuilder(MessagesEnum.Register, message);

                if (nickname.Length > 0)
                {
                    var users = (await database.GetUsersByUserNameAsync(nickname)).Where(x => x.DiscordUserID is null);

                    if (users.Count() < 1)
                    {
                        builder.Color = GetColor(MessagesEnum.RegisterNeedMoreInfo);

                        builder.Description = "Не можу знайти гравця. Перевірте запит.";
                    }
                    else if (users.Count() > 1)
                    {
                        builder.Color = GetColor(MessagesEnum.RegisterNeedMoreInfo);

                        builder.Description = $"Уточніть нікнейм, бо за цим шаблоном знайдено кілька гравців: {string.Join(", ", users.Select(x => x.UserName))}";
                    }
                    else
                    {
                        var user = users.First();

                        await database.RegisterUserAsync(user.UserID, message.Author.Id);

                        builder.Color = GetColor(MessagesEnum.RegisterSuccessful);

                        builder.Description = $"Зареєстровано {message.Author.Mention} як гравця {user.UserName}";
                    }
                }
                else
                {
                    builder.Color = GetColor(MessagesEnum.RegisterNeedMoreInfo);

                    builder.Description = "Ви не написати свій нікнейм, повторіть команду, тільки цього разу допишіть свій нікнейм.";
                }

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
            else
                await UserAlreadyRegisteredAsync(message);
        }
    }
}
