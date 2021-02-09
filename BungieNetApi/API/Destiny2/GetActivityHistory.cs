using System;

namespace API.Destiny2.GetActivityHistory
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
        public Activity[] activities { get; set; }
    }

    public class Activity
    {
        public DateTime period { get; set; }
        public Activitydetails activityDetails { get; set; }
        public Values values { get; set; }
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

    public class Values
    {
        public Assists assists { get; set; }
        public Score score { get; set; }
        public Kills kills { get; set; }
        public Averagescoreperkill averageScorePerKill { get; set; }
        public Deaths deaths { get; set; }
        public Averagescoreperlife averageScorePerLife { get; set; }
        public Completed completed { get; set; }
        public Opponentsdefeated opponentsDefeated { get; set; }
        public Efficiency efficiency { get; set; }
        public Killsdeathsratio killsDeathsRatio { get; set; }
        public Killsdeathsassists killsDeathsAssists { get; set; }
        public Activitydurationseconds activityDurationSeconds { get; set; }
        public Team team { get; set; }
        public Completionreason completionReason { get; set; }
        public Fireteamid fireteamId { get; set; }
        public Startseconds startSeconds { get; set; }
        public Timeplayedseconds timePlayedSeconds { get; set; }
        public Playercount playerCount { get; set; }
        public Teamscore teamScore { get; set; }
        public Standing standing { get; set; }
    }

    public class Assists
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Basic
    {
        public float value { get; set; }
        public string displayValue { get; set; }
    }

    public class Score
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Kills
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Averagescoreperkill
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Deaths
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Averagescoreperlife
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Completed
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Opponentsdefeated
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Efficiency
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Killsdeathsratio
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Killsdeathsassists
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Activitydurationseconds
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Team
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Completionreason
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Fireteamid
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Startseconds
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Timeplayedseconds
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Playercount
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Teamscore
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Standing
    {
        public string statId { get; set; }
        public Basic basic { get; set; }
    }

    public class Messagedata
    {
    }
}
