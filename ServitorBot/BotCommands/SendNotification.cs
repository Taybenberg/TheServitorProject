using DestinyInfocardsService;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ServitorBot.BotCommands;

namespace ServitorBot
{
    public partial class ServitorDiscordBot
    {
        public async Task SendNotificationAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Servitor reset notification");

            using var scope = _scopeFactory.CreateScope();

            var destinyInfocards = scope.ServiceProvider.GetRequiredService<IDestinyInfocards>();

            var sectors = await destinyInfocards.GetLostSectorsInfocardAsync();
            var infocard = InfocardHelper.ParseInfocard(sectors).Build();

            foreach (var channeldID in _mainChannelIDs)
            {
                var channel = _client.GetChannel(channeldID) as IMessageChannel;

                await channel.SendMessageAsync(embed: infocard);
            }
        }
    }
}
