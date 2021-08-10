using Discord;
using System.Linq;
using System.Threading.Tasks;

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
                when messageCommands[MessagesEnum.Help]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Help][0]}** " +
                            $"виводить перелік доступних команд у боті.\n" +
                            $"Деякі команди мають параметри, які виділені симолом **%**, це означає, що параметр динамічний " +
                            $"та вказується користувачем індивідуально у заданому форматі для команди.\n" +
                            $"Символ **%** у тексті параметру вказувати не потрібно.\nСкористайтеся командою " +
                            $"**{messageCommands[MessagesEnum.Help][0]} %команда%**, " +
                            $"щоби переглянути використання параметру для команди чи інші відомості по команді.\n" +
                            $"\nБойова група:\n" +
                            $"<@228896926991515649> - автор бота **{_client.CurrentUser.Username}**\n" +
                            $"<@679220982082174977> - дизайн інформаціних карток\n" +
                            $"<@356816080326361088> - дизайн інформаціних карток\n" +
                            $"<@326308954000850944> - інформаційний супровід та просування";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Bip]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Bip][0]}** " +
                            $"допомагає визначити, чи приймаються в даний момент команди.\n" +
                            $"Якщо бот функціонує, то у відповідь ви отримаєте повідомлення **біп…**\n" +
                            $"Використовуйте цю команду, якщо ви не отримали результат іншої команди, або якщо вважаєте, що бот може не працювати.";

                        builder.ImageUrl = "https://i.imgur.com/fWFj2Xz.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Weekly]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Weekly][0]}** " +
                            $"дозволяє переглянути тижневу ротацію найтфолу та горнила, а також визначає, чи доступний цього тижня Залізний стяг.\n" +
                            $"Вміст цього повідомлення є частиною інформаційної картки про тижневий ресет (дизайнер картки - <@356816080326361088>), " +
                            $"яка надсилається автоматично щовівторка на початку кожного тижня у Destiny 2.\n" +
                            $"Якщо в момент виконання команди сервери Destiny не працюють, то результат команди не буде отримано.\n" +
                            $"Наявність Залізного стягу перевіряється на ресурсі https://www.light.gg";

                        builder.ImageUrl = "https://i.imgur.com/iZ70dgo.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Sectors]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Sectors][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки - <@679220982082174977>) " +
                            $"з відомостями про загублені сектори складності \"легенда\" та \"майстер\" цього денного ресету.\n" +
                            $"Вміст цієї картки є частиною інформаційної картки про денний ресет (дизайнер картки - <@356816080326361088>), " +
                            $"яка надсилається автоматично на початку кожного дня у Destiny 2.\n" +
                            $"На початку нового сезону або з появою нових секторів можливе виведення хибної інформації.\n" +
                            $"Інформація підтягується з ресурсу https://www.todayindestiny.com/";

                        builder.ImageUrl = "https://i.imgur.com/0hudBnh.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Resources]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Resources][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки - <@356816080326361088>) " +
                            $"з відомостями про асортимент ресурсів та модів Ади-1, Банші-44 та Павука цього денного ресету.\n" +
                            $"Вміст цієї картки є частиною інформаційної картки про денний ресет (дизайнер картки - <@356816080326361088>), " +
                            $"яка надсилається автоматично на початку кожного дня у Destiny 2.\n" +
                            $"Інформація підтягується з ресурсу https://www.todayindestiny.com/vendors";

                        builder.ImageUrl = "https://i.imgur.com/LroPcmY.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Xur]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Xur][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки - <@679220982082174977>) " +
                            $"з відомостями про поточний асортимент Зура. Буде згенеровано порожню картку, " +
                            $"якщо на момент виклику команди Зур відсутній, або сервери Destiny не працюють.\n" +
                            $"Картка надсилається автоматично щоп'ятниці після денного ресету.\n" +
                            $"З метою уникнення флуду команда користувача та попередньо згенерована картка видаляються.\n" +
                            $"Місцеперебування Зура підтягується з ресурсу https://xur.wiki/";

                        builder.ImageUrl = "https://i.imgur.com/QbdQyRp.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Osiris]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Osiris][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки - <@679220982082174977>) " +
                            $"з відомостями про лутпул Випробувань Осіріса. Якщо на момент виклику команди випробування ще не активні, " +
                            $"то буде згенеровано порожню картку, якщо лутпул відомо лише частково - картка буде частково заповненою.\n" +
                            $"З метою уникнення флуду команда користувача та попередньо згенерована картка видаляються.\n" +
                            $"Інформація підтягується з ресурсу https://www.light.gg";

                        builder.ImageUrl = "https://i.imgur.com/kpgx3SK.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Eververse]
                .Any(x => c.StartsWith(x)):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Eververse][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки - <@679220982082174977>) " +
                            $"з відомостями про асортимент Тесс Еверіс за визначений тиждень.\n" +
                            $"\nМожливі 3 варіанти команди:\n" +
                            $"**{messageCommands[MessagesEnum.Eververse][0]}** без параметрів - виводить асортимент поточного тижня\n" +
                            $"**{messageCommands[MessagesEnum.Eververse][0]}** зі статичним параметром ***все*** - виводить асортимент за всі тижні сезону\n" +
                            $"**{messageCommands[MessagesEnum.Eververse][0]}** з динамічним параметром ***№ тижня*** - " +
                            $"виводить асортимент за вказаний тиждень сезону, потрібно прописати номер тижня\n" +
                            $"\nЯкщо вказано параметр, формат якого не відповідає вказаним вище (або номер тижня виходить за діапазон сезону), " +
                            $"то буде виведено асортимент поточного тижня.\n" +
                            $"Вміст цієї картки є частиною інформаційної картки про тижневий ресет (дизайнер картки - <@356816080326361088>), " +
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

                case string c
                when messageCommands[MessagesEnum.MyActivities]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.MyActivities][0]}** " +
                            $"обчислює кількість ваших активностей, які ви закрили перебуваючи учасником клану (але не раніше 01.01.2021).\n" +
                            $"Підрахунок ведеться загалом на акаунт, на окремих персонажів та для кожного типу активності окремо.\n" +
                            $"Додатково виводиться інфографіка, яка показує % активностей " +
                            $"ПвП (горнило), ПвПвЕ (гамбіт) та ПвЕ (все інше).\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.MyPartners]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.MyPartners][0]}** " +
                            $"обчислює кількість активностей, які ви закрили перебуваючи учасником клану " +
                            $"(але не раніше 01.01.2021) разом з іншими користувачами.\n" +
                            $"Список виводиться за спаданням кількості спільних активностей.\n" +
                            $"Для десяти найбільш споріднених гравців з вами додатково виводиться інфографіка, " +
                            $"яка показує кількість активностей ПвП (горнило), ПвПвЕ (гамбіт) та ПвЕ (все інше).\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.ClanActivities]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.MyActivities][0]}** " +
                            $"обчислює кількість активностей, які закрили учасники клану (але не раніше 01.01.2021).\n" +
                            $"Підрахунок ведеться загалом та для кожного типу активності окремо.\n" +
                            $"Додатково виводиться інфографіка, яка показує % активностей " +
                            $"ПвП (горнило), ПвПвЕ (гамбіт) та ПвЕ (все інше).";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Modes]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Modes][0]}** " +
                            $"виводить список наявних типів активностей, " +
                            $"які можна використовувати у якості параметрів для команд " +
                            $"**{messageCommands[MessagesEnum.ClanStats][0]}** та **{messageCommands[MessagesEnum.Leaderboard][0]}**.";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.ClanStats]
                .Any(x => c.StartsWith(x)):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.ClanStats][0]}** " +
                            $"виводить агреговану статистику учасників клану за різними показниками у вказаній активності.\n" +
                            $"Команда потребує динамічного параметру ***режим***, який вказує тип активності, для якого виводиться статистика.\n" +
                            $"Переглянути список можливих параметрів можна за допомогою команди **{messageCommands[MessagesEnum.Modes][0]}**.\n" +
                            $"Увага! Команда досі в бета-версії!";

                        builder.ImageUrl = "https://i.imgur.com/P8u73oD.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Leaderboard]
                .Any(x => c.StartsWith(x)):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Leaderboard][0]}** " +
                            $"виводить список лідерів клану за різними показниками у вказаній активності.\n" +
                            $"Типово виводиться 3 користувачі з найкращим результатом певного показника, утім, " +
                            $"якщо команду викликає зареєстрований користувач, він зможе побачити власне місце у списку, " +
                            $"а також проглянути інфографіку для своїх показників.\n" +
                            $"Команда потребує динамічного параметру ***режим***, який вказує тип активності, для якого виводиться статистика.\n" +
                            $"Переглянути список можливих параметрів можна за допомогою команди **{messageCommands[MessagesEnum.Modes][0]}**.\n" +
                            $"Увага! Команда досі в бета-версії!";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Apostates]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Apostates][0]}** " +
                            $"виводить список останніх рейдів, підземель та випробувань осіріса, " +
                            $"у яких були учасники клану з користувачами, які не є учасниками клану.\n" +
                            $"Список активностей містить список користувачів, які були в даній активності та їхні клантеги.";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum._100K]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum._100K][0]}** " +
                            $"виводить список останніх найтфолів на складності \"легенда\" і вище, " +
                            $"у яких були учасники клану з користувачами, які не є учасниками клану.\n" +
                            $"Список найтфолів містить список користувачів, які були в даному найтфолі та їхні клантеги.";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Register]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Register][0]}** " +
                            $"призначена для того, щоб встановити, якому учаснику клану в Destiny 2 відповідає ваш профіль Discord.\n" +
                            $"Це дозволить вам користуватися всіма перевагами бота.\n" +
                            $"Якщо ваш нік в Discord такий самий, як і в грі, чи дуже на нього схожий (схожість понад 90%), " +
                            $"то вас буде зареєстровано автоматично одразу після виклику команди **{messageCommands[MessagesEnum.Register][0]}**.\n" +
                            $"Інакше вам потрібно буде уточнити свій нікнейм вручну у якості динамічного параметра " +
                            $"для команди **{messageCommands[MessagesEnum.NotRegistered][0]}**.\n" +
                            $"Потрібно вказувати нікнейм саме тієї платформи, з якої відбулося приєднання до клану.";

                        builder.ImageUrl = "https://i.imgur.com/tEsgGor.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;
            }
        }
    }
}
