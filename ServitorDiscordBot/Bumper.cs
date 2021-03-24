using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;

namespace ServitorDiscordBot
{
    class Bumper
    {
        public event Func<IEnumerable<KeyValuePair<string, DateTime>>, Task> Notify;

        class Bump
        {
            const int userBumpCooldown = 12;
            const int bumpCooldown = 4;

            private ConcurrentDictionary<string, DateTime> bumpList = new();
            public IEnumerable<KeyValuePair<string, DateTime>> BumpList
            {
                get
                {
                    foreach (var bl in bumpList.Where(x => x.Value < DateTime.Now))
                        bumpList.TryRemove(bl);

                    return bumpList;
                }
            }

            private DateTime nextBump = DateTime.Now.AddHours(bumpCooldown);
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

            public DateTime AddUser(string userName)
            {
                var curr = DateTime.Now;

                nextBump = curr.AddHours(bumpCooldown);

                var cooldown = curr.AddHours(userBumpCooldown);

                if (!bumpList.TryAdd(userName, cooldown))
                    bumpList[userName] = cooldown;

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

        public void AddUser(string userName)
        {
            timer.Stop();

            timer.Interval = (bump.AddUser(userName) - DateTime.Now).TotalMilliseconds;

            timer.Start();

            File.WriteAllText(path, JsonSerializer.Serialize(bump));
        }
    }
}
