using Discord;

namespace ServitorDiscordBot.BotCommands.TextCommands
{
    internal partial class ServiceCommandsManager
    {
        private async Task CommandRandom(IMessage message)
        {
            try
            {
                if (message.MentionedRoleIds.Count == 1)
                {
                    var roleID = message.MentionedRoleIds.First();

                    var members = _client.GetGuild((message.Channel as IGuildChannel).GuildId).GetRole(roleID).Members;

                    var count = members.Count();
                    var member = members.ElementAt(new Random().Next(count));

                    var builder = new EmbedBuilder()
                        .WithColor(0xA5D6A7)
                        .WithDescription($"Серед {count} користувачів моє око побачило {member.Mention} {CommonData.DiscordEmoji.Emoji.ServitorIlluminati}");

                    await message.Channel.SendMessageAsync(embed: builder.Build());
                }
                else
                {
                    var strs = message.Content.Split(' ');

                    if (strs.Length == 2 && uint.TryParse(strs[1], out var next))
                    {
                        var builder = new EmbedBuilder()
                            .WithColor(0xA5D6A7)
                            .WithDescription($"Віщую вам число **{new Random().Next((int)next)}** {CommonData.DiscordEmoji.Emoji.ServitorIlluminati}");

                        await message.Channel.SendMessageAsync(embed: builder.Build());
                    }
                }
            }
            catch { }
        }
    }
}
