using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;

namespace ServitorDiscordBot
{
    class Bumper
    {
        public event Func<Dictionary<ulong, (string, DateTime)>, Task> Notify;

        class Bump
        {
            public record BumpValue
            {
                public string Username { get; set; }
                public DateTime Cooldown { get; set; }
            }

            const int userBumpCooldown = 12;
            const int bumpCooldown = 4;

            public ConcurrentDictionary<ulong, BumpValue> bumpList { get; set; } = new();

            [JsonIgnore]
            public Dictionary<ulong, (string, DateTime)> BumpList
            {
                get
                {
                    foreach (var bl in bumpList.Where(x => x.Value.Cooldown < DateTime.Now))
                        bumpList.TryRemove(bl);

                    return bumpList.ToDictionary(x => x.Key, x => (x.Value.Username, x.Value.Cooldown));
                }
            }

            public DateTime nextBump { get; set; } = DateTime.Now.AddHours(bumpCooldown);

            [JsonIgnore]
            public DateTime NextBump
            {
                get
                {
                    var curr = DateTime.Now;

                    if (nextBump < curr)
                        nextBump = curr.AddHours(bumpCooldown);

                    return nextBump;
                }
            }

            public DateTime AddUser(ulong userID, string userName)
            {
                var curr = DateTime.Now;

                nextBump = curr.AddHours(bumpCooldown);

                var cooldown = curr.AddHours(userBumpCooldown);

                if (!bumpList.TryAdd(userID, new BumpValue
                {
                    Username = userName,
                    Cooldown = cooldown
                }))
                    bumpList[userID].Cooldown = cooldown;

                return nextBump;
            }
        }

        Bump bump;
        Timer timer = new();

        const string path = "Bump.json";

        public DateTime NextBump { get { return bump.NextBump; } }

        public Bumper()
        {
            if (File.Exists(path))
                bump = JsonSerializer.Deserialize<Bump>(File.ReadAllText(path));
            else
                bump = new();

            timer.AutoReset = false;

            timer.Interval = (bump.NextBump - DateTime.Now).TotalMilliseconds;

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();

                Notify?.Invoke(bump.BumpList);
            };

            timer.Start();
        }

        public void AddUser(ulong userID, string userName)
        {
            timer.Stop();

            timer.Interval = (bump.AddUser(userID, userName) - DateTime.Now).TotalMilliseconds;

            timer.Start();

            File.WriteAllText(path, JsonSerializer.Serialize(bump));
        }
    }
}
