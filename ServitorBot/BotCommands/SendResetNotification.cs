using DestinyInfocardsService;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServitorBot.BotCommands;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        public async Task SendResetNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Servitor reset notification");

            using var scope = _scopeFactory.CreateScope();

            var destinyInfocards = scope.ServiceProvider.GetRequiredService<IDestinyInfocards>();

            List<Task<Embed>> tasks = new();

            var currDate = DateTime.UtcNow;

            if (currDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var infocard = await destinyInfocards.GetEververseInfocardAsync();

                    return InfocardHelper.ParseInfocard(infocard).Build();
                }));
            }

            tasks.Add(Task.Run(async () =>
            {
                var infocard = await destinyInfocards.GetLostSectorsInfocardAsync();

                return InfocardHelper.ParseInfocard(infocard).Build();
            }));

            tasks.Add(Task.Run(async () =>
            {
                var infocard = await destinyInfocards.GetResourcesInfocardAsync();

                return InfocardHelper.ParseInfocard(infocard).Build();
            }));

            var embeds = await Task.WhenAll(tasks);

            foreach (var channeldID in _mainChannelIDs)
            {
                var channel = _client.GetChannel(channeldID) as IMessageChannel;

                await channel.SendMessageAsync(embeds: embeds);
            }
        }
    }
}
