using BungieNetApi.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BungieNetApi.Entities
{
    public class Activity
    {
        private readonly BungieNetApiClient _apiClient;
        internal Activity(BungieNetApiClient apiClient) => _apiClient = apiClient;

        public long InstanceId { get; internal set; }

        private DateTime? _period = null;
        public DateTime Period
        {
            get
            {
                if (_period is null)
                    initActivityDetailsAsync().Wait();

                return (DateTime)_period;
            }
        }

        private ActivityType? _activityType = null;
        public ActivityType ActivityType
        {
            get
            {
                if (_activityType is null)
                    initActivityDetailsAsync().Wait();

                return (ActivityType)_activityType;
            }
        }

        public record ActivityUserStats
        {
            public long CharacterId { get; internal set; }
            public long MembershipId { get; internal set; }
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

        private ActivityUserStats[] _activityUserStats = null;
        public IEnumerable<ActivityUserStats> UserStats
        {
            get
            {
                if (_activityUserStats is null)
                    initActivityDetailsAsync().Wait();

                return _activityUserStats;
            }
        }

        private async Task initActivityDetailsAsync()
        {
            var rawActivity = await _apiClient.getRawActivityDetailsAsync(InstanceId.ToString());

            if (rawActivity is not null)
            {
                _period = rawActivity.period;
                _activityType = (ActivityType)rawActivity.activityDetails.mode;
                _activityUserStats = new ActivityUserStats[rawActivity.entries.Length];

                for (int i = 0; i < _activityUserStats.Length; i++)
                {
                    _activityUserStats[i] = new ActivityUserStats
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

                    (_activityUserStats[i].StandingValue, _activityUserStats[i].StandingDisplayValue) =
                        (rawActivity.entries[i].values.standing is not null ?
                        (rawActivity.entries[i].values.standing.basic.value, rawActivity.entries[i].values.standing.basic.displayValue) :
                        (-1.0f, "Unknown"));
                }
            }
        }
    }
}
