using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BungieNetApi
{
    /// <summary>
    /// https://bungie-net.github.io/multi/index.html
    /// </summary>
    public partial class BungieNetApiClient
    {
        private readonly ApiKey _xApiKey;

        private readonly long _clanId;

        public BungieNetApiClient(IConfiguration configuration)
        {
            _clanId = configuration.GetSection("Destiny2:ClanID").Get<long>();

            _xApiKey = configuration.GetSection("Destiny2:BungieApiKey").Get<ApiKey>();
        }

        public async Task<IEnumerable<(string sign, string name)>> GetUserClansAsync(MembershipType membershipType, long membershipId)
        {
            var rawClans = await getRawUserClans((int)membershipType, membershipId.ToString());

            return rawClans.Select(x=> (x.group.clanInfo.clanCallsign, x.group.name));
        }

        public async Task<IEnumerable<KeyValuePair<long, User>>> GetUsersAsync()
        {
            ConcurrentDictionary<long, User> users = new();

            var rawClanMembers = await getRawUsersAsync(_clanId.ToString());

            Parallel.ForEach(rawClanMembers, (rawuser) =>
            {
                var rawProfile = getRawProfileAsync(rawuser.destinyUserInfo.membershipType, rawuser.destinyUserInfo.membershipId).Result;

                var userProfile = new User
                {
                    MembershipId = long.Parse(rawuser.destinyUserInfo.membershipId),
                    LastSeenDisplayName = rawuser.destinyUserInfo.LastSeenDisplayName,
                    DateLastPlayed = rawProfile.data.dateLastPlayed,
                    ClanJoinDate = rawuser.joinDate,
                    Characters = new Character[rawProfile.data.characterIds.Length],
                    MembershipType = (MembershipType)rawuser.destinyUserInfo.membershipType
                };

                for (int i = 0; i < rawProfile.data.characterIds.Length; i++)
                {
                    var rawCharacter = getRawCharacterAsync((int)userProfile.MembershipType, userProfile.MembershipId.ToString(), rawProfile.data.characterIds[i]).Result;

                    userProfile.Characters[i] = new Character
                    {
                        CharacterId = long.Parse(rawCharacter.data.characterId),
                        MembershipId = long.Parse(rawCharacter.data.membershipId),
                        DateLastPlayed = rawCharacter.data.dateLastPlayed,
                        Class = (DestinyClass)rawCharacter.data.classType,
                        Race = (DestinyRace)rawCharacter.data.raceType,
                        Gender = (DestinyGender)rawCharacter.data.genderType
                    };
                }

                users.TryAdd(userProfile.MembershipId, userProfile);
            });

            return users.OrderByDescending(x => x.Value.DateLastPlayed);
        }

        public async Task<IEnumerable<KeyValuePair<long, Activity>>> GetUserActivitiesAsync(MembershipType membershipType, long membershipId, long characterId, int count, int page)
        {
            ConcurrentDictionary<long, Activity> activities = new();

            var rawUserActivities = await getRawActivitiesAsync((int)membershipType, membershipId.ToString(), characterId.ToString(), count, page);

            if (rawUserActivities is null)
                return activities;

            Parallel.ForEach(rawUserActivities, (rawUserActivity) =>
            {
                var activity = GetActivityDetailsAsync(rawUserActivity.activityDetails.instanceId).Result;

                if (activity is not null)
                {
                    activities.TryAdd(activity.InstanceId, activity);
                }
            });

            return activities.OrderByDescending(x => x.Value.Period);
        }

        public async Task<Activity> GetActivityDetailsAsync(string instanceId)
        {
            var rawActivity = await getRawActivityDetailsAsync(instanceId);

            if (rawActivity is not null)
            {
                var activity = new Activity
                {
                    InstanceId = long.Parse(rawActivity.activityDetails.instanceId),
                    Period = rawActivity.period,
                    ActivityType = (ActivityType)rawActivity.activityDetails.mode,
                    ActivityUserStats = new ActivityUserStats[rawActivity.entries.Length]
                };

                for (int i = 0; i < rawActivity.entries.Length; i++)
                {
                    activity.ActivityUserStats[i] = new ActivityUserStats
                    {
                        CharacterId = long.Parse(rawActivity.entries[i].characterId),
                        MembershipId = long.Parse(rawActivity.entries[i].player.destinyUserInfo.membershipId),
                        MembershipType = (MembershipType)rawActivity.entries[i].player.destinyUserInfo.membershipType,
                        DisplayName = rawActivity.entries[i].player.destinyUserInfo.displayName,
                        ActivityDurationSeconds = rawActivity.entries[i].values.activityDurationSeconds.basic.value,
                        Completed = rawActivity.entries[i].values.completed.basic.value > 0,
                        CompletionReasonValue = rawActivity.entries[i].values.completionReason.basic.value,
                        CompletionReasonDisplayValue = rawActivity.entries[i].values.completionReason.basic.displayValue,
                        Score = rawActivity.entries[i].values.score.basic.value,
                        TeamScore = rawActivity.entries[i].values.teamScore.basic.value
                    };

                    (activity.ActivityUserStats[i].StandingValue, activity.ActivityUserStats[i].StandingDisplayValue) =
                        (rawActivity.entries[i].values.standing is not null ?
                        (rawActivity.entries[i].values.standing.basic.value, rawActivity.entries[i].values.standing.basic.displayValue) :
                        (-1.0f, "Unknown"));
                }

                return activity;
            }

            return null;
        }
    }
}