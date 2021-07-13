using Discord;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetEververseInventoryAsync(IMessageChannel channel, string week = null)
        {
            int currWeek = 0;
            int.TryParse(week, out currWeek);

            if (currWeek < 1 || currWeek > 15)
                currWeek = (int)(DateTime.Now - _seasonStart).TotalDays / 7 + 1;

            using var inventory = await getImageFactory().GetEververseAsync(_seasonName, _seasonStart, currWeek);

            await channel.SendFileAsync(inventory, "EververseInventory.png");
        }

        private async Task GetRoadmapAsync(IMessageChannel channel)
        {
            using var roadmap = await getImageFactory().GetRoadmapAsync();

            if (roadmap is not null)
                await channel.SendFileAsync(roadmap, "Roadmap.png");
        }

        private async Task GetResourcesPoolAsync(IMessageChannel channel)
        {
            using var resources = await getImageFactory().GetResourcesAsync();

            await channel.SendFileAsync(resources, "ResourcesPool.png");
        }

        private async Task GetLostSectorsLootAsync(IMessageChannel channel)
        {
            using var sectors = await getImageFactory().GetLostSectorsAsync();

            await channel.SendFileAsync(sectors, "LostSectorsLoot.png");
        }

        private ConcurrentDictionary<ulong, ulong> osirisInventory = new();
        private async Task GetOsirisInventoryAsync(IMessageChannel channel)
        {
            using var inventory = await getImageFactory().GetOsirisAsync();

            var msg = await channel.SendFileAsync(inventory, "OsirisInventory.png");

            await antiFlood(osirisInventory, channel.Id, msg.Id);
        }

        private ConcurrentDictionary<ulong, ulong> xurInventory = new();
        private async Task GetXurInventoryAsync(IMessageChannel channel, bool getLocation = true)
        {
            using var inventory = await getImageFactory().GetXurAsync(getLocation);

            var msg = await channel.SendFileAsync(inventory, "XurInventory.png");

            await antiFlood(xurInventory, channel.Id, msg.Id);
        }

        private async Task antiFlood(ConcurrentDictionary<ulong, ulong> dictionary, ulong channelID, ulong messageID)
        {
            if (!dictionary.TryAdd(channelID, messageID))
            {
                var ch = _client.GetChannel(channelID) as IMessageChannel;

                var msg = await ch.GetMessageAsync(xurInventory[channelID]);

                try
                {
                    await msg.DeleteAsync();
                }
                catch { }

                dictionary[channelID] = messageID;
            }
        }
    }
}
