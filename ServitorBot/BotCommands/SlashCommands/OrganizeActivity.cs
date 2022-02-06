using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
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
                    .WithTitle($"Збір у активності")
                    .WithImageUrl("https://i.imgur.com/UG7Go73.png")
                    .WithDescription($"Команда **{CommandName}** дозволяє організувати збір у вказану активність на визначену дату та має наступні параметри:" +
                        $"\n\n**режим** - тип активності, впливає на іконку збору та можливий розмір бойової групи (1, 3, 4 або 6 гравців)." +
                        $"Список режимів можна переглянути за допомогою команди **режими**, " +
                        $"утім режим може бути довільним на ваш розсуд (тоді розмір групи становитиме 3 гравців)." +
                        $"\n\n**дата** - запланована дата вашої активності, має бути у форматі **dd.MM-HH:mm** (день.місяць-година:хвилини), " +
                        $"уважно плануйте дату, якщо сьогодні, до прикладу, 10 лютого, а ви збираєте на 9, то це буде аж у наступному році." +
                        $"\n\n**назва** - назва вашої активності, параметр необов'язковий та декоративний. Якщо ви вкажете відому ботові назву, " +
                        $"тоді буде підставлено відповідну іконку. Для рейдів можна вказати шаблон назви (до прикладу, **lw** буде перетворено у **Last Wish**)." +
                        $"\n\n**опис** - нотатка до активності яка допоможе іншим гравцям зрозуміти мету збору. Параметр необов'язковий.")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Керування активністю")
                    .WithImageUrl("https://i.imgur.com/gYzvrKX.png")
                    .WithDescription($"Керувати активністю може лише організатор збору.\n" +
                        $"Для цього організатор має відповісти на повідомлення зі збором однією з наступних команд:" +
                        $"\n\n**!скасувати** - скасовує збір у активність. Видалення повідомлення зі збором також скасує збір." +
                        $"\n\n**!змінити режим** ***%режим%*** - змінює тип активності. Список режимів можна переглянути за допомогою команди **режими**." +
                        $"\n\n**!змінити назву** ***%назва%*** - змінює або встановлює назву активності. Для рейдів можна вказати шаблон назви." +
                        $"\n\n**!змінити опис** ***%опис%*** - змінює або встановлює нотатку до збору." +
                        $"\n\n**!перенести** ***%дата%*** - переносить заплановану дату активності. Дата має бути у форматі **dd.MM-HH:mm** (день.місяць-година:хвилини)." +
                        $"\n\n**!зарезервувати** ***@user1, @user2…*** - додає вказаних користувачів до списку бойової групи." +
                        $"\n\n**!виключити** ***@user1, @user2…*** - вилучає вказаних користувачів зі списку бойової групи.")
                    .Build(),

                new EmbedBuilder()
                    .WithColor(0xBE5BEF)
                    .WithTitle($"Бойова група")
                    .WithImageUrl("https://i.imgur.com/piqyD64.png")
                    .WithDescription($"Перший гравець бойової групи вважається організатором збору, він має права на використання команд керування активністю.\n" +
                        $"Основну бойову групу формують гравці починаючи з другого до максимального розміру групи, який передбачає даний тип активності. " +
                        $"Ці гравці отримують нагадування про наближення початку активності та сповіщення про оновлення збору.\n" +
                        $"Інші гравці формують лаву запасних.\n" +
                        $"\nЗаписатися/виписатися з активності може будь-який користувач з роллю **Destiny 2** натиснувши кнопку **+**.\n" +
                        $"Організатор активності може додавати чи вилучати користувачів з бойової групи за допомогою команд керування активністю.\n" +
                        $"\nЯкщо з черги бойової групи вибуває гравець, то гравець після нього стає на його місце. " +
                        $"Якщо з черги вибувають геть усі гравці, то активність буде атоматично скасовано.\n" +
                        $"\nЗа допомогою команди **!передати** ***@адресат*** можна передати своє місце у черзі іншому гравцеві. " +
                        $"Якщо інший гравець присутній у черзі, але його місце має нижчий пріоритет, то ви з ним поміняєтеся місцями; " +
                        $"якщо інший гравець взагалі відсутній у черзі, то він займе ваше місце, а ви вибудете з черги.")
                    .Build()
            };
    }
}
