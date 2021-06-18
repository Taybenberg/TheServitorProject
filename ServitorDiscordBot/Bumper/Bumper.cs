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
    class Bumper : IDisposable
    {
        const int bumpPreIntervalMs = 10000;

        public event Func<Dictionary<string, DateTime>, Task> Notify;

        class Bump
        {
            const int userBumpCooldownHours = 12;
            const int bumpCooldownHours = 4;

            public ConcurrentDictionary<string, DateTime> bumpList { get; set; } = new();

            [JsonIgnore]
            public Dictionary<string, DateTime> BumpList
            {
                get
                {
                    foreach (var bl in bumpList.Where(x => x.Value < DateTime.Now))
                        bumpList.TryRemove(bl);

                    return bumpList.ToDictionary(x => x.Key, x => x.Value);
                }
            }

            public DateTime nextBump { get; set; } = DateTime.Now.AddHours(bumpCooldownHours);

            [JsonIgnore]
            public DateTime NextBump
            {
                get
                {
                    var curr = DateTime.Now;

                    if (nextBump < curr)
                        nextBump = curr.AddHours(bumpCooldownHours);

                    return nextBump;
                }
            }

            public DateTime AddUser(string userID)
            {
                var curr = DateTime.Now;

                nextBump = curr.AddHours(bumpCooldownHours);

                var cooldown = curr.AddHours(userBumpCooldownHours);

                if (!bumpList.TryAdd(userID, cooldown))
                    bumpList[userID] = cooldown;

                return nextBump;
            }
        }

        private Bump _bump;
        Timer _timer = new();

        const string path = "Bump.json";

        public DateTime NextBump { get { return _bump.NextBump; } }

        public Bumper()
        {
            if (File.Exists(path))
                _bump = JsonSerializer.Deserialize<Bump>(File.ReadAllText(path));
            else
                _bump = new();

            _timer.AutoReset = false;

            _timer.Interval = (_bump.NextBump - DateTime.Now).TotalMilliseconds - bumpPreIntervalMs;

            _timer.Elapsed += (_, _) =>
            {
                _timer.Stop();

                Notify?.Invoke(_bump.BumpList);
            };

            _timer.Start();
        }

        public void AddUser(string userID)
        {
            _timer.Stop();

            _timer.Interval = (_bump.AddUser(userID) - DateTime.Now).TotalMilliseconds - bumpPreIntervalMs;

            _timer.Start();

            File.WriteAllText(path, JsonSerializer.Serialize(_bump));
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}
