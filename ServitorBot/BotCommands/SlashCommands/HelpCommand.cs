using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorBot.BotCommands.SlashCommands
{
    internal class HelpCommand : ISlashCommand
    {
        public string CommandName => "допомога";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription($"Отримати допомогу по команді …")
                .AddOption(GetChoises());

        private SlashCommandOptionBuilder GetChoises()
        {
            var builder = new SlashCommandOptionBuilder()
                    .WithName("команда")
                    .WithDescription("Команда, по якій ви хочете отримати допомогу")
                    .WithRequired(true)
                    .WithType(ApplicationCommandOptionType.String);

            builder.Choices = CommandHelper.SlashCommands
                    .Select(x => new ApplicationCommandOptionChoiceProperties
                    {
                        Name = x.CommandName,
                        Value = x.CommandName
                    }).ToList();

            return builder;
        }

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .AddField("Бойова група", $"<@228896926991515649> – розробник бота\n" +
                            $"<@373381055924797441> – тестування\n" +
                            $"<@225342881953611777> – тестування")
                .AddField("Окрема подяка", $"<@679220982082174977> – дизайн інформаційних карток\n" +
                            $"<@356816080326361088> – дизайн інформаційних карток\n" +
                            $"<@326308954000850944> – інформаційний супровід");
                            

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            var option = command.Data.Options.FirstOrDefault();

            var slashCommand = CommandHelper.SlashCommands.FirstOrDefault(x => x.CommandName == (string)option.Value);

            if (slashCommand is not null)
                await slashCommand.ExecuteCommandHelpAsync(command);
        }
    }
}
