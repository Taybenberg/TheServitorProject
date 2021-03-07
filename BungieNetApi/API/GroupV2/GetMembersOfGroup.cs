using System;
using System.Runtime.Serialization;

namespace API.GroupV2.GetMembersOfGroup
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
        public Result[] results { get; set; }

        [IgnoreDataMember]
        public int totalResults { get; set; }
        [IgnoreDataMember]
        public bool hasMore { get; set; }
        [IgnoreDataMember]
        public Query query { get; set; }
        [IgnoreDataMember]
        public bool useTotalResults { get; set; }
    }

    public class Query
    {
        public int itemsPerPage { get; set; }
        public int currentPage { get; set; }
    }

    public class Result
    {
        [IgnoreDataMember]
        public int memberType { get; set; }
        [IgnoreDataMember]
        public bool isOnline { get; set; }
        [IgnoreDataMember]
        public string lastOnlineStatusChange { get; set; }
        [IgnoreDataMember]
        public string groupId { get; set; }

        public Destinyuserinfo destinyUserInfo { get; set; }

        [IgnoreDataMember]
        public Bungienetuserinfo bungieNetUserInfo { get; set; }

        public DateTime joinDate { get; set; }
    }

    public class Destinyuserinfo
    {
        public string LastSeenDisplayName { get; set; }

        [IgnoreDataMember]
        public int LastSeenDisplayNameType { get; set; }
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

        [IgnoreDataMember]
        public string displayName { get; set; }
    }

    public class Bungienetuserinfo
    {
        public string supplementalDisplayName { get; set; }
        public string iconPath { get; set; }
        public int crossSaveOverride { get; set; }
        public bool isPublic { get; set; }
        public int membershipType { get; set; }
        public string membershipId { get; set; }
        public string displayName { get; set; }
    }

    public class Messagedata
    {
    }
}