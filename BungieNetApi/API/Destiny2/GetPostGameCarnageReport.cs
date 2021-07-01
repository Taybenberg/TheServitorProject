using System;
using System.Runtime.Serialization;

namespace API.Destiny2.GetPostGameCarnageReport
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
        [IgnoreDataMember]
        public DateTime period { get; set; }
        [IgnoreDataMember]
        public int startingPhaseIndex { get; set; }

        public Activitydetails activityDetails { get; set; }
        public Entry[] entries { get; set; }

        [IgnoreDataMember]
        public object[] teams { get; set; }
    }

    public class Activitydetails
    {
        [IgnoreDataMember]
        public long referenceId { get; set; }
        [IgnoreDataMember]
        public long directorActivityHash { get; set; }

        public string instanceId { get; set; }
        public int mode { get; set; }

        [IgnoreDataMember]
        public int[] modes { get; set; }
        [IgnoreDataMember]
        public bool isPrivate { get; set; }
        [IgnoreDataMember]
        public int membershipType { get; set; }
    }

    public class Entry
    {
        [IgnoreDataMember]
        public int standing { get; set; }
        [IgnoreDataMember]
        public Score score { get; set; }

        public Player player { get; set; }
        public string characterId { get; set; }
        public Values values { get; set; }

        [IgnoreDataMember]
        public Extended extended { get; set; }
    }

    public class Score
    {
        public Basic basic { get; set; }
    }

    public class Basic
    {
        public float value { get; set; }
        public string displayValue { get; set; }
    }

    public class Player
    {
        public Destinyuserinfo destinyUserInfo { get; set; }

        [IgnoreDataMember]
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
        public long emblemHash { get; set; }
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

        public int membershipType { get; set; }
        public string membershipId { get; set; }
        public string displayName { get; set; }
    }

    public class Values
    {
        [IgnoreDataMember]
        public Assists assists { get; set; }

        public Completed completed { get; set; }

        [IgnoreDataMember]
        public Deaths deaths { get; set; }
        [IgnoreDataMember]
        public Kills kills { get; set; }
        [IgnoreDataMember]
        public Opponentsdefeated opponentsDefeated { get; set; }
        [IgnoreDataMember]
        public Efficiency efficiency { get; set; }
        [IgnoreDataMember]
        public Killsdeathsratio killsDeathsRatio { get; set; }
        [IgnoreDataMember]
        public Killsdeathsassists killsDeathsAssists { get; set; }

        public Score score { get; set; }
        public Activitydurationseconds activityDurationSeconds { get; set; }
        public Completionreason completionReason { get; set; }

        [IgnoreDataMember]
        public Fireteamid fireteamId { get; set; }
        [IgnoreDataMember]
        public Startseconds startSeconds { get; set; }
        [IgnoreDataMember]
        public Timeplayedseconds timePlayedSeconds { get; set; }
        [IgnoreDataMember]
        public Playercount playerCount { get; set; }

        public Teamscore teamScore { get; set; }
        public Standing standing { get; set; }
    }

    public class Standing
    {
        public Basic basic { get; set; }
    }

    public class Assists
    {
        public Basic basic { get; set; }
    }

    public class Completed
    {
        public Basic basic { get; set; }
    }

    public class Deaths
    {
        public Basic basic { get; set; }
    }

    public class Kills
    {
        public Basic basic { get; set; }
    }

    public class Opponentsdefeated
    {
        public Basic basic { get; set; }
    }

    public class Efficiency
    {
        public Basic basic { get; set; }
    }

    public class Killsdeathsratio
    {
        public Basic basic { get; set; }
    }

    public class Killsdeathsassists
    {
        public Basic basic { get; set; }
    }

    public class Activitydurationseconds
    {
        public Basic basic { get; set; }
    }

    public class Completionreason
    {
        public Basic basic { get; set; }
    }

    public class Fireteamid
    {
        public Basic basic { get; set; }
    }

    public class Startseconds
    {
        public Basic basic { get; set; }
    }

    public class Timeplayedseconds
    {
        public Basic basic { get; set; }
    }

    public class Playercount
    {
        public Basic basic { get; set; }
    }

    public class Teamscore
    {
        public Basic basic { get; set; }
    }

    public class Extended
    {
        public Weapon[] weapons { get; set; }
        public Values1 values { get; set; }
    }

    public class Values1
    {
        public Precisionkills precisionKills { get; set; }
        public Weaponkillsgrenade weaponKillsGrenade { get; set; }
        public Weaponkillsmelee weaponKillsMelee { get; set; }
        public Weaponkillssuper weaponKillsSuper { get; set; }
        public Weaponkillsability weaponKillsAbility { get; set; }
    }

    public class Precisionkills
    {
        public Basic basic { get; set; }
    }

    public class Weaponkillsgrenade
    {
        public Basic basic { get; set; }
    }

    public class Weaponkillsmelee
    {
        public Basic basic { get; set; }
    }

    public class Weaponkillssuper
    {
        public Basic basic { get; set; }
    }

    public class Weaponkillsability
    {
        public Basic basic { get; set; }
    }

    public class Weapon
    {
        public long referenceId { get; set; }
        public Values2 values { get; set; }
    }

    public class Values2
    {
        public Uniqueweaponkills uniqueWeaponKills { get; set; }
        public Uniqueweaponprecisionkills uniqueWeaponPrecisionKills { get; set; }
        public Uniqueweaponkillsprecisionkills uniqueWeaponKillsPrecisionKills { get; set; }
    }

    public class Uniqueweaponkills
    {
        public Basic basic { get; set; }
    }

    public class Uniqueweaponprecisionkills
    {
        public Basic basic { get; set; }
    }

    public class Uniqueweaponkillsprecisionkills
    {
        public Basic basic { get; set; }
    }

    public class Messagedata
    {
    }

}
