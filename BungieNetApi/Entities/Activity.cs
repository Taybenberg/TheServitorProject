using BungieNetApi.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BungieNetApi.Entities
{
    public class Activity
    {
        public long InstanceID { get; internal set; }

        public DateTime Period { get; internal set; }

        public ActivityType ActivityType { get; internal set; }

        public long ReferenceID { get; internal set; }

        public long DirectorActivityHash { get; internal set; }

        public record ActivityUserStats
        {
            public long CharacterID { get; internal set; }
            public long MembershipID { get; internal set; }
            public MembershipType MembershipType { get; internal set; }
            public string DisplayName { get; internal set; }
            public float ActivityDurationSeconds { get; internal set; }
            public bool Completed { get; internal set; }
            public float CompletionReasonValue { get; internal set; }
            public string CompletionReasonDisplayValue { get; internal set; }
            public float StandingValue { get; internal set; }
            public string StandingDisplayValue { get; internal set; }
            public float Score { get; internal set; }
            public float TeamScore { get; internal set; }
        }

        private class ActivityContainer
        {
            public ActivityUserStats[] UserStats;

            internal ActivityContainer(BungieNetApiClient apiClient, long instanceID)
            {
                var rawActivity = apiClient.getRawActivityDetailsAsync(instanceID.ToString()).Result;

                if (rawActivity is not null)
                {
                    UserStats = rawActivity.entries.Select(x => new ActivityUserStats
                    {
                        CharacterID = long.Parse(x.characterId),
                        MembershipID = long.Parse(x.player.destinyUserInfo.membershipId),
                        MembershipType = (MembershipType)x.player.destinyUserInfo.membershipType,
                        DisplayName = x.player.destinyUserInfo.displayName,
                        ActivityDurationSeconds = x.values.activityDurationSeconds.basic.value,
                        Completed = x.values.completed.basic.value > 0,
                        CompletionReasonValue = x.values.completionReason.basic.value,
                        CompletionReasonDisplayValue = x.values.completionReason.basic.displayValue,
                        Score = x.values.score.basic.value,
                        TeamScore = x.values.teamScore.basic.value,
                        StandingValue = x.values.standing?.basic.value ?? -1.0f,
                        StandingDisplayValue = x.values.standing?.basic.displayValue ?? "Unknown"
                    }).ToArray();
                }
            }
        }

        private Lazy<ActivityContainer> _container;

        internal Activity(BungieNetApiClient apiClient)
        {
            _container = new(() => new ActivityContainer(apiClient, InstanceID));
        }

        public ConcurrentBag<ActivityUserStats> _userStats;
        public IEnumerable<ActivityUserStats> UserStats
        {
            get
            {
                return _userStats?.ToArray() ?? _container.Value.UserStats;
            }
        }

        public void MergeUserStats(IEnumerable<ActivityUserStats> stats)
        {
            foreach (var stat in stats)
                _userStats.Add(stat);
        }
    }
}
