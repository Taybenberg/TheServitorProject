using System.Runtime.Serialization;

namespace API.Destiny2.Manifest.DestinyActivityDefinition
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
        public Displayproperties displayProperties { get; set; }

        public Originaldisplayproperties originalDisplayProperties { get; set; }

        [IgnoreDataMember]
        public Selectionscreendisplayproperties selectionScreenDisplayProperties { get; set; }
        [IgnoreDataMember]
        public string releaseIcon { get; set; }
        [IgnoreDataMember]
        public int releaseTime { get; set; }
        [IgnoreDataMember]
        public int completionUnlockHash { get; set; }
        [IgnoreDataMember]
        public int activityLightLevel { get; set; }
        [IgnoreDataMember]
        public long destinationHash { get; set; }
        [IgnoreDataMember]
        public long placeHash { get; set; }
        [IgnoreDataMember]
        public int activityTypeHash { get; set; }
        [IgnoreDataMember]
        public int tier { get; set; }

        public string pgcrImage { get; set; }

        [IgnoreDataMember]
        public Reward[] rewards { get; set; }
        [IgnoreDataMember]
        public Modifier[] modifiers { get; set; }
        [IgnoreDataMember]
        public bool isPlaylist { get; set; }
        [IgnoreDataMember]
        public Challenge[] challenges { get; set; }
        [IgnoreDataMember]
        public object[] optionalUnlockStrings { get; set; }
        [IgnoreDataMember]
        public bool inheritFromFreeRoam { get; set; }
        [IgnoreDataMember]
        public bool suppressOtherRewards { get; set; }
        [IgnoreDataMember]
        public object[] playlistItems { get; set; }
        [IgnoreDataMember]
        public Matchmaking matchmaking { get; set; }
        [IgnoreDataMember]
        public int directActivityModeHash { get; set; }
        [IgnoreDataMember]
        public int directActivityModeType { get; set; }
        [IgnoreDataMember]
        public object[] loadouts { get; set; }
        [IgnoreDataMember]
        public long[] activityModeHashes { get; set; }
        [IgnoreDataMember]
        public int[] activityModeTypes { get; set; }
        [IgnoreDataMember]
        public bool isPvP { get; set; }
        [IgnoreDataMember]
        public object[] insertionPoints { get; set; }
        [IgnoreDataMember]
        public object[] activityLocationMappings { get; set; }
        [IgnoreDataMember]
        public long hash { get; set; }
        [IgnoreDataMember]
        public int index { get; set; }
        [IgnoreDataMember]
        public bool redacted { get; set; }
        [IgnoreDataMember]
        public bool blacklisted { get; set; }
    }

    public class Displayproperties
    {
        public string description { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool hasIcon { get; set; }
    }

    public class Originaldisplayproperties
    {
        public string description { get; set; }
        public string name { get; set; }

        [IgnoreDataMember]
        public string icon { get; set; }
        [IgnoreDataMember]
        public bool hasIcon { get; set; }
    }

    public class Selectionscreendisplayproperties
    {
        public string description { get; set; }
        public string name { get; set; }
        public bool hasIcon { get; set; }
    }

    public class Matchmaking
    {
        public bool isMatchmade { get; set; }
        public int minParty { get; set; }
        public int maxParty { get; set; }
        public int maxPlayers { get; set; }
        public bool requiresGuardianOath { get; set; }
    }

    public class Reward
    {
        public Rewarditem[] rewardItems { get; set; }
    }

    public class Rewarditem
    {
        public long itemHash { get; set; }
        public int quantity { get; set; }
    }

    public class Modifier
    {
        public long activityModifierHash { get; set; }
    }

    public class Challenge
    {
        public int rewardSiteHash { get; set; }
        public int inhibitRewardsUnlockHash { get; set; }
        public long objectiveHash { get; set; }
        public Dummyreward[] dummyRewards { get; set; }
    }

    public class Dummyreward
    {
        public long itemHash { get; set; }
        public int quantity { get; set; }
    }

    public class Messagedata
    {
    }
}
