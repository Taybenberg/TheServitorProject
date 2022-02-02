using BungieSharper.Entities.Destiny.HistoricalStats.Definitions;
using CommonData.Localization;
using Discord;
using ServitorDiscordBot.BotCommands.SlashCommands;

namespace ServitorDiscordBot.BotCommands
{
    internal static class CommandHelper
    {
        public static ISlashCommand[] SlashCommands =>
            new ISlashCommand[]
            {
                new HelpCommand(),
                new BipCommand(),
                new ModesCommand(),
                new ClanActivitiesCommand(),
                new MyActivitiesCommand(),
                new MyPartnersCommand(),
                new SuspiciousActivitiesCommand()
            };

        public static EmbedBuilder WaitResponceBuilder =>
            new EmbedBuilder()
                .WithColor(0x76766B)
                .WithTitle(CommonData.DiscordEmoji.Emoji.Loading)
                .WithDescription(GetCommandExecutionImpression());

        public static EmbedBuilder UserIsNotRegisteredBuilder =>
            new EmbedBuilder()
                .WithColor(0xF1BA74)
                .WithTitle("Необхідна реєстрація")
                .WithDescription("Ґардіане, спершу вас необідно ідентифікувати у базі даних Авангарду.\n" +
                "Це здійснюється шляхом виконання процедури реєстрації.\nДля цього скористайтеся командою **реєстрація**");

        public static string GetActivityCountImpression(int count, string name) =>
            new Random().Next(10) switch
            {
                0 => $"Неймовірно! **{count}** активностей на рахунку {name}! Так тримати!",
                1 => $"Оце так! **{count}** активностей на рахунку {name}! Куди там залізним лордам до вас!",
                2 => $"Надзвичайно! **{count}** активностей на рахунку {name}! Та ви скажені!",
                3 => $"Непосильно! **{count}** активностей на рахунку {name}! Вас варто боятися!",
                4 => $"Парадоксально! **{count}** активностей на рахунку {name}! Марі Сов варто було повернутися лишень заради того, щоб побачити вас!",
                5 => $"Фантастика! **{count}** активностей на рахунку {name}! Це ви вбиваєте богів? Тоді все зрозуміло.",
                6 => $"Немислимо! **{count}** активностей на рахунку {name}! Від вашого світла можна осліпнути!",
                7 => $"Незрівнянно! **{count}** активностей на рахунку {name}! Ви виняткові!",
                8 => $"Непомірно! **{count}** активностей на рахунку {name}! Ви ще не розтрощили якесь небесне тіло? Саме пора!",
                _ => $"Нічого собі! **{count}** активностей на рахунку {name}! Авангард шокований!"
            };

        public static string GetCommandExecutionImpression() =>
            new Random().Next(30) switch
            {
                0 => $"Я займусь вашою командою, а ви поки порахуйте бейбі фоленів.",
                1 => $"А за командою командаа…\nА ЗА КОМАНДОЮ КОМАНДАА…\nОСЬ І КІНЧИЛИСЬ ЮЗЕРИ, АЛЕ НІ, ТАМ ЩЕ КОМАНДА…",
                2 => $"Команду прийнято до виконання.",
                3 => $"Ви готові побачити результат команди?\nТак, так, капітане!\nЯ не чую!\nТАК, ТАК, КАПІТАНЕ!",
                4 => $"А що, якщо ваша команда це насправді ілюзія?",
                5 => $"Ваша команда витратить більше блиску, ніж зазвичай.",
                6 => $"О, ви ще не забули про мене?\nВиконую команду для вас.",
                7 => $"Виконую ваш запит, на це знадобиться трохи часу…",
                8 => $"Усім залишатися на своїх місцях!\nВиконується команда!",
                9 => $"Нас було дев'ять, а потім виконалася ця команда…",
                10 => $"Ви хочете, щоб я виконав вашу команду, так?\nВи ж дійсно цього хочете, так?",
                11 => $"Ніхто:\nАбсолютно ніхто:\nВи:",
                12 => $"Кажете, виконати команду?\nСпершу женіть блиск!",
                13 => $"А я знаю, що ви хочете, щоб я виконав команду!",
                14 => $"Подейкують, що користувачів, які мене дуже задовбували більше ніхто не бачив.\nНу ви чекайте результат, чекайте…",
                15 => $"Були часи, коли в одному рейді поміщалося по 12 ґардіанів.\nОй булоо, ой булоо…\nА ви оце різними командами цікавитесь.",
                16 => $"Лиш вонаа, лиш вонаа, сидітиме сумна, буде пити – не п'яніти від дешевої команди…",
                17 => $"Ініціалізація модуля Kepler-186 для виконання команди…",
                18 => $"Межі між реальностями команд починають стиратися.",
                _ => $"Команда виконується.\nВи завжди можете підтримати розробку бота [філіжанкою кави](https://www.buymeacoffee.com/servitor)."
            };

        public static DateTime? GetPeriod(string value) =>
            value switch
            {
                "останній тиждень" => DateTime.UtcNow.AddDays(-7),
                "останній місяць" => DateTime.UtcNow.AddMonths(-1),
                "останній рік" => DateTime.UtcNow.AddYears(-1),
                _ => null
            };

        public static SlashCommandOptionBuilder AddPeriodChoises(this SlashCommandOptionBuilder optionBuilder)
        {
            var periods = new string[]
            {
                "останній тиждень",
                "останній місяць",
                "останній рік"
            };

            optionBuilder.Choices = periods
                .Select(x => new ApplicationCommandOptionChoiceProperties
                {
                    Name = x,
                    Value = x
                }).ToList();

            return optionBuilder
                .WithType(ApplicationCommandOptionType.String);
        }

        public static SlashCommandOptionBuilder AddSuspiciousActivitiesChoises(this SlashCommandOptionBuilder optionBuilder)
        {
            var activityModes = new DestinyActivityModeType[]
            {
                DestinyActivityModeType.Raid,
                DestinyActivityModeType.Dungeon,
                DestinyActivityModeType.ScoredNightfall
            };

            var activityNames = activityModes
                .Select(x => Translation.StatsActivityNames[x][0].ToLower());

            optionBuilder.Choices = activityNames
                .Select(x => new ApplicationCommandOptionChoiceProperties
                {
                    Name = x,
                    Value = x
                }).ToList();

            return optionBuilder
                .WithType(ApplicationCommandOptionType.String);
        }
    }
}
