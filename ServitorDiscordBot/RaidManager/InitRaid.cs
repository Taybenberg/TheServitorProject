using DataProcessor.DiscordEmoji;
using DataProcessor.RaidManager;
using Discord;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task InitRaidAsync(IMessage message)
        {
            var builder = GetBuilder(MessagesEnum.Raid, null, false);

            try
            {
                var raid = new RaidContainer();

                var command = message.Content.Substring(5);

                var raidType = command.Substring(0, command.IndexOf(' '));

                raid.RaidType = raidType.ToLower() switch
                {
                    "lw" or "лв" or "об" => RaidType.LW,
                    "gos" or "сп" or "сс" => RaidType.GOS,
                    "dsc" or "сгк" => RaidType.DSC,
                    "vog" or "вог" or "кс" => RaidType.VOG_L,
                    "vogm" or "вогм" or "ксм" => RaidType.VOG_M,
                    _ => throw new Exception()
                };

                command = command.Remove(0, command.IndexOf(' ') + 1);

                int index = command.IndexOf(' ');

                var date = DateTime.ParseExact((index > 0 ? command.Substring(0, index) : command), "d.M-H:m", CultureInfo.CurrentCulture);

                if (date < DateTime.Now)
                    date = date.AddYears(1);

                if (DateTime.Now.AddMonths(1) < date)
                    throw new Exception();

                raid.PlannedDate = date;

                raid.AddUser(message.Author.Id);

                raid.AddUsers(message.Author.Id, message.MentionedUserIds);

                command = Regex.Replace(command, "<@\\D?\\d+>", string.Empty);

                if (index > 0)
                    raid.Description = command.Remove(0, index + 1);

                raid.DecorateBuilder(builder);

                var msg = await message.Channel.SendMessageAsync($"<@&{_destinyRoleId}>", embed: builder.Build());

                await _raidManager.AddRaidAsync(msg.Id, raid);

                await msg.AddReactionsAsync(new string[]
                    { EmojiContainer.Check, EmojiContainer.UnCheck }
                    .Select(x => Emote.Parse(x)).ToArray());
            }
            catch
            {
                builder.Color = GetColor(MessagesEnum.Error);

                builder.Description = $"Сталася помилка під час створення рейду. Перевірте, чи формат команди коректний.\nЩоби переглянути довідку, скористайтеся командою **допомога**.";

                await message.Channel.SendMessageAsync(embed: builder.Build());
            }
        }
    }
}
