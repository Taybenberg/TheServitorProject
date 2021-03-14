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

        [IgnoreDataMember]
        public Player player { get; set; }

        public string characterId { get; set; }
        public Value value { get; set; }
    }

    public class Player
    {
        public Destinyuserinfo destinyUserInfo { get; set; }
        public string characterClass { get; set; }
        public long classHash { get; set; }
        public long raceHash { get; set; }
        public long genderHash { get; set; }
        public int characterLevel { get; set; }
        public int lightLevel { get; set; }
        public int emblemHash { get; set; }
    }

    public class Destinyuserinfo
    {
        public string iconPath { get; set; }
        public int crossSaveOverride { get; set; }
        public int[] applicableMembershipTypes { get; set; }
        public bool isPublic { get; set; }
        public int membershipType { get; set; }
        public string membershipId { get; set; }
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
