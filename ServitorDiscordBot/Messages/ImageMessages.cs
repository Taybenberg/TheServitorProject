using Discord;
using Extensions.Inventory;
using Extensions.Parsers;
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

            IInventoryParser<EververseInventory> parser = new EververseParser(_seasonName, _seasonStart, currWeek);

            using var inventory = await parser.GetImageAsync();

            channel ??= _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendFileAsync(inventory, "EververseInventory.png");
        }

        private async Task GetResourcesPoolAsync(IMessageChannel channel = null)
        {
            IInventoryParser<ResourcesInventory> parser = new ResourcesParser();

            using var resources = await parser.GetImageAsync();

            channel ??= _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendFileAsync(resources, "ResourcesPool.png");
        }

        private async Task GetLostSectorsLootAsync(IMessageChannel channel = null)
        {
            IInventoryParser<LostSectorsInventory> parser = new LostSectorsParser();

            using var sectors = await parser.GetImageAsync();

            channel ??= _client.GetChannel(_channelId[0]) as IMessageChannel;

            await channel.SendFileAsync(sectors, "LostSectorsLoot.png");
        }

        private ConcurrentDictionary<ulong, ulong> osirisInventory = new();
        private async Task GetOsirisInventoryAsync(IMessageChannel channel)
        {
            IInventoryParser<OsirisInventory> parser = new TrialsOfOsirisParser();

            using var inventory = await parser.GetImageAsync();

            var message = await channel.SendFileAsync(inventory, "OsirisInventory.png");

            if (!osirisInventory.TryAdd(channel.Id, message.Id))
            {
                var ch = _client.GetChannel(channel.Id) as IMessageChannel;

                var msg = await ch.GetMessageAsync(osirisInventory[channel.Id]);

                try
                {
                    await msg.DeleteAsync();
                }
                catch (Exception) { }

                osirisInventory[channel.Id] = message.Id;
            }
        }
    }
}
