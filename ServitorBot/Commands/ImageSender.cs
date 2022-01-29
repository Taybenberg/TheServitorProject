using Discord;
using System.Collections.Concurrent;

namespace ServitorDiscordBot
{
    public partial class ServitorBot
    {
        private async Task GetDailyResetAsync(IMessageChannel channel)
        {
            /*
            using var reset = await getImageFactory().GetDailyResetAsync(_seasonName, GetWeekNumber());

            await channel.SendFileAsync(reset, "DailyReset.png");
            */
        }

        private async Task GetWeeklyResetAsync(IMessageChannel channel)
        {
            /*
            using var reset = await getImageFactory().GetWeeklyResetAsync(_seasonName, _seasonStart, GetWeekNumber());

            await channel.SendFileAsync(reset, "WeeklyReset.png");
            */
        }

        private async Task GetEververseFullInventoryAsync(IMessageChannel channel)
        {
            /*
            using var inventory = await getImageFactory().GetEververseFullAsync(_seasonName, _seasonStart, _seasonEnd);

            await channel.SendFileAsync(inventory, "EververseFullInventory.png");
            */
        }

        private async Task GetEververseInventoryAsync(IMessageChannel channel, string week = null)
        {
            /*
            int currWeek = 0;
            int.TryParse(week, out currWeek);

            if (currWeek < 16 || ((_seasonEnd - _seasonStart).TotalDays / 7 + 1) < currWeek)
                currWeek = GetWeekNumber();

            using var inventory = await getImageFactory().GetEververseAsync(_seasonName, _seasonStart, currWeek);

            await channel.SendFileAsync(inventory, "EververseInventory.png");
            */
        }

        private async Task GetResourcesPoolAsync(IMessageChannel channel)
        {
            /*
            using var resources = await getImageFactory().GetResourcesAsync();

            await channel.SendFileAsync(resources, "ResourcesPool.png");
            */
        }

        private async Task GetLostSectorsLootAsync(IMessageChannel channel)
        {
            /*
            using var sectors = await getImageFactory().GetLostSectorsAsync();

            await channel.SendFileAsync(sectors, "LostSectorsLoot.png");
            */
        }

        private async Task GetXurInventoryAsync(IMessageChannel channel, bool getLocation = true)
        {
            /*
            using var inventory = await getImageFactory().GetXurAsync(getLocation);

            var msg = await channel.SendFileAsync(inventory, "XurInventory.png");
            */
        }
    }
}
