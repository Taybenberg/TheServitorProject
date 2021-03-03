using System;
using System.Runtime.Serialization;

namespace API.Destiny2.DestinyComponentType.Components200
{

    public class Rootobject
    {
        public Response Response { get; set; }

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

    public class Response
    {
        public Character character { get; set; }
    }

    public class Character
    {
        public Data data { get; set; }

        [IgnoreDataMember]
        public int privacy { get; set; }
    }

    public class Data
    {
        public string membershipId { get; set; }

        [IgnoreDataMember]
        public int membershipType { get; set; }

        public string characterId { get; set; }
        public DateTime dateLastPlayed { get; set; }

        [IgnoreDataMember]
        public string minutesPlayedThisSession { get; set; }
        [IgnoreDataMember]
        public string minutesPlayedTotal { get; set; }
        [IgnoreDataMember]
        public int light { get; set; }
        [IgnoreDataMember]
        public Stats stats { get; set; }
        [IgnoreDataMember]
        public long raceHash { get; set; }
        [IgnoreDataMember]
        public long genderHash { get; set; }
        [IgnoreDataMember]
        public long classHash { get; set; }

        public int raceType { get; set; }
        public int classType { get; set; }
        public int genderType { get; set; }

        [IgnoreDataMember]
        public string emblemPath { get; set; }
        [IgnoreDataMember]
        public string emblemBackgroundPath { get; set; }
        [IgnoreDataMember]
        public long emblemHash { get; set; }
        [IgnoreDataMember]
        public Emblemcolor emblemColor { get; set; }
        [IgnoreDataMember]
        public Levelprogression levelProgression { get; set; }
        [IgnoreDataMember]
        public int baseCharacterLevel { get; set; }
        [IgnoreDataMember]
        public float percentToNextLevel { get; set; }
        [IgnoreDataMember]
        public long titleRecordHash { get; set; }
    }

    public class Stats
    {
        public int _1935470627 { get; set; }
        public int _2996146975 { get; set; }
        public int _392767087 { get; set; }
        public int _1943323491 { get; set; }
        public int _1735777505 { get; set; }
        public int _144602215 { get; set; }
        public int _4244567218 { get; set; }
    }

    public class Emblemcolor
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int alpha { get; set; }
    }

    public class Levelprogression
    {
        public int progressionHash { get; set; }
        public int dailyProgress { get; set; }
        public int dailyLimit { get; set; }
        public int weeklyProgress { get; set; }
        public int weeklyLimit { get; set; }
        public int currentProgress { get; set; }
        public int level { get; set; }
        public int levelCap { get; set; }
        public int stepIndex { get; set; }
        public int progressToNextLevel { get; set; }
        public int nextLevelAt { get; set; }
    }

    public class Messagedata
    {
    }
}
