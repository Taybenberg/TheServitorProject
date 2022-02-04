using BungieSharper.Client;
using BungieSharper.Entities.Destiny;
using ClanActivitiesDatabase;
using ClanActivitiesDatabase.ORM;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ClanActivitiesService
{
    public partial class ClanActivitiesManager
    {
        public async Task SyncClanUsersAsync()
        {
            _logger.LogInformation($"{DateTime.Now} Syncing Users");

            using var scope = _scopeFactory.CreateScope();

            var apiClient = scope.ServiceProvider.GetRequiredService<BungieApiClient>();

            var membersTasks = _clanIDs.Select(x => apiClient.Api.GroupV2_GetMembersOfGroup(0, x));

            var activitiesDB = scope.ServiceProvider.GetRequiredService<IClanActivitiesDB>();

            var dbUsers = await activitiesDB.GetUsersWithCharactersAsync();
            var dbUsersDict = dbUsers.ToDictionary(x => x.UserID, x => x);

            await Task.WhenAll(membersTasks);

            var groupMembers = membersTasks.SelectMany(x => x.Result.Results);
            var groupMembersDict = groupMembers.ToDictionary(x => x.DestinyUserInfo.MembershipId, x => x);

            var usersToDelete = dbUsersDict.Where(x => !groupMembersDict.ContainsKey(x.Key)).Select(x => x.Value);
            var usersToUpdate = new ConcurrentBag<User>();
            var usersToAdd = new ConcurrentBag<User>();

            var components = new DestinyComponentType[] 
            { 
                DestinyComponentType.Profiles, 
                DestinyComponentType.Characters 
            };

            var chunks = groupMembersDict.Chunk(groupMembersDict.Count() / 8 + 1);

            var tasks = chunks.Select(x => Task.Run(async () =>
            {
                foreach (var member in x)
                {
                    var profile = await apiClient.Api.Destiny2_GetProfile(member.Key, member.Value.DestinyUserInfo.MembershipType, components);

                    var dbUser = dbUsersDict.GetValueOrDefault(member.Key);

                    if (dbUser is null)
                    {
                        usersToAdd.Add(new User
                        {
                            UserID = member.Key,
                            ClanID = member.Value.GroupId,
                            ClanJoinDate = member.Value.JoinDate,
                            UserName = $"{member.Value.DestinyUserInfo.BungieGlobalDisplayName}#{member.Value.DestinyUserInfo.BungieGlobalDisplayNameCode}",
                            MembershipType = (int)member.Value.DestinyUserInfo.MembershipType,
                            DateLastPlayed = profile.Profile.Data.DateLastPlayed,
                            Characters = profile.Characters.Data
                                .Select(y => new Character
                                {
                                    UserID = member.Key,
                                    CharacterID = y.Key,
                                    DateLastPlayed = y.Value.DateLastPlayed,
                                    Class = (int)y.Value.ClassType,
                                    Race = (int)y.Value.RaceType,
                                    Gender = (int)y.Value.GenderType
                                }).ToList()
                        });
                    }
                    else if (dbUser.DateLastPlayed < profile.Profile.Data.DateLastPlayed)
                    {
                        var profileChars = profile.Characters.Data;

                        foreach (var diffChr in dbUser.Characters.Where(y => !profileChars.ContainsKey(y.CharacterID)))
                            dbUser.Characters.Remove(diffChr);

                        foreach (var chr in profileChars)
                        {
                            var dbChr = dbUser.Characters.FirstOrDefault(y => y.CharacterID == chr.Key);

                            if (dbChr is null)
                            {
                                dbUser.Characters.Add(new Character
                                {
                                    UserID = member.Key,
                                    CharacterID = chr.Key,
                                    DateLastPlayed = chr.Value.DateLastPlayed,
                                    Class = (int)chr.Value.ClassType,
                                    Race = (int)chr.Value.RaceType,
                                    Gender = (int)chr.Value.GenderType
                                });
                            }
                            else if (dbChr.DateLastPlayed < chr.Value.DateLastPlayed)
                            {
                                dbChr.DateLastPlayed = chr.Value.DateLastPlayed;
                                dbChr.Class = (int)chr.Value.ClassType;
                                dbChr.Race = (int)chr.Value.RaceType;
                                dbChr.Gender = (int)chr.Value.GenderType;
                            }
                        }

                        dbUser.ClanID = member.Value.GroupId;
                        dbUser.UserName = $"{member.Value.DestinyUserInfo.BungieGlobalDisplayName}#{member.Value.DestinyUserInfo.BungieGlobalDisplayNameCode}";
                        dbUser.MembershipType = (int)member.Value.DestinyUserInfo.MembershipType;
                        dbUser.DateLastPlayed = profile.Profile.Data.DateLastPlayed;

                        usersToUpdate.Add(dbUser);
                    }
                }
            }));

            await Task.WhenAll(tasks);

            await activitiesDB.SyncUsersAsync(usersToDelete, usersToUpdate, usersToAdd);

            _logger.LogInformation($"{DateTime.Now} Users synced");
        }
    }
}
