using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServitorDiscordBot
{
    partial class RaidManager : IDisposable
    {
        private readonly ILogger<ServitorBot> _logger;

        const string path = "Raids.json";

        public event Func<RaidContainer, Task> Notify;
        public event Func<RaidContainer, Task> Update;
        public event Func<ulong, Task> Delete;

        private ConcurrentDictionary<ulong, RaidContainer> Raids { get; set; } = new();

        public RaidManager(ILogger<ServitorBot> logger) => _logger = logger;

        public void Load()
        {
            if (File.Exists(path))
            {
                var raids = JsonSerializer.Deserialize<ConcurrentDictionary<ulong, RaidContainer>>(File.ReadAllText(path));

                foreach (var raid in raids.Values)
                {
                    Raids.TryAdd(raid.ID, raid);

                    raid.Notify += Raid_Notify;
                    raid.Update += Raid_Update;
                    raid.Delete += Raid_Delete;

                    raid.Start();
                }

                _logger.LogInformation($"{DateTime.Now} {Raids.Count} Raids scheduled");
            }
        }

        public void Backup()
        {
            File.WriteAllText(path, JsonSerializer.Serialize(Raids));
        }

        public RaidContainer this[ulong messageID]
        {
            get
            {
                if (Raids.ContainsKey(messageID))
                    return Raids[messageID];

                return null;
            }
        }

        public async Task AddRaidAsync(ulong messageID, RaidContainer raid)
        {
            raid.ID = messageID;

            Raids.TryAdd(messageID, raid);

            raid.Notify += Raid_Notify;
            raid.Update += Raid_Update;
            raid.Delete += Raid_Delete;

            raid.Start();

            _logger.LogInformation($"{DateTime.Now} New raid scheduled on {raid.PlannedDate}");

            Backup();
        }

        public async Task TryRemoveRaid(ulong messageID)
        {
            if (Raids.ContainsKey(messageID))
            {
                Raids[messageID].Stop();
            }
        }

        private async Task Raid_Delete(ulong id)
        {
            Raids[id].Dispose();

            Raids.TryRemove(id, out _);

            Delete?.Invoke(id);

            _logger.LogInformation($"{DateTime.Now} Raid {id} removed");

            Backup();
        }

        private async Task Raid_Update(ulong id)
        {
            Update?.Invoke(Raids[id]);

            _logger.LogInformation($"{DateTime.Now} Raid {id} updated");

            Backup();
        }

        private async Task Raid_Notify(ulong id)
        {
            Notify?.Invoke(Raids[id]);

            _logger.LogInformation($"{DateTime.Now} Raid {id} notification");
        }

        public void Dispose()
        {
            foreach (var raid in Raids.Values)
                raid.Dispose();
        }
    }
}
