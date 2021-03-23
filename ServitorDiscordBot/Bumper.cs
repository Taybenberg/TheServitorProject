using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

            public DateTime nextBump = DateTime.Now.AddHours(bumpCooldown);
            public ConcurrentDictionary<string, DateTime> bumpList = new();

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

        static Bump bump = new();
        Timer timer = new();

        public Bumper()
        {
            timer.AutoReset = false;
            timer.Interval = (bump.nextBump - DateTime.Now).TotalMilliseconds;

            timer.Elapsed += (_, _) =>
            {
                timer.Stop();

                foreach (var bl in bump.bumpList.Where(x => x.Value < DateTime.Now))
                    bump.bumpList.TryRemove(bl);

                Notify?.Invoke(bump.bumpList);
            };

            timer.Start();
        }

        public void AddUser(string userName)
        {
            timer.Stop();

            timer.Interval = (bump.AddUser(userName) - DateTime.Now).TotalMilliseconds;

            timer.Start();
        }
    }
}
