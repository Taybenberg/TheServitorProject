using Discord;
using Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetHelpMessageAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Help);

            builder.Author = new();

            var g = (channel as IGuildChannel).Guild;

            builder.Author.IconUrl = g.IconUrl;
            builder.Author.Name = $"На варті спільноти {g.Name}";

            builder.Title = "Допомога";
            builder.Description = $"Я **{_client.CurrentUser.Username}**, " +
                $"дружній прислужник, якого на околицях сонячної системи підібрав відважний ґардіан. " +
                $"Я не становлю загрози і присягаюсь служити на благо Останнього міста. " +
                $"Наразі Авангард надав мені роль обчислювальної машини у H.E.L.M. для збору статистичних даних про діяльність вашого [клану]({_clanUrl}).\n" +
                $"\n**Зараз я вмію виконувати наступні функції:**\n" +
                $"\n***біп*** - *запит на перевірку моєї працездатності*\n" +
                $"\n***тиждень*** - *переглянути інформацію про поточний тиждень*\n" +
                $"\n***сектори*** - *переглянути лутпул сьогоднішніх загублених секторів*\n" +
                $"\n***ресурси*** - *переглянути поточний асортимент вендорів*\n" +
                $"\n***зур*** - *переглянути інвентар Зура*\n" +
                $"\n***осіріс*** - *переглянути нагороди за випробування Осіріса*\n" +
                $"\n***еверверс*** - *переглянути поточний асортимент Тесс Еверіс*\n" +
                $"\n***еверверс %тиждень%*** - *переглянути асортимент Тесс Еверіс за визначений тиждень (1-15)*\n" +
                $"\n***мої активності*** - *кількість активностей ґардіана у цьому році*\n" +
                $"\n***мої побратими*** - *список побратимів ґардіана*\n" +
                $"\n***кланові активності*** - *кількість активностей клану в цьому році*\n" +
                $"\n***режими*** - *список типів активностей*\n" +
                $"\n***статистика клану %режим%*** - *агрегована статистика клану в типі активності*\n" +
                $"\n***дошка лідерів %режим%*** - *список лідерів у типі активності*\n" +
                $"\n***відступники*** - *виявити потенційно небезпечні активності окрім нальотів*\n" +
                $"\n***100K*** - *виявити потенційно небезпечні нальоти з сумою очок більше 100К*\n" +
                $"\n***реєстрація*** - *прив'язати акаунт Destiny 2 до профілю в Discord*";

            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task GetBipMessageAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Bip);

            builder.Description = "біп…";

            await channel.SendMessageAsync(embed: builder.Build());
        }

        private async Task GetModesAsync(IMessageChannel channel)
        {
            var builder = new EmbedBuilder();

            builder.Color = GetColor(MessagesEnum.Modes);

            builder.Title = $"Режими";

            builder.Description = string.Empty;

            foreach (var mode in Localization.StatsActivityNames.Values.OrderBy(x => x[0]))
                builder.Description += $"**{mode[0]}** | {mode[1]}\n";

            builder.Footer = GetFooter();

            await channel.SendMessageAsync(embed: builder.Build());
        }
    }
}
