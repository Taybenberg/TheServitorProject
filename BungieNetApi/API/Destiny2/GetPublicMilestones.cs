using System;
using System.Runtime.Serialization;

namespace API.Destiny2.GetPublicMilestones
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
        [IgnoreDataMember]
        public _534869653 _534869653 { get; set; }
        [IgnoreDataMember]
        public _3181387331 _3181387331 { get; set; }
        [IgnoreDataMember]
        public _4253138191 _4253138191 { get; set; }
        [IgnoreDataMember]
        public _3603098564 _3603098564 { get; set; }
        [IgnoreDataMember]
        public _3802603984 _3802603984 { get; set; }
        [IgnoreDataMember]
        public _2709491520 _2709491520 { get; set; }
        [IgnoreDataMember]
        public _2594202463 _2594202463 { get; set; }
        [IgnoreDataMember]
        public _3899487295 _3899487295 { get; set; }
        [IgnoreDataMember]
        public _480262465 _480262465 { get; set; }
        [IgnoreDataMember]
        public _2712317338 _2712317338 { get; set; }
        [IgnoreDataMember]
        public _2540726600 _2540726600 { get; set; }
        [IgnoreDataMember]
        public _1424672028 _1424672028 { get; set; }
        [IgnoreDataMember]
        public _541780856 _541780856 { get; set; }
        [IgnoreDataMember]
        public _2406589846 _2406589846 { get; set; }
        [IgnoreDataMember]
        public _1403471610 _1403471610 { get; set; }

        public _1942283261 _1942283261 { get; set; }

        [IgnoreDataMember]
        public _2029743966 _2029743966 { get; set; }
        [IgnoreDataMember]
        public _3448738070 _3448738070 { get; set; }
        [IgnoreDataMember]
        public _1368032265 _1368032265 { get; set; }
        [IgnoreDataMember]
        public _3312774044 _3312774044 { get; set; }
        [IgnoreDataMember]
        public _1437935813 _1437935813 { get; set; }
        [IgnoreDataMember]
        public _3628293757 _3628293757 { get; set; }
        [IgnoreDataMember]
        public _3628293755 _3628293755 { get; set; }
        [IgnoreDataMember]
        public _3628293753 _3628293753 { get; set; }
        [IgnoreDataMember]
        public _825965416 _825965416 { get; set; }
        [IgnoreDataMember]
        public _3632712541 _3632712541 { get; set; }
        [IgnoreDataMember]
        public _2953722265 _2953722265 { get; set; }
        [IgnoreDataMember]
        public _3031052508 _3031052508 { get; set; }
    }

    public class _534869653
    {
        public int milestoneHash { get; set; }
        public Vendor[] vendors { get; set; }
        public int order { get; set; }
    }

    public class Vendor
    {
        public long vendorHash { get; set; }
    }

    public class _3181387331
    {
        public long milestoneHash { get; set; }
        public Activity[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] phaseHashes { get; set; }
        public Booleanactivityoptions booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions
    {
        public bool _106117858 { get; set; }
    }

    public class _4253138191
    {
        public long milestoneHash { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class _3603098564
    {
        public long milestoneHash { get; set; }
        public Availablequest[] availableQuests { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Availablequest
    {
        public int questItemHash { get; set; }
    }

    public class _3802603984
    {
        public long milestoneHash { get; set; }
        public Availablequest1[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest1
    {
        public long questItemHash { get; set; }
    }

    public class _2709491520
    {
        public long milestoneHash { get; set; }
        public Availablequest2[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest2
    {
        public long questItemHash { get; set; }
    }

    public class _2594202463
    {
        public long milestoneHash { get; set; }
        public Availablequest3[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest3
    {
        public int questItemHash { get; set; }
    }

    public class _3899487295
    {
        public long milestoneHash { get; set; }
        public Availablequest4[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest4
    {
        public int questItemHash { get; set; }
    }

    public class _480262465
    {
        public int milestoneHash { get; set; }
        public Availablequest5[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest5
    {
        public int questItemHash { get; set; }
    }

    public class _2712317338
    {
        public long milestoneHash { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class _2540726600
    {
        public long milestoneHash { get; set; }
        public Availablequest6[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest6
    {
        public long questItemHash { get; set; }
    }

    public class _1424672028
    {
        public int milestoneHash { get; set; }
        public Availablequest7[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest7
    {
        public int questItemHash { get; set; }
    }

    public class _541780856
    {
        public int milestoneHash { get; set; }
        public Activity1[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity1
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
        public long[] phaseHashes { get; set; }
        public Booleanactivityoptions1 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions1
    {
        public bool _1282942169 { get; set; }
    }

    public class _2406589846
    {
        public long milestoneHash { get; set; }
        public Availablequest8[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest8
    {
        public int questItemHash { get; set; }
    }

    public class _1403471610
    {
        public int milestoneHash { get; set; }
        public Availablequest9[] availableQuests { get; set; }
        public int order { get; set; }
    }

    public class Availablequest9
    {
        public int questItemHash { get; set; }
    }

    public class _1942283261
    {
        public int milestoneHash { get; set; }
        public Activity2[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity2
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
        public Booleanactivityoptions2 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions2
    {
        public bool _1173729666 { get; set; }
        public bool _1118409995 { get; set; }
        public bool _1724944068 { get; set; }
        public bool _1658489738 { get; set; }
        public bool _2988353107 { get; set; }
        public bool _494695110 { get; set; }
        public bool _2681513389 { get; set; }
        public bool _701670507 { get; set; }
        public bool _1633278527 { get; set; }
        public bool _2395430749 { get; set; }
        public bool _2286717819 { get; set; }
        public bool _1273646627 { get; set; }
        public bool _1631522863 { get; set; }
    }

    public class _2029743966
    {
        public int milestoneHash { get; set; }
        public Activity3[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity3
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
        public Booleanactivityoptions3 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions3
    {
        public bool _1173729666 { get; set; }
        public bool _1118409995 { get; set; }
        public bool _1724944068 { get; set; }
        public bool _1658489738 { get; set; }
        public bool _2988353107 { get; set; }
        public bool _494695110 { get; set; }
        public bool _2681513389 { get; set; }
        public bool _701670507 { get; set; }
        public bool _1633278527 { get; set; }
        public bool _2395430749 { get; set; }
        public bool _2286717819 { get; set; }
        public bool _1273646627 { get; set; }
        public bool _1631522863 { get; set; }
    }

    public class _3448738070
    {
        public long milestoneHash { get; set; }
        public Activity4[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity4
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public Booleanactivityoptions4 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions4
    {
        public bool _46294267 { get; set; }
        public bool _1719431080 { get; set; }
        public bool _176840106 { get; set; }
    }

    public class _1368032265
    {
        public int milestoneHash { get; set; }
        public Activity5[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity5
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public Booleanactivityoptions5 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions5
    {
        public bool _2557768255 { get; set; }
        public bool _2395430749 { get; set; }
        public bool _871535305 { get; set; }
        public bool _176840106 { get; set; }
        public bool _23698980 { get; set; }
    }

    public class _3312774044
    {
        public long milestoneHash { get; set; }
        public Activity6[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity6
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public Booleanactivityoptions6 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions6
    {
        public bool _2732230962 { get; set; }
        public bool _1407915498 { get; set; }
        public bool _176840106 { get; set; }
        public bool _23698980 { get; set; }
        public bool _1030388888 { get; set; }
        public bool _4240501378 { get; set; }
        public bool _3673305833 { get; set; }
    }

    public class _1437935813
    {
        public int milestoneHash { get; set; }
        public Activity7[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity7
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
        public Booleanactivityoptions7 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions7
    {
        public bool _494695110 { get; set; }
    }

    public class _3628293757
    {
        public long milestoneHash { get; set; }
        public Activity8[] activities { get; set; }
        public int order { get; set; }
    }

    public class Activity8
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public Booleanactivityoptions8 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions8
    {
        public bool _298714641 { get; set; }
        public bool _4240501378 { get; set; }
        public bool _3673305833 { get; set; }
        public bool _2732230962 { get; set; }
        public bool _1407915498 { get; set; }
        public bool _176840106 { get; set; }
        public bool _23698980 { get; set; }
    }

    public class _3628293755
    {
        public long milestoneHash { get; set; }
        public Activity9[] activities { get; set; }
        public int order { get; set; }
    }

    public class Activity9
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public Booleanactivityoptions9 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions9
    {
        public bool _298714641 { get; set; }
        public bool _4240501378 { get; set; }
        public bool _3673305833 { get; set; }
        public bool _2732230962 { get; set; }
        public bool _1407915498 { get; set; }
        public bool _176840106 { get; set; }
        public bool _23698980 { get; set; }
    }

    public class _3628293753
    {
        public long milestoneHash { get; set; }
        public Activity10[] activities { get; set; }
        public int order { get; set; }
    }

    public class Activity10
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public Booleanactivityoptions10 booleanActivityOptions { get; set; }
    }

    public class Booleanactivityoptions10
    {
        public bool _298714641 { get; set; }
        public bool _4240501378 { get; set; }
        public bool _3673305833 { get; set; }
        public bool _2732230962 { get; set; }
        public bool _1407915498 { get; set; }
        public bool _176840106 { get; set; }
        public bool _23698980 { get; set; }
    }

    public class _825965416
    {
        public int milestoneHash { get; set; }
        public Activity11[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity11
    {
        public int activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
    }

    public class _3632712541
    {
        public long milestoneHash { get; set; }
        public Activity12[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity12
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
    }

    public class _2953722265
    {
        public long milestoneHash { get; set; }
        public Activity13[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity13
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
    }

    public class _3031052508
    {
        public long milestoneHash { get; set; }
        public Activity14[] activities { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int order { get; set; }
    }

    public class Activity14
    {
        public long activityHash { get; set; }
        public object[] challengeObjectiveHashes { get; set; }
        public long[] modifierHashes { get; set; }
    }

    public class Messagedata
    {
    }

}
