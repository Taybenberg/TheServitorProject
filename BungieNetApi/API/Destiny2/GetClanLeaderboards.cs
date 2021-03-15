using System.Collections.Generic;
using System.Runtime.Serialization;

namespace API.Destiny2.GetClanLeaderboards
{
    public class Rootobject
    {
        public Dictionary<string, Dictionary<string, Stat>> Response { get; set; }

        [IgnoreDataMember]
        public int ErrorCode { get; set; }
        [IgnoreDataMember]
        public int ThrottleSeconds { get; set; }
        [IgnoreDataMember]
        public string ErrorStatus { get; set; }
        [IgnoreDataMember]
        public string Message { get; set; }
        [IgnoreDataMember]
        public Messagedata MessageData { get; set; }
    }

    public class Stat
    {
        [IgnoreDataMember]
        public string statId { get; set; }

        public Entry[] entries { get; set; }
    }

    public class Entry
    {
        public int rank { get; set; }

        public Player player { get; set; }

        [IgnoreDataMember]
        public string characterId { get; set; }

        public Value value { get; set; }
    }

    public class Player
    {
        public Destinyuserinfo destinyUserInfo { get; set; }
        public string characterClass { get; set; }

        [IgnoreDataMember]
        public long classHash { get; set; }
        [IgnoreDataMember]
        public long raceHash { get; set; }
        [IgnoreDataMember]
        public long genderHash { get; set; }
        [IgnoreDataMember]
        public int characterLevel { get; set; }
        [IgnoreDataMember]
        public int lightLevel { get; set; }
        [IgnoreDataMember]
        public int emblemHash { get; set; }
    }

    public class Destinyuserinfo
    {
        [IgnoreDataMember]
        public string iconPath { get; set; }
        [IgnoreDataMember]
        public int crossSaveOverride { get; set; }
        [IgnoreDataMember]
        public int[] applicableMembershipTypes { get; set; }
        [IgnoreDataMember]
        public bool isPublic { get; set; }
        [IgnoreDataMember]
        public int membershipType { get; set; }

        public string membershipId { get; set; }

        [IgnoreDataMember]
        public string displayName { get; set; }
    }

    public class Value
    {
        public Basic basic { get; set; }

        [IgnoreDataMember]
        public string activityId { get; set; }
    }

    public class Basic
    {
        [IgnoreDataMember]
        public float value { get; set; }

        public string displayValue { get; set; }
    }

    public class Messagedata
    {
    }

}
