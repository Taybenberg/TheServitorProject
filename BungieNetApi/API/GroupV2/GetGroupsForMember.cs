using System;
using System.Runtime.Serialization;

namespace API.GroupV2.GetGroupsForMember
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
        public Areallmembershipsinactive areAllMembershipsInactive { get; set; }

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

    public class Areallmembershipsinactive
    {
        public bool _2756228 { get; set; }
    }

    public class Query
    {
        public int itemsPerPage { get; set; }
        public int currentPage { get; set; }
    }

    public class Result
    {
        [IgnoreDataMember]
        public Member member { get; set; }

        public Group group { get; set; }
    }

    public class Member
    {
        public int memberType { get; set; }
        public bool isOnline { get; set; }
        public string lastOnlineStatusChange { get; set; }
        public string groupId { get; set; }
        public Destinyuserinfo destinyUserInfo { get; set; }
        public DateTime joinDate { get; set; }
    }

    public class Destinyuserinfo
    {
        public string LastSeenDisplayName { get; set; }
        public int LastSeenDisplayNameType { get; set; }
        public string iconPath { get; set; }
        public int crossSaveOverride { get; set; }
        public int[] applicableMembershipTypes { get; set; }
        public bool isPublic { get; set; }
        public int membershipType { get; set; }
        public string membershipId { get; set; }
        public string displayName { get; set; }
    }

    public class Group
    {
        [IgnoreDataMember]
        public string groupId { get; set; }

        public string name { get; set; }

        [IgnoreDataMember]
        public int groupType { get; set; }
        [IgnoreDataMember]
        public string membershipIdCreated { get; set; }
        [IgnoreDataMember]
        public DateTime creationDate { get; set; }
        [IgnoreDataMember]
        public DateTime modificationDate { get; set; }
        [IgnoreDataMember]
        public string about { get; set; }
        [IgnoreDataMember]
        public object[] tags { get; set; }
        [IgnoreDataMember]
        public int memberCount { get; set; }
        [IgnoreDataMember]
        public bool isPublic { get; set; }
        [IgnoreDataMember]
        public bool isPublicTopicAdminOnly { get; set; }
        [IgnoreDataMember]
        public string motto { get; set; }
        [IgnoreDataMember]
        public bool allowChat { get; set; }
        [IgnoreDataMember]
        public bool isDefaultPostPublic { get; set; }
        [IgnoreDataMember]
        public int chatSecurity { get; set; }
        [IgnoreDataMember]
        public string locale { get; set; }
        [IgnoreDataMember]
        public int avatarImageIndex { get; set; }
        [IgnoreDataMember]
        public int homepage { get; set; }
        [IgnoreDataMember]
        public int membershipOption { get; set; }
        [IgnoreDataMember]
        public int defaultPublicity { get; set; }
        [IgnoreDataMember]
        public string theme { get; set; }
        [IgnoreDataMember]
        public string bannerPath { get; set; }
        [IgnoreDataMember]
        public string avatarPath { get; set; }
        [IgnoreDataMember]
        public string conversationId { get; set; }
        [IgnoreDataMember]
        public bool enableInvitationMessagingForAdmins { get; set; }
        [IgnoreDataMember]
        public DateTime banExpireDate { get; set; }
        [IgnoreDataMember]
        public Features features { get; set; }

        public Claninfo clanInfo { get; set; }
    }

    public class Features
    {
        public int maximumMembers { get; set; }
        public int maximumMembershipsOfGroupType { get; set; }
        public int capabilities { get; set; }
        public int[] membershipTypes { get; set; }
        public bool invitePermissionOverride { get; set; }
        public bool updateCulturePermissionOverride { get; set; }
        public int hostGuidedGamePermissionOverride { get; set; }
        public bool updateBannerPermissionOverride { get; set; }
        public int joinLevel { get; set; }
    }

    public class Claninfo
    {
        [IgnoreDataMember]
        public D2clanprogressions d2ClanProgressions { get; set; }

        public string clanCallsign { get; set; }

        [IgnoreDataMember]
        public Clanbannerdata clanBannerData { get; set; }
    }

    public class D2clanprogressions
    {
        public _584850370 _584850370 { get; set; }
        public _1273404180 _1273404180 { get; set; }
        public _3759191272 _3759191272 { get; set; }
        public _3381682691 _3381682691 { get; set; }
    }

    public class _584850370
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

    public class _1273404180
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

    public class _3759191272
    {
        public long progressionHash { get; set; }
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

    public class _3381682691
    {
        public long progressionHash { get; set; }
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

    public class Clanbannerdata
    {
        public long decalId { get; set; }
        public long decalColorId { get; set; }
        public long decalBackgroundColorId { get; set; }
        public int gonfalonId { get; set; }
        public long gonfalonColorId { get; set; }
        public int gonfalonDetailId { get; set; }
        public long gonfalonDetailColorId { get; set; }
    }

    public class Messagedata
    {
    }
}
