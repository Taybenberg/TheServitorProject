using ClanActivitiesService;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace ServitorDiscordBot.BotCommands.SlashCommands
{
    internal class RegisterCommand : ISlashCommand
    {
        public string CommandName => "реєстрація";

        public SlashCommandBuilder SlashCommand =>
            new SlashCommandBuilder()
                .WithName(CommandName)
                .WithDescription("Прив'язати акаунт Destiny 2 до профілю в Discord")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("нікнейм")
                    .WithDescription("Ваш нікнейм у Destiny 2")
                    .WithRequired(false)
                    .WithType(ApplicationCommandOptionType.String));

        public async Task ExecuteCommandHelpAsync(SocketSlashCommand command)
        {
            var builder = new EmbedBuilder()
                .WithColor(0xBE5BEF)
                .WithTitle($"Допомога \"{CommandName}\"")
                .WithDescription($"Команда призначена для того, щоб встановити, якому учаснику клану в Destiny 2 відповідає ваш профіль Discord.\n" +
                            $"Це дозволить вам користуватися всіма перевагами бота.\n" +
                            $"Якщо ваш нік в Discord такий самий, як і в грі, чи дуже на нього схожий (схожість понад 90%), " +
                            $"то ви можете зареєстрватися без вказування свого нікнейму у параметрах команди.\n" +
                            $"Інакше потрібно вказувати глобальний нікнейм Bungie.");

            await command.RespondAsync(embed: builder.Build());
        }

        public async Task ExecuteCommandAsync(SocketSlashCommand command, IServiceScopeFactory scopeFactory)
        {
            await command.RespondAsync(embed: CommandHelper.WaitResponceBuilder.Build());

            var option = command.Data.Options.FirstOrDefault();

            using var scope = scopeFactory.CreateScope();

            var clanActivities = scope.ServiceProvider.GetRequiredService<IClanActivities>();

            var registerContainer = option is null ?
                await clanActivities.TryRegisterUserAsync(command.User.Id, command.User.Username) :
                await clanActivities.TryRegisterUserAsync(command.User.Id, (string)option.Value);

            if (registerContainer is null)
            {
                var b = new EmbedBuilder()
                    .WithColor(0xFFDD71)
                    .WithTitle("Реєстрація")
                    .WithDescription($"Ґардіане, у цьому немає потреби - ви вже зареєстровані.");

                await command.ModifyOriginalResponseAsync(x => x.Embed = b.Build());
                return;
            }

            if (!registerContainer.IsSuccessful)
            {
                if (option is null)
                {
                    var b = new EmbedBuilder()
                        .WithColor(0xDDFAB0)
                        .WithTitle("Реєстрація")
                        .WithDescription($"Зареєструвавшись, ви зможете користуватися усіма наявними можливостями бота.\n" +
                            $"Важливо, аби ви були учасником клану **хоча б один день**.\n" +
                            $"У параметрі команди вкажіть свій нікнейм Bungie, він схожий на щось на кшталт **{command.User.Username}#1234**.\n" +
                            $"Якщо ви граєте на кількох платформах, то, як правило, " +
                            $"дійсним є нікнейм тієї платформи, з якої ви приєдналися до клану.\n" +
                            $"Якщо не пам'ятаєте нікнейм повністю, не проблема, я можу спробувати знайти його за частковим збігом.");

                    await command.ModifyOriginalResponseAsync(x => x.Embed = b.Build());
                }
                else
                {
                    var b = new EmbedBuilder()
                        .WithColor(0xFF8C67)
                        .WithTitle("Реєстрація")
                        .WithDescription($"Не вдалося знайти користувача.\n" +
                            $"Уточніть, будь ласка, свій нікнейм Bungie, він схожий на щось на кшталт **{command.User.Username}#1234**.\n" +
                            $"Якщо ви граєте на кількох платформах, спробуйте прописати нікнейм з іншої платформи.");

                    await command.ModifyOriginalResponseAsync(x => x.Embed = b.Build());
                }

                return;
            }

            var builder = new EmbedBuilder()
                .WithColor(0xA6F167)
                .WithTitle("Реєстрація")
                .WithDescription($"Зареєстровано {command.User.Mention} як гравця платформи {registerContainer.Platform} **{registerContainer.UserName}**.");

            await command.ModifyOriginalResponseAsync(x => x.Embed = builder.Build());
        }
    }
}
