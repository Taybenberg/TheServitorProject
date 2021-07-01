using BungieNetApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database
{
    public partial class ClanDatabase
    {
        public async Task SyncUsersAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Users");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<IApiClient>();

            var clanUsers = (await apiClient.Clan.GetUsersAsync()).ToDictionary(x => x.MembershipID, x => x);

            var dbUsers = await Users.Include("Characters").ToDictionaryAsync(x => x.UserID, x => x);

            ConcurrentBag<User> newUsers = new();
            ConcurrentBag<User> updUsers = new();

            ConcurrentBag<Character> diffChars = new();
            ConcurrentBag<Character> newChars = new();
            ConcurrentBag<Character> updChars = new();

            var diffDbUsers = dbUsers.Where(x => !clanUsers.ContainsKey(x.Key)).Select(x => x.Value);

            Parallel.ForEach(clanUsers, new ParallelOptions { MaxDegreeOfParallelism = 3 }, (usr) =>
            {
                var dbUsr = dbUsers.GetValueOrDefault(usr.Key);

                if (dbUsr is null)
                {
                    newUsers.Add(new User
                    {
                        UserID = usr.Value.MembershipID,
                        UserName = usr.Value.LastSeenDisplayName,
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
                    dbUsr.UserName = usr.Value.LastSeenDisplayName;
                    dbUsr.DateLastPlayed = usr.Value.DateLastPlayed;
                    dbUsr.ClanJoinDate = usr.Value.ClanJoinDate;
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

            Users.RemoveRange(diffDbUsers);
            Users.AddRange(newUsers);
            Users.UpdateRange(updUsers);

            Characters.RemoveRange(diffChars);
            Characters.AddRange(newChars);
            Characters.UpdateRange(updChars);

            await SaveChangesAsync();

            _logger.LogInformation($"{DateTime.Now} Users synced");
        }
    }
}
