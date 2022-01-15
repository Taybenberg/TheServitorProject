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
                            $"<@228896926991515649> – розробник бота **{_client.CurrentUser.Username}**\n" +
                            $"<@679220982082174977> – дизайн інформаційних карток\n" +
                            $"<@356816080326361088> – дизайн інформаційних карток\n" +
                            $"<@326308954000850944> – інформаційний супровід та просування\n" +
                            $"<@373381055924797441> – тестування\n" +
                            $"<@225342881953611777> – тестування";

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
                            $"Вміст цього повідомлення є частиною інформаційної картки про тижневий ресет (дизайнер картки – <@356816080326361088>), " +
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
                            $"генерує інформаційну картку (дизайнер картки – <@679220982082174977>) " +
                            $"з відомостями про загублені сектори складності \"легенда\" та \"майстер\" цього денного ресету.\n" +
                            $"Вміст цієї картки є частиною інформаційної картки про денний ресет (дизайнер картки – <@356816080326361088>), " +
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
                when messageCommands[MessagesEnum.Xur]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Xur][0]}** " +
                            $"генерує інформаційну картку (дизайнер картки – <@679220982082174977>) " +
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

                case string c
                when messageCommands[MessagesEnum.MyActivities]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.MyActivities][0]}** " +
                            $"обчислює кількість ваших активностей, які ви закрили перебуваючи учасником клану за вказаний період.\n" +
                            $"\nМожливі 3 варіанти команди:\n" +
                            $"**{messageCommands[MessagesEnum.MyActivities][0]}** без параметрів – виводить звіт за весь час (але не раніше 01.01.2021).\n" +
                            $"**{messageCommands[MessagesEnum.MyActivities][0]}** зі статичним параметром ***тиждень*** – виводить звіт за останні 7 днів.\n" +
                            $"**{messageCommands[MessagesEnum.MyActivities][0]}** зі статичним параметром ***місяць*** – виводить звіт за останні 30 днів.\n" +
                            $"\nПідрахунок ведеться загалом на акаунт, на окремих персонажів та для кожного типу активності окремо.\n" +
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
                            $"за вказаний період разом з іншими учасниками клану (кооперативні активності).\n" +
                            $"Також обчислюється співвідношення кооперативних активностей і загальної кількості активностей.\n" +
                            $"\nМожливі 3 варіанти команди:\n" +
                            $"**{messageCommands[MessagesEnum.MyPartners][0]}** без параметрів – виводить звіт за весь час (але не раніше 01.01.2021).\n" +
                            $"**{messageCommands[MessagesEnum.MyPartners][0]}** зі статичним параметром ***тиждень*** – виводить звіт за останні 7 днів.\n" +
                            $"**{messageCommands[MessagesEnum.MyPartners][0]}** зі статичним параметром ***місяць*** – виводить звіт за останні 30 днів.\n" +
                            $"\nСписок виводиться за спаданням кількості спільних активностей.\n" +
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
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.ClanActivities][0]}** " +
                            $"обчислює кількість активностей, які закрили учасники клану за вказаний період.\n" +
                            $"\nМожливі 3 варіанти команди:\n" +
                            $"**{messageCommands[MessagesEnum.ClanActivities][0]}** без параметрів – виводить звіт за весь час (але не раніше 01.01.2021).\n" +
                            $"**{messageCommands[MessagesEnum.ClanActivities][0]}** зі статичним параметром ***тиждень*** – виводить звіт за останні 7 днів.\n" +
                            $"**{messageCommands[MessagesEnum.ClanActivities][0]}** зі статичним параметром ***місяць*** – виводить звіт за останні 30 днів.\n" +
                            $"\nПідрахунок ведеться загалом та для кожного типу активності окремо.\n" +
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
                            $"виводить агреговану статистику учасників вашого клану за різними показниками у вказаній активності.\n" +
                            $"Команда потребує динамічного параметру ***режим***, який вказує тип активності, для якого виводиться статистика.\n" +
                            $"Переглянути список можливих параметрів можна за допомогою команди **{messageCommands[MessagesEnum.Modes][0]}**.\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.\n" +
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
                            $"виводить список лідерів вашого клану за різними показниками у вказаній активності.\n" +
                            $"Виводиться 3 користувачі з найкращим результатом певного показника, " +
                            $"якщо вас немає у першій трійці, то додатково вказується ваше місце у рейтингу.\n" +
                            $"Додатково виводиться інфографіка для ваших показників.\n" +
                            $"Команда потребує динамічного параметру ***режим***, який вказує тип активності, для якого виводиться статистика.\n" +
                            $"Переглянути список можливих параметрів можна за допомогою команди **{messageCommands[MessagesEnum.Modes][0]}**.\n" +
                            $"Використання цієї команди вимагає реєстрації в боті.\n" +
                            $"Увага! Команда досі в бета-версії!";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case string c
                when messageCommands[MessagesEnum.Apostates]
                .Contains(c):
                    {
                        builder.Description = $"Команда **{messageCommands[MessagesEnum.Apostates][0]}** " +
                            $"виводить список останніх рейдів та підземель, " +
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
                            $"Потрібно вказувати глобальний нікнейм Bungie.";

                        builder.ImageUrl = "https://i.imgur.com/tEsgGor.png";

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                    return;

                case "музика" or "music":
                    {
                        builder.Title = "Музика";

                        builder.Description = "Відтворює аудіодоріжку зі вказаного аудіо, відео чи плейлисту " +
                            "в YouTube, YouTube Music, Soundcloud або ж аудіофайл з іншого ресурсу.\n" +
                            "Перед використанням команди **play** ви маєте бути під'єднані до голосового каналу, щоби бот знав, куди під'єднуватися.\n" +
                            "Усіма іншими командами користуйтеся лише після того, як бот почне відтворення.\n" +
                            "\nНаявні наступні типи команд:\n" +
                            "**play** ***%URL%*** – під'єднується до голосового каналу та відтворює вказане відео, аудіо чи плейлист.\n" +
                            "**playdirect** ***%URL%*** – під'єднується до голосового каналу та відтворює аудіо " +
                            "за прямим посиланням у обмеженому режимі (без пітримки команд керування чергою).\n" +
                            "**add** ***%URL%*** – додає до черги відтворення вказане відео чи плейлист.\n" +
                            "**prev** – відтворює попереднє відео у черзі.\n" +
                            "**next** – відтворює наступне відео у черзі.\n" +
                            "**queue** – виводить список відео у черзі.\n" +
                            "**shuffle** – перемішує чергу (поточне відео стає першим у черзі).\n" +
                            "**pause** – призупиняє відтворення поточного відео.\n" +
                            "**continue** – продовжує відтворення поточного відео.\n" +
                            "**stop** – зупиняє відтворення та від'єднується від голосового каналу.\n" +
                            "\nВідтворення зупиняється автоматично після досягнення кінця черги (стосується команд **prev** та **next**).\n" +
                            "Також ви можете зупинити відтворення перетягнувши бота у інший канал.\n" +
                            "Зверніть увагу, що бот може відтворювати лише одне відео та перебувати лише у одному голосовому каналі за раз.\n" +
                            "Не підтримуються живі етери YouTube та відео, які вимагають авторизації для підтвердження віку.";

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

                case "рейд" or "raid":
                    {
                        {
                            builder.ImageUrl = "https://i.imgur.com/gj0TWAY.png";

                            builder.Description = "Команда **рейд** " +
                                "дозволяє організувати збір у вказаний рейд на визначену дату.\n" +
                                "За 10 хвилин до початку рейду бот надсилає сповіщення першим шести ґардіанам у черзі про те, що наближається рейд.\n" +
                                "Через 60 хвилин після початку рейду збір видаляється автоматично.\n" +
                                "Команди керування збором у рейди видаляються автоматично.\n" +
                                "\nОсновні можливості:\n" +
                                "– Організація збору в рейд\n" +
                                "– Скасування збору в рейд\n" +
                                "– Перенесення рейду\n" +
                                "– Резервування місць у рейді\n" +
                                "– Передача місця у черзі\n";

                            await message.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        {
                            builder.Title = "Організація збору в рейд";

                            builder.ImageUrl = "https://i.imgur.com/OB8O85p.png";

                            builder.Description = $"Збір у рейд виконується за допомогою команди **рейд** з наступними параметрами:\n" +
                                $"**рейд** ***%тип% %дата%*** – створює рейд заданого типу на визначену дату.\n" +
                                $"**рейд** ***%тип% %дата% %опис%*** – створює рейд заданого типу на визначену дату та містить опис.\n" +
                                $"**рейд** ***%тип% %дата% %список гравців%*** – створює рейд заданого типу на визначену дату та резервує місце за вказаними гравцями.\n" +
                                $"**рейд** ***%тип% %дата% %опис% %список гравців%*** – створює рейд заданого типу на визначену дату, містить опис та резервує місце за вказаними гравцями.\n" +
                                $"\nПідтримується організація збору в рейди наступних типів (використовуйте один з аліасів для параметру ***тип***):\n" +
                                $"{CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(Activity.ActivityRaidType.LW)} **{Translation.ActivityRaidTypes[Activity.ActivityRaidType.LW]}** – **LW**, **ЛВ**, **ОБ**\n" +
                                $"{CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(Activity.ActivityRaidType.GOS)} **{Translation.ActivityRaidTypes[Activity.ActivityRaidType.GOS]}** – **GOS**, **СП**, **СС**\n" +
                                $"{CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(Activity.ActivityRaidType.DSC)} **{Translation.ActivityRaidTypes[Activity.ActivityRaidType.DSC]}** – **DSC**, **СГК**\n" +
                                $"{CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(Activity.ActivityRaidType.VOGL)} **{Translation.ActivityRaidTypes[Activity.ActivityRaidType.VOGL]}** – **VOG**, **ВОГ**, **КС**\n" +
                                $"{CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(Activity.ActivityRaidType.VOGM)} **{Translation.ActivityRaidTypes[Activity.ActivityRaidType.VOGM]}** – **VOGM**, **ВОГМ**, **КСМ**\n" +
                                $"\nЗверніть увагу, що з технічних міркувань можливу дату збору обмежено.\n" +
                                $"Різниця між датою збору й поточною датою не може перевищувати 30 днів.";

                            await message.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        {
                            builder.Title = "Скасування рейду";

                            builder.ImageUrl = "https://i.imgur.com/TIuPX3o.png";

                            builder.Description = "Скасування рейду вилучає оголошення бота про збір у рейд.\n" +
                                "Скасований рейд назавжди вилучається з планувальника бота, " +
                                "тому учасники збору не отримають сповіщення про збір перед початком рейду, " +
                                "а будь-які модифікації збору стануть неможливими.\n" +
                                "\nСкасування рейду можливе кількома способами:\n" +
                                "– За допомогою відповіді на оголошення бота про збір у рейд, яка містить команду " +
                                "**скасувати** (командою може скористатися лише організатор збору).\n" +
                                "– Шляхом видалення оголошення бота про збір у рейд.\n" +
                                "– Автоматично, якщо у черзі не залишиться жодного гравця.\n" +
                                "– Автоматично через 60 хвилин після дати початку рейду.";

                            await message.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        {
                            builder.Title = "Перенесення рейду";

                            builder.ImageUrl = "https://i.imgur.com/xXQv0Na.png";

                            builder.Description = "Перенесення рейду дозволяє змінити дату початку рейду.\n" +
                                "За 10 хвилин до початку рейду бот надсилає сповіщення першим шести ґардіанам у черзі про те, що наближається рейд.\n" +
                                "Через 60 хвилин після початку рейду збір видаляється автоматично.\n" +
                                "\nПеренесення рейду здійснюється шляхом надсилання відповіді на оголошення бота про збір у рейд, " +
                                "яка містить команду **перенести** ***%дата%***.\n" +
                                "\nЗверніть увагу, що параметр ***дата*** з технічних міркувань не може бути старшим більше ніж на 30 днів за поточну дату.";

                            await message.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        {
                            builder.Title = "Резервування місць у рейді";

                            builder.ImageUrl = "https://i.imgur.com/ocwoLH2.png";

                            builder.Description = $"Резервування місця дозволяє закріпити місце за гравцем у черзі на збір у рейд.\n" +
                                $"Оскільки місць на рейд всього 6, то передбачається, що перші 6 гравців з черги підуть у рейд, " +
                                $"утім гравці, для яких не знайшлося місця в черзі можуть записатися у лаву запасних, " +
                                $"і тоді вони зможуть претендувати на місце у бойовій групі, якщо місце попереду звільниться.\n" +
                                $"\nРезервування місця можливе кількома способами:\n" +
                                $"– Резервування організатором збору безпосердньо під час створення збору в рейд " +
                                $"шляхом додавання списку гравців до команди (резервується місце для гравців зі списку).\n" +
                                $"– Резервування організатором збору шляхом надсилання відповіді на оголошення бота про збір у рейд, " +
                                $"яка містить команду **зарезервувати** ***%список гравців%*** (резервується місце для гравців зі списку).\n" +
                                $"– Шляхом додавання гравцем з роллю <@&{_destinyRoleId}> реакції {CommonData.DiscordEmoji.Emoji.Check} до оголошення бота про збір у рейд " +
                                $"(резервується місце для гравця, що додав реакцію).\n" +
                                $"\nЗверніть увагу, що будь-який гравець з черги може звільнити своє місце у черзі шляхом додавання реакції " +
                                $"{CommonData.DiscordEmoji.Emoji.UnCheck} до оголошення бота про збір у рейд.\n" +
                                $"Якщо у черзі не залишиться жодного гравця, збір у рейд буде скасовано.";

                            await message.Channel.SendMessageAsync(embed: builder.Build());
                        }
                        {
                            builder.Title = "Передача місця у черзі";

                            builder.ImageUrl = "https://i.imgur.com/HblDCxf.png";

                            builder.Description = $"Передача місця у черзі забезпечує можливість звільнення гравцем свого місця у черзі для наступного гравця.\n" +
                                $"Якщо місце передає організатор збору, то таким чином він втратить повноваження огранізатора, " +
                                $"а організатором збору стане інший гравець у черзі.\n" +
                                $"\nПередати місце у черзі можна кількома способами:\n" +
                                $"– Шляхом додавання гравцем реакції {CommonData.DiscordEmoji.Emoji.UnCheck} до оголошення бота про збір у рейд " +
                                $"(місце гравця отримає наступний гравець у черзі).\n" +
                                $"– Шляхом надсилання відповіді на оголошення бота про збір у рейд," +
                                $"яка місить команду **передати** ***%@гравець%*** (якщо згаданий гравець не перебуває у черзі, то гравець, " +
                                $"який викликав команду втратить своє місце у черзі, а його місце займе згаданий гравець; " +
                                $"якщо місце згаданого гравця знаходиться далі у черзі після гравця, що викликав команду, то вони поміняються місцями).\n" +
                                $"\nЗверніть увагу, що якщо у черзі не залишиться жодного гравця, збір у рейд буде скасовано.";

                            await message.Channel.SendMessageAsync(embed: builder.Build());
                        }
                    }
                    return;
            }
        }
    }
}
