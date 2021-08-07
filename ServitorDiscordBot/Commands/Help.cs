using Discord;
using System.Threading.Tasks;
using static ServitorDiscordBot.MessagesEnum;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetHelpAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.Help, message);

            builder.Author = new();

            var g = (message.Channel as IGuildChannel).Guild;

            builder.Author.IconUrl = g.IconUrl;
            builder.Author.Name = $"На варті спільноти {g.Name} з 10.02.2021";

            builder.Description = $"Я **{_client.CurrentUser.Username}**, " +
                $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                $"Наразі Авангард надав мені роль обчислювальної машини у H.E.L.M. для збору статистичних даних про діяльність вашого [клану]({_clanUrl}).\n" +
                $"\n**Перелік доступних команд** (для перегляду детальної довідки по команді введіть **допомога %команда%**):\n" +

                $"\n**{messageCommands[Bip][0]}** - запит на перевірку моєї працездатності\n" +

                $"\n**{messageCommands[Weekly][0]}** - переглянути інформацію про поточний тиждень\n" +

                $"\n**{messageCommands[Sectors][0]}** - переглянути лутпул сьогоднішніх загублених секторів\n" +

                $"\n**{messageCommands[Resources][0]}** - переглянути поточний асортимент вендорів\n" +

                $"\n**{messageCommands[Xur][0]}** - переглянути інвентар Зура\n" +

                $"\n**{messageCommands[Osiris][0]}** - переглянути нагороди за випробування Осіріса\n" +

                $"\n**{messageCommands[Eververse][0]}** - переглянути поточний асортимент Тесс Еверіс\n" +

                $"\n**{messageCommands[Eververse][0]} %тиждень%** - переглянути асортимент Тесс Еверіс за визначений тиждень (1-15)\n" +

                $"\n**{messageCommands[EververseAll][0]}** - переглянути весь сезонний асортимент Тесс Еверіс\n" +

                $"\n**{messageCommands[MyGrandmasters][0]}** - переглянути закриті ґардіаном найтфоли складності грандмайстер\n" +

                $"\n**{messageCommands[MyRaids][0]}** - переглянути закриті ґардіаном рейди цього тижня\n" +

                $"\n**{messageCommands[MyActivities][0]}** - кількість активностей ґардіана у цьому році\n" +

                $"\n**{messageCommands[MyPartners][0]}** - список побратимів ґардіана\n" +

                $"\n**{messageCommands[ClanActivities][0]}** - кількість активностей клану в цьому році\n" +

                $"\n**{messageCommands[Modes][0]}** - список типів активностей\n" +

                $"\n**{messageCommands[ClanStats][0]} %режим%** - агрегована статистика клану в типі активності\n" +

                $"\n**{messageCommands[Leaderboard][0]} %режим%** - список лідерів у типі активності\n" +

                $"\n**{messageCommands[Apostates][0]}** - виявити потенційно небезпечні активності окрім нальотів\n" +

                $"\n**{messageCommands[_100K][0]}** - виявити потенційно небезпечні нальоти з сумою очок більше 100К\n" +

                $"\n**{messageCommands[Register][0]}** - прив'язати акаунт Destiny 2 до профілю в Discord";

            await message.Channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
