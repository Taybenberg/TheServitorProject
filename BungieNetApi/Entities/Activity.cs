using BungieNetApi.Enums;
using System;
using System.Collections.Generic;

namespace BungieNetApi.Entities
{
    public class Activity
    {
        public long InstanceID { get; internal set; }

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
            public DateTime Period;
            public ActivityType ActivityType;
            public ActivityUserStats[] UserStats;

            internal ActivityContainer(BungieNetApiClient apiClient, long instanceID)
            {
                var rawActivity = apiClient.getRawActivityDetailsAsync(instanceID.ToString()).Result;

                if (rawActivity is not null)
                {
                    Period = rawActivity.period;
                    ActivityType = (ActivityType)rawActivity.activityDetails.mode;
                    UserStats = new ActivityUserStats[rawActivity.entries.Length];

                    for (int i = 0; i < UserStats.Length; i++)
                    {
                        UserStats[i] = new ActivityUserStats
                        {
                            CharacterID = long.Parse(rawActivity.entries[i].characterId),
                            MembershipID = long.Parse(rawActivity.entries[i].player.destinyUserInfo.membershipId),
                            MembershipType = (MembershipType)rawActivity.entries[i].player.destinyUserInfo.membershipType,
                            DisplayName = rawActivity.entries[i].player.destinyUserInfo.displayName,
                            ActivityDurationSeconds = rawActivity.entries[i].values.activityDurationSeconds.basic.value,
                            Completed = rawActivity.entries[i].values.completed.basic.value > 0,
                            CompletionReasonValue = rawActivity.entries[i].values.completionReason.basic.value,
                            CompletionReasonDisplayValue = rawActivity.entries[i].values.completionReason.basic.displayValue,
                            Score = rawActivity.entries[i].values.score.basic.value,
                            TeamScore = rawActivity.entries[i].values.teamScore.basic.value
                        };

                        (UserStats[i].StandingValue, UserStats[i].StandingDisplayValue) =
                            (rawActivity.entries[i].values.standing is not null ?
                            (rawActivity.entries[i].values.standing.basic.value, rawActivity.entries[i].values.standing.basic.displayValue) :
                            (-1.0f, "Unknown"));
                    }
                }
            }
        }

        private Lazy<ActivityContainer> _container;

        internal Activity(BungieNetApiClient apiClient)
        {
            _container = new(() => new ActivityContainer(apiClient, InstanceID));
        }

        public DateTime Period
        {
            get
            {
                return _container.Value.Period;
            }
        }

        public ActivityType ActivityType
        {
            get
            {
                return _container.Value.ActivityType;
            }
        }

        public IEnumerable<ActivityUserStats> UserStats
        {
            get
            {
                return _container.Value.UserStats;
            }
        }
    }
}
