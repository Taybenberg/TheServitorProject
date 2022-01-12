using BungieNetApi;
using ClanActivitiesDatabase.ORM;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ClanActivitiesDatabase
{
    public partial class ClanActivitiesUoW
    {
        public async Task SyncUsersAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Users");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            ConcurrentBag<IEnumerable<BungieNetApi.Entities.User>> clanUsersCollection = new();

            Parallel.ForEach(_configuration.GetSection("Destiny2:ClanIDs").Get<HashSet<long>>(), (clanID) =>
            {
                clanUsersCollection.Add(apiClient.GetClan(clanID).GetUsersAsync().Result);
            });

            var clanUsers = clanUsersCollection.SelectMany(x => x).ToDictionary(x => x.MembershipID, x => x);

            var dbUsers = await _context.Users.Include(c => c.Characters).ToDictionaryAsync(x => x.UserID, x => x);

            ConcurrentBag<User> newUsers = new();
            ConcurrentBag<User> updUsers = new();

            ConcurrentBag<Character> diffChars = new();
            ConcurrentBag<Character> newChars = new();
            ConcurrentBag<Character> updChars = new();

            var diffDbUsers = dbUsers.Where(x => !clanUsers.ContainsKey(x.Key)).Select(x => x.Value);

            Parallel.ForEach(clanUsers, (usr) =>
            {
                var dbUsr = dbUsers.GetValueOrDefault(usr.Key);

                if (dbUsr is null)
                {
                    newUsers.Add(new User
                    {
                        UserID = usr.Value.MembershipID,
                        ClanID = usr.Value.ClanID,
                        UserName = usr.Value.BungieName,
                        DateLastPlayed = usr.Value.DateLastPlayed,
                        ClanJoinDate = usr.Value.ClanJoinDate,
                        MembershipType = usr.Value.MembershipType,
                        Characters = usr.Value.Characters.Select(chr =>
                        new Character
                        {
                            CharacterID = chr.CharacterID,
                            DateLastPlayed = chr.DateLastPlayed,
                            Class = chr.Class,
                            Race = chr.Race,
                            Gender = chr.Gender,
                            UserID = chr.MembershipID
                        }).ToList()
                    });
                }
                else if (dbUsr.DateLastPlayed < usr.Value.DateLastPlayed)
                {
                    dbUsr.ClanID = usr.Value.ClanID;
                    dbUsr.UserName = usr.Value.BungieName;
                    dbUsr.DateLastPlayed = usr.Value.DateLastPlayed;
                    dbUsr.MembershipType = usr.Value.MembershipType;

                    updUsers.Add(dbUsr);

                    foreach (var diff in dbUsr.Characters.Where(x => !usr.Value.Characters.Any(y => y.CharacterID == x.CharacterID)))
                    {
                        diffChars.Add(diff);
                    }

                    foreach (var chr in usr.Value.Characters)
                    {
                        var dbChr = dbUsr.Characters.FirstOrDefault(x => x.CharacterID == chr.CharacterID);

                        if (dbChr is null)
                        {
                            newChars.Add(new Character
                            {
                                CharacterID = chr.CharacterID,
                                DateLastPlayed = chr.DateLastPlayed,
                                Class = chr.Class,
                                Race = chr.Race,
                                Gender = chr.Gender,
                                UserID = chr.MembershipID
                            });
                        }
                        else
                        {
                            dbChr.DateLastPlayed = chr.DateLastPlayed;
                            dbChr.Class = chr.Class;
                            dbChr.Race = chr.Race;
                            dbChr.Gender = chr.Gender;

                            updChars.Add(dbChr);
                        }
                    }
                }
            });

            _context.Users.RemoveRange(diffDbUsers);
            _context.Users.AddRange(newUsers);
            _context.Users.UpdateRange(updUsers);

            _context.Characters.RemoveRange(diffChars);
            _context.Characters.AddRange(newChars);
            _context.Characters.UpdateRange(updChars);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now} Users synced");
        }
    }
}
