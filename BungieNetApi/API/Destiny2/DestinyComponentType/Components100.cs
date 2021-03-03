using System;
using System.Runtime.Serialization;

namespace API.Destiny2.DestinyComponentType.Components100
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
        public Profile profile { get; set; }
    }

    public class Profile
    {
        public Data data { get; set; }

        [IgnoreDataMember]
        public int privacy { get; set; }
    }

    public class Data
    {
        [IgnoreDataMember]
        public Userinfo userInfo { get; set; }

        public DateTime dateLastPlayed { get; set; }

        [IgnoreDataMember]
        public int versionsOwned { get; set; }

        public string[] characterIds { get; set; }

        [IgnoreDataMember]
        public long[] seasonHashes { get; set; }
        [IgnoreDataMember]
        public long currentSeasonHash { get; set; }
        [IgnoreDataMember]
        public int currentSeasonRewardPowerCap { get; set; }
    }

    public class Userinfo
    {
        public int crossSaveOverride { get; set; }
        public int[] applicableMembershipTypes { get; set; }
        public bool isPublic { get; set; }
        public int membershipType { get; set; }
        public string membershipId { get; set; }
        public string displayName { get; set; }
    }

    public class Messagedata
    {
    }
}
