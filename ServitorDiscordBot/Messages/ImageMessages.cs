using Discord;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        public async Task GetEververseInventoryAsync(IMessageChannel channel, string week = null)
        {
            int currWeek = 0;
            int.TryParse(week, out currWeek);

            if (currWeek < 1 || currWeek > 15)
                currWeek = (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

            var parser = getFactory().GetEververseParser(_seasonName, _seasonStart, currWeek);

            using var inventory = await parser.GetImageAsync();

            await channel.SendFileAsync(inventory, "EververseInventory.png");
        }

        private async Task GetResourcesPoolAsync(IMessageChannel channel)
        {
            var parser = getFactory().GetResourcesParser();

            using var resources = await parser.GetImageAsync();

            await channel.SendFileAsync(resources, "ResourcesPool.png");
        }

        private async Task GetLostSectorsLootAsync(IMessageChannel channel)
        {
            var parser = getFactory().GetLostSectorsParser();

            using var sectors = await parser.GetImageAsync();

            await channel.SendFileAsync(sectors, "LostSectorsLoot.png");
        }

        private ConcurrentDictionary<ulong, ulong> osirisInventory = new();
        private async Task GetOsirisInventoryAsync(IMessageChannel channel)
        {
            var parser = getFactory().GetOsirisParser();

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

        private ConcurrentDictionary<ulong, ulong> xurInventory = new();
        private async Task GetXurInventoryAsync(IMessageChannel channel, bool getLocation = true)
        {
            var parser = getFactory().GetXurParser(getLocation);

            using var inventory = await parser.GetImageAsync();

            var message = await channel.SendFileAsync(inventory, "XurInventory.png");

            if (!xurInventory.TryAdd(channel.Id, message.Id))
            {
                var ch = _client.GetChannel(channel.Id) as IMessageChannel;

                var msg = await ch.GetMessageAsync(xurInventory[channel.Id]);

                try
                {
                    await msg.DeleteAsync();
                }
                catch { }

                xurInventory[channel.Id] = message.Id;
            }
        }
    }
}
