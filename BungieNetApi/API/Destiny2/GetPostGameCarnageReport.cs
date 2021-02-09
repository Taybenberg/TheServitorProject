using System;

namespace API.Destiny2.GetPostGameCarnageReport
{

    public class Rootobject
    {
        public Response Response { get; set; }
        public int ErrorCode { get; set; }
        public int ThrottleSeconds { get; set; }
        public string ErrorStatus { get; set; }
        public string Message { get; set; }
        public Messagedata MessageData { get; set; }
    }

    public class Response
    {
        public DateTime period { get; set; }
        public int startingPhaseIndex { get; set; }
        public Activitydetails activityDetails { get; set; }
        public Entry[] entries { get; set; }
        public object[] teams { get; set; }
    }

    public class Activitydetails
    {
        public long referenceId { get; set; }
        public long directorActivityHash { get; set; }
        public string instanceId { get; set; }
        public int mode { get; set; }
        public int[] modes { get; set; }
        public bool isPrivate { get; set; }
        public int membershipType { get; set; }
    }

    public class Entry
    {
        public int standing { get; set; }
        public Score score { get; set; }
        public Player player { get; set; }
        public string characterId { get; set; }
        public Values values { get; set; }
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
        public string characterClass { get; set; }
        public long classHash { get; set; }
        public long raceHash { get; set; }
        public long genderHash { get; set; }
        public int characterLevel { get; set; }
        public int lightLevel { get; set; }
        public long emblemHash { get; set; }
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

    public class Values
    {
        public Assists assists { get; set; }
        public Completed completed { get; set; }
        public Deaths deaths { get; set; }
        public Kills kills { get; set; }
        public Opponentsdefeated opponentsDefeated { get; set; }
        public Efficiency efficiency { get; set; }
        public Killsdeathsratio killsDeathsRatio { get; set; }
        public Killsdeathsassists killsDeathsAssists { get; set; }
        public Score score { get; set; }
        public Activitydurationseconds activityDurationSeconds { get; set; }
        public Completionreason completionReason { get; set; }
        public Fireteamid fireteamId { get; set; }
        public Startseconds startSeconds { get; set; }
        public Timeplayedseconds timePlayedSeconds { get; set; }
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
