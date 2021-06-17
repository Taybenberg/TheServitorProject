using Discord;
using Extensions;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task GetEververseInventoryAsync(IMessageChannel channel = null, string week = null)
        {
            int currWeek = 0;
            int.TryParse(week, out currWeek);

            if (currWeek < 1 || currWeek > 15)
                currWeek = (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

            using var inventory = await EververseParser.GetEververseInventoryAsync(_seasonName, _seasonStart, currWeek);

            channel ??= _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendFileAsync(inventory, "EververseInventory.png");
        }

        private async Task GetResourcesPoolAsync(IMessageChannel channel = null)
        {
            using var resources = await ResourcesParser.GetResourcesAsync();

            channel ??= _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendFileAsync(resources, "ResourcesPool.png");
        }

        private async Task GetLostSectorsLootAsync(IMessageChannel channel = null)
        {
            using var sectors = await LostSectorsParser.GetLostSectorsAsync();

            channel ??= _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendFileAsync(sectors, "LostSectorsLoot.png");
        }

        private ConcurrentDictionary<ulong, ulong> osirisInventory = new();
        private async Task GetOsirisInventoryAsync(IMessageChannel channel)
        {
            using var inventory = await TrialsOfOsirisParser.GetOsirisInventoryAsync();

            var message = await channel.SendFileAsync(inventory, "OsirisInventory.png");

            if (!osirisInventory.TryAdd(channel.Id, message.Id))
            {
                var ch = _client.GetChannel(channel.Id) as IMessageChannel;

                var msg = await ch.GetMessageAsync(osirisInventory[channel.Id]);
                await msg.DeleteAsync();

                osirisInventory[channel.Id] = message.Id;
            }
        }
    }
}
