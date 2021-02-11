using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public partial class ClanDatabase
    {
        public async Task SyncUserRelationsAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing UserRelations");

            DateTime currDay = DateTime.Now.Date;

            var users = await Users.Include(u => u.Characters).ToListAsync();

            var activities = await Activities.Include(s => s.ActivityUserStats).Where(a => currDay.AddDays(-1) < a.Period && a.Period <= currDay).ToListAsync();

            var existingRelations = await UserRelations.ToListAsync();

            ConcurrentBag<UserRelations> newRelations = new();
            ConcurrentBag<UserRelations> updRelations = new();

            Parallel.For(0, users.Count, (i) =>
            {
                var userActivities = users[i].Characters.SelectMany(c => activities.Where(a => a.ActivityUserStats.Where(s => s.CharacterID == c.CharacterID).Any())).Distinct();

                var relation = existingRelations.FirstOrDefault(x => x.User1ID == users[i].UserID && x.User2ID == null);

                if (relation is null)
                {
                    newRelations.Add(new UserRelations
                    {
                        User1ID = users[i].UserID,
                        User2ID = null,
                        Count = userActivities.Count()
                    });
                }
                else
                {
                    relation.Count += userActivities.Count();
                    updRelations.Add(relation);
                }

                Parallel.For(i + 1, users.Count, (j) =>
                {
                    var pairedActivities = users[j].Characters.SelectMany(c => userActivities.Where(a => a.ActivityUserStats.Where(s => s.CharacterID == c.CharacterID).Any())).Distinct();

                    var relations = existingRelations.FirstOrDefault(x => x.User1ID == users[i].UserID && x.User2ID == users[j].UserID);

                    if (relations is null)
                    {
                        newRelations.Add(new UserRelations
                        {
                            User1ID = users[i].UserID,
                            User2ID = users[j].UserID,
                            Count = pairedActivities.Count()
                        });
                    }
                    else
                    {
                        relations.Count += pairedActivities.Count();
                        updRelations.Add(relation);
                    }

                    relations = existingRelations.FirstOrDefault(x => x.User1ID == users[j].UserID && x.User2ID == users[i].UserID);

                    if (relations is null)
                    {
                        newRelations.Add(new UserRelations
                        {
                            User1ID = users[j].UserID,
                            User2ID = users[i].UserID,
                            Count = pairedActivities.Count()
                        });
                    }
                    else
                    {
                        relations.Count += pairedActivities.Count();
                        updRelations.Add(relation);
                    }
                });
            });

            UserRelations.AddRange(newRelations);
            UserRelations.UpdateRange(updRelations);

            await SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now} UserRelations synced");
        }
    }
}
