using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Destiny2.GetPublicMilestones
{

    public class Rootobject
    {
        public Dictionary<string, Milestone> Response { get; set; }

        [JsonIgnore]
        public int ErrorCode { get; set; }
        [JsonIgnore]
        public int ThrottleSeconds { get; set; }
        [JsonIgnore]
        public string ErrorStatus { get; set; }
        [JsonIgnore]
        public string Message { get; set; }
        [JsonIgnore]
        public Messagedata MessageData { get; set; }
    }

    public class Milestone
    {
        [JsonIgnore]
        public long milestoneHash { get; set; }
        [JsonIgnore]
        public Vendor[] vendors { get; set; }

        public Activity[] activities { get; set; }

        [JsonIgnore]
        public Availablequest[] availableQuests { get; set; }
        [JsonIgnore]
        public DateTime startDate { get; set; }
        [JsonIgnore]
        public DateTime endDate { get; set; }
        [JsonIgnore]
        public int order { get; set; }
    }

    public class Vendor
    {
        public long vendorHash { get; set; }
    }

    public class Activity
    {
        public long activityHash { get; set; }

        [JsonIgnore]
        public object[] challengeObjectiveHashes { get; set; }
        [JsonIgnore]
        public long[] modifierHashes { get; set; }
        [JsonIgnore]
        public long[] phaseHashes { get; set; }
        [JsonIgnore]
        public Dictionary<string, bool> booleanActivityOptions { get; set; }
    }

    public class Availablequest
    {
        public long questItemHash { get; set; }
    }

    public class Messagedata
    {
    }
}
