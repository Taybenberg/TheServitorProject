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
        public async Task SyncUsersAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Users");

            var usersBuffer = await _apiClient.GetUsersAsync();

            var dbUsers = await Users.Include("Characters").ToListAsync();

            var diffDbUsers = dbUsers.Where(x => !usersBuffer.Any(y => y.Key == x.UserID));
            Users.RemoveRange(diffDbUsers);

            var dbRelations = await UserRelations.ToListAsync();
            var diffRelations = dbRelations.Where(x => diffDbUsers.Any(y => y.UserID == x.User1ID || y.UserID == x.User2ID));
            UserRelations.RemoveRange(diffRelations);

            ConcurrentBag<User> newUsers = new();
            ConcurrentBag<User> updUsers = new();

            ConcurrentBag<Character> diffChars = new();
            ConcurrentBag<Character> newChars = new();
            ConcurrentBag<Character> updChars = new();

            Parallel.ForEach(usersBuffer, (usr) =>
            {
                var dbUsr = dbUsers.FirstOrDefault(x => x.UserID == usr.Key);

                if (dbUsr is null)
                {
                    newUsers.Add(new User
                    {
                        UserID = usr.Key,
                        UserName = usr.Value.LastSeenDisplayName,
                        DateLastPlayed = usr.Value.DateLastPlayed,
                        ClanJoinDate = usr.Value.ClanJoinDate,
                        MembershipType = usr.Value.MembershipType,
                        Characters = usr.Value.Characters.Select(chr =>
                        new Character
                        {
                            CharacterID = chr.CharacterId,
                            DateLastPlayed = chr.DateLastPlayed,
                            Class = chr.Class,
                            Race = chr.Race,
                            Gender = chr.Gender,
                            UserID = chr.MembershipId
                        }).ToList()
                    });
                }
                else
                {
                    dbUsr.UserID = usr.Key;
                    dbUsr.UserName = usr.Value.LastSeenDisplayName;
                    dbUsr.DateLastPlayed = usr.Value.DateLastPlayed;
                    dbUsr.ClanJoinDate = usr.Value.ClanJoinDate;
                    dbUsr.MembershipType = usr.Value.MembershipType;

                    updUsers.Add(dbUsr);

                    foreach (var diff in dbUsr.Characters.Where(x => !usr.Value.Characters.Any(y => y.CharacterId == x.CharacterID)))
                    {
                        diffChars.Add(diff);
                    }

                    foreach (var chr in usr.Value.Characters)
                    {
                        var dbChr = dbUsr.Characters.FirstOrDefault(x => x.CharacterID == chr.CharacterId);

                        if (dbChr is null)
                        {
                            newChars.Add(new Character
                            {
                                CharacterID = chr.CharacterId,
                                DateLastPlayed = chr.DateLastPlayed,
                                Class = chr.Class,
                                Race = chr.Race,
                                Gender = chr.Gender,
                                UserID = chr.MembershipId
                            });
                        }
                        else
                        {
                            dbChr.CharacterID = chr.CharacterId;
                            dbChr.DateLastPlayed = chr.DateLastPlayed;
                            dbChr.Class = chr.Class;
                            dbChr.Race = chr.Race;
                            dbChr.Gender = chr.Gender;
                            dbChr.UserID = chr.MembershipId;

                            updChars.Add(dbChr);
                        }
                    }
                }
            });

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
