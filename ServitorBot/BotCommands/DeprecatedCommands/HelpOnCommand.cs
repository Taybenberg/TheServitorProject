using CommonData.Activities;
using CommonData.Localization;
using Discord;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetHelpOnCommandAsync(IMessage message, string command)
        {
            var builder = GetBuilder(MessagesEnum.Help, message);

            switch (command)
            {
                case string c
                when messageCommands[MessagesEnum.Weekly]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Weekly][0]}** " +
                            $"дозволяє переглянути тижневу ротацію найтфолу та горнила, а також визначає, чи доступний цього тижня Залізний стяг.\n" +
                            $"Вміст цього повідомлення є частиною інформаційної картки про тижневий ресет (дизайнер картки – <@356816080326361088>), " +
                            $"яка надсилається автоматично щовівторка на початку кожного тижня у Destiny 2.\n" +
                            $"Якщо в момент виконання команди сервери Destiny не працюють, то результат команди не буде отримано.\n" +
                            $"Наявність Залізного стягу перевіряється на ресурсі https://www.light.gg";

                        builder.ImageUrl = "https://i.imgur.com/iZ70dgo.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Resources]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Resources][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки – <@356816080326361088>) " +
                            $"з відомостями про асортимент ресурсів та модів Ади-1, Банші-44 та Павука цього денного ресету.\n" +
                            $"Вміст цієї картки є частиною інформаційної картки про денний ресет (дизайнер картки – <@356816080326361088>), " +
                            $"яка надсилається автоматично на початку кожного дня у Destiny 2.\n" +
                            $"Інформація підтягується з ресурсу https://www.todayindestiny.com/vendors";

                        builder.ImageUrl = "https://i.imgur.com/LroPcmY.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Eververse]
                .Any(x => c.StartsWith(x)):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Eververse][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки – <@679220982082174977>) " +
                            $"з відомостями про асортимент Тесс Еверіс за визначений тиждень.\n" +
                            $"\nМожливі 3 варіанти команди:\n" +
                            $"**{messageCommands[MessagesEnum.Eververse][0]}** без параметрів – виводить асортимент поточного тижня\n" +
                            $"**{messageCommands[MessagesEnum.Eververse][0]}** зі статичним параметром ***все*** – виводить асортимент за всі тижні сезону\n" +
                            $"**{messageCommands[MessagesEnum.Eververse][0]}** з динамічним параметром ***№ тижня*** – " +
                            $"виводить асортимент за вказаний тиждень сезону, потрібно прописати номер тижня\n" +
                            $"\nЯкщо вказано параметр, формат якого не відповідає вказаним вище (або номер тижня виходить за діапазон сезону), " +
                            $"то буде виведено асортимент поточного тижня.\n" +
                            $"Вміст цієї картки є частиною інформаційної картки про тижневий ресет (дизайнер картки – <@356816080326361088>), " +
                            $"яка надсилається автоматично щовівторка на початку кожного тижня у Destiny 2.\n" +
                            $"Можливе виведення хибної інформації на початку сезону, допоки не сформовано таблицю сезонного лутпулу.\n" +
                            $"Інформація підтягується з ресурсу https://www.todayindestiny.com/eververseCalendar";

                        builder.ImageUrl = "https://i.imgur.com/V78rDXR.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.MyGrandmasters]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.MyGrandmasters][0]}** " +
                            $"виводить список закритих вами грандмайстрів у поточному сезоні та за весь час.\n" +
                            $"Враховуються лише ті грандмайстри, які ви закрили перебуваючи учасником клану (але не раніше 01.01.2021).\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.";

                        builder.ImageUrl = "https://i.imgur.com/6lkU3Ev.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.MyRaids]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.MyRaids][0]}** " +
                            $"виводить список закритих вами рейдів цього тижня на різних персонажах.\n" +
                            $"Враховуються лише ті рейди, які ви закрили перебуваючи учасником клану.\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.";

                        builder.ImageUrl = "https://i.imgur.com/PNVWxNW.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case "рандом" or "random":
                    {
                        builder.Title = "Рандом";

                        builder.Description = "Наявні 2 типи команд:\n" +
                            "**!random** ***N*** – генерує випадкове ціле число X в діапазоні 0 <= X < N.\n" +
                            "**!random** ***@Role*** – обирає випадкового користувача зі вказаною роллю.";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case "адмін" or "admin":
                    {
                        builder.Title = "Видалення повідомлень";

                        builder.ImageUrl = "https://i.imgur.com/VYzsWPK.png";

                        builder.Description = "Видалення повідомлення можливе за прямим посиланням на повідомлення, або відповіддю на повідомлення.\n" +
                            "Видалення відповіддю на повідомлення здійснюється шляхом прописання відповідної команди у тексті відповіді.\n" +
                            "Якщо необхідно видалити за прямим посиланням, то замість відповіді необхідно дописати в кінці команди пряме посилання на повідомлення " +
                            "(видалення за прямим посиланням дозволяє видаляти повідомлення на іншому каналі сервера).\n" +
                            "\nНаявні 5 команд для видалення:\n" +
                            "**!delete_this** – видаляє вказане повідомлення.\n" +
                            "**!delete_after** ***count*** – масово видаляє вказану кількість повідомлень ПІСЛЯ вказаного повідомлення, " +
                            "можливе видалення лише тих повідомлень, які створені менше 2 тижнів тому.\n" +
                            "**!delete_slow_after** ***count*** – почергово видаляє вказану кількість повідомлень ПІСЛЯ вказаного повідомлення, " +
                            "якщо видаляється занадто велика кількість повідомлень, дискорд може обмежити можливість видалення на деякий час.\n" +
                            "**!delete_before** ***count*** – масово видаляє вказану кількість повідомлень ДО вказаного повідомлення, " +
                            "можливе видалення лише тих повідомлень, які створені менше 2 тижнів тому.\n" +
                            "**!delete_slow_before** ***count*** – почергово видаляє вказану кількість повідомлень ДО вказаного повідомлення, " +
                            "якщо видаляється занадто велика кількість повідомлень, дискорд може обмежити можливість видалення на деякий час.\n" +
                            "\nЯкщо задана кількість перевищує наявну кількість повідомлень у каналі, " +
                            "то буде видалено лише наявні повідомлення у заданому напрямку.\n" +
                            "Використовувати команду можуть лише власники серверів та користувачі з ролями \"administrator\", \"moderator\" або \"raid lead\".";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;
            }
        }
    }
}
