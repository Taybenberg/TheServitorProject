using Database;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task UserIsNotRegisteredAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.NotRegistered);

            builder.Title = "Реєстрація";
            builder.Description = "Я розумію ваш запал, але ж спершу зареєструйтеся!\nКоманда: ***реєстрація***";

            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task UserAlreadyRegisteredAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.AlreadyRegistered);

            builder.Title = "Реєстрація";
            builder.Description = "Ґардіане, ви вже зареєстровані…";

            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task RegisterMessageAsync(IMessage message)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (database.IsDiscordUserRegistered(message.Author.Id))
                await UserAlreadyRegisteredAsync(message.Channel);
            else
            {
                var builder = new EmbedBuilder();

                builder.Color = GetColor(MessagesEnum.Register);

                builder.Title = "Реєстрація";
                builder.Description = $"Добре, давайте ж запишемо вас. Важливо, аби ви були учасником клану **хоча б один день**. " +
                    $"Якщо це так, можемо продовжити.\nВведіть команду ***зареєструватися [ваш нікнейм у Steam]*** (або іншій платформі, з якої ви вступили до клану)\n" +
                    $"Приклад команди: ***зареєструватися {message.Author.Username}***\n" +
                    $"Регістр літер не має значення, можете написати лише фрагмент нікнейму, але він має містити достатню кількіть символів для точної ідентифікації профілю.";

                builder.Footer = GetFooter();

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }

        private async Task TryRegisterUserAsync(IMessage message, string nickname)
        {
            using var scope = _scopeFactory.CreateScope();

            var database = scope.ServiceProvider.GetRequiredService<ClanDatabase>();

            if (!database.IsDiscordUserRegistered(message.Author.Id))
            {
                var builder = new EmbedBuilder();

                builder.Title = "Реєстрація";

                builder.Footer = GetFooter();

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
                await UserAlreadyRegisteredAsync(message.Channel);
        }
    }
}
