using CommonData.Activities;
using CommonData.Localization;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class OrganizeActivity : ISlashCommand
    {
        public string CommandName => "збір";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Організувати збір у активність")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("режим")
                    .WithDescription("Переглянути список режимів можна за допомогою команди \"режими\"")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.String))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("дата")
                    .WithDescription("Дата повинна бути у форматі \"dd.MM-HH:mm\"")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.String))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("назва")
                    .WithDescription("Назва активності (довільна)")
                    .WithRequired(false)
                    .WithType(ApplicationCommandOptionType.String))
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("опис")
                    .WithDescription("Нотатка до збору")
                    .WithRequired(false)
                    .WithType(ApplicationCommandOptionType.String));

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command) =>
            await command.RespondAsync(embeds: HelpEmbeds, ephemeral: true);

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory) =>
            await command.RespondAsync(embed: CommandHelper.WrongChannelBuilder.Build(), ephemeral: true);

        public Embed[] HelpEmbeds =>
            new Embed[]
            {
                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Допомога \"{CommandName}\"")
                    .WithImageUrl("https://i.imgur.com/gj0TWAY.png")
                    .WithDescription("Команда дозволяє організувати збір у вказаний рейд на визначену дату.\n" +
                                "За 10 хвилин до початку рейду бот надсилає сповіщення першим шести ґардіанам у черзі про те, що наближається рейд.\n" +
                                "Через 60 хвилин після початку рейду збір видаляється автоматично.\n" +
                                "Команди керування збором у рейди видаляються автоматично.\n" +
                                "\nОсновні можливості:\n" +
                                "– Організація збору в рейд\n" +
                                "– Скасування збору в рейд\n" +
                                "– Перенесення рейду\n" +
                                "– Резервування місць у рейді\n" +
                                "– Передача місця у черзі\n")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Організація збору в рейд")
                    .WithImageUrl("https://i.imgur.com/OB8O85p.png")
                    .WithDescription($"Збір у рейд виконується за допомогою команди **рейд** з наступними параметрами:\n" +
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
                                $"Різниця між датою збору й поточною датою не може перевищувати 30 днів.")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Скасування рейду")
                    .WithImageUrl("https://i.imgur.com/TIuPX3o.png")
                    .WithDescription("Скасування рейду вилучає оголошення бота про збір у рейд.\n" +
                            "Скасований рейд назавжди вилучається з планувальника бота, " +
                            "тому учасники збору не отримають сповіщення про збір перед початком рейду, " +
                            "а будь-які модифікації збору стануть неможливими.\n" +
                            "\nСкасування рейду можливе кількома способами:\n" +
                            "– За допомогою відповіді на оголошення бота про збір у рейд, яка містить команду " +
                            "**скасувати** (командою може скористатися лише організатор збору).\n" +
                            "– Шляхом видалення оголошення бота про збір у рейд.\n" +
                            "– Автоматично, якщо у черзі не залишиться жодного гравця.\n" +
                            "– Автоматично через 60 хвилин після дати початку рейду.")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Перенесення рейду")
                    .WithImageUrl("https://i.imgur.com/xXQv0Na.png")
                    .WithDescription("Перенесення рейду дозволяє змінити дату початку рейду.\n" +
                            "За 10 хвилин до початку рейду бот надсилає сповіщення першим шести ґардіанам у черзі про те, що наближається рейд.\n" +
                            "Через 60 хвилин після початку рейду збір видаляється автоматично.\n" +
                            "\nПеренесення рейду здійснюється шляхом надсилання відповіді на оголошення бота про збір у рейд, " +
                            "яка містить команду **перенести** ***%дата%***.\n" +
                            "\nЗверніть увагу, що параметр ***дата*** з технічних міркувань не може бути старшим більше ніж на 30 днів за поточну дату.")
                    .Build(),
                /*
                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Резервування місць у рейді")
                    .WithImageUrl("https://i.imgur.com/ocwoLH2.png")
                    .WithDescription($"Резервування місця дозволяє закріпити місце за гравцем у черзі на збір у рейд.\n" +
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
                            $"Якщо у черзі не залишиться жодного гравця, збір у рейд буде скасовано.")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Передача місця у черзі")
                    .WithImageUrl("https://i.imgur.com/HblDCxf.png")
                    .WithDescription($"Передача місця у черзі забезпечує можливість звільнення гравцем свого місця у черзі для наступного гравця.\n" +
                            $"Якщо місце передає організатор збору, то таким чином він втратить повноваження огранізатора, " +
                            $"а організатором збору стане інший гравець у черзі.\n" +
                            $"\nПередати місце у черзі можна кількома способами:\n" +
                            $"– Шляхом додавання гравцем реакції {CommonData.DiscordEmoji.Emoji.UnCheck} до оголошення бота про збір у рейд " +
                            $"(місце гравця отримає наступний гравець у черзі).\n" +
                            $"– Шляхом надсилання відповіді на оголошення бота про збір у рейд," +
                            $"яка місить команду **передати** ***%@гравець%*** (якщо згаданий гравець не перебуває у черзі, то гравець, " +
                            $"який викликав команду втратить своє місце у черзі, а його місце займе згаданий гравець; " +
                            $"якщо місце згаданого гравця знаходиться далі у черзі після гравця, що викликав команду, то вони поміняються місцями).\n" +
                            $"\nЗверніть увагу, що якщо у черзі не залишиться жодного гравця, збір у рейд буде скасовано.")
                    .Build(),*/
            };
    }
}
