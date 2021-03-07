using System.Runtime.Serialization;

namespace API.Destiny2.GetDestinyEntityDefinition
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
        public Displayproperties displayProperties { get; set; }

        [IgnoreDataMember]
        public object[] tooltipNotifications { get; set; }
        [IgnoreDataMember]
        public int collectibleHash { get; set; }
        [IgnoreDataMember]
        public string iconWatermark { get; set; }
        [IgnoreDataMember]
        public string iconWatermarkShelved { get; set; }
        [IgnoreDataMember]
        public Backgroundcolor backgroundColor { get; set; }
        [IgnoreDataMember]
        public string screenshot { get; set; }
        [IgnoreDataMember]
        public string itemTypeDisplayName { get; set; }
        [IgnoreDataMember]
        public string flavorText { get; set; }
        [IgnoreDataMember]
        public string uiItemDisplayStyle { get; set; }

        public string itemTypeAndTierDisplayName { get; set; }

        [IgnoreDataMember]
        public string displaySource { get; set; }
        [IgnoreDataMember]
        public Action action { get; set; }
        [IgnoreDataMember]
        public Inventory inventory { get; set; }
        [IgnoreDataMember]
        public Stats stats { get; set; }

        public Equippingblock equippingBlock { get; set; }

        [IgnoreDataMember]
        public Translationblock translationBlock { get; set; }
        [IgnoreDataMember]
        public Preview preview { get; set; }
        [IgnoreDataMember]
        public Quality quality { get; set; }
        [IgnoreDataMember]
        public Objectives objectives { get; set; }
        [IgnoreDataMember]
        public int acquireRewardSiteHash { get; set; }
        [IgnoreDataMember]
        public int acquireUnlockHash { get; set; }
        [IgnoreDataMember]
        public Sockets sockets { get; set; }
        [IgnoreDataMember]
        public Talentgrid talentGrid { get; set; }
        [IgnoreDataMember]
        public Investmentstat[] investmentStats { get; set; }
        [IgnoreDataMember]
        public Perk[] perks { get; set; }
        [IgnoreDataMember]
        public long loreHash { get; set; }
        [IgnoreDataMember]
        public long summaryItemHash { get; set; }
        [IgnoreDataMember]
        public bool allowActions { get; set; }
        [IgnoreDataMember]
        public bool doesPostmasterPullHaveSideEffects { get; set; }
        [IgnoreDataMember]
        public bool nonTransferrable { get; set; }
        [IgnoreDataMember]
        public int[] itemCategoryHashes { get; set; }
        [IgnoreDataMember]
        public int specialItemType { get; set; }
        [IgnoreDataMember]
        public int itemType { get; set; }
        [IgnoreDataMember]
        public int itemSubType { get; set; }
        [IgnoreDataMember]
        public int classType { get; set; }
        [IgnoreDataMember]
        public int breakerType { get; set; }
        [IgnoreDataMember]
        public bool equippable { get; set; }
        [IgnoreDataMember]
        public long[] damageTypeHashes { get; set; }
        [IgnoreDataMember]
        public int[] damageTypes { get; set; }
        [IgnoreDataMember]
        public int defaultDamageType { get; set; }
        [IgnoreDataMember]
        public long defaultDamageTypeHash { get; set; }
        [IgnoreDataMember]
        public bool isWrapper { get; set; }
        [IgnoreDataMember]
        public string[] traitIds { get; set; }
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
        [IgnoreDataMember]
        public string description { get; set; }

        public string name { get; set; }
        public string icon { get; set; }

        [IgnoreDataMember]
        public bool hasIcon { get; set; }
    }

    public class Backgroundcolor
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int alpha { get; set; }
    }

    public class Action
    {
        public string verbName { get; set; }
        public string verbDescription { get; set; }
        public bool isPositive { get; set; }
        public int requiredCooldownSeconds { get; set; }
        public object[] requiredItems { get; set; }
        public object[] progressionRewards { get; set; }
        public string actionTypeLabel { get; set; }
        public int rewardSheetHash { get; set; }
        public int rewardItemHash { get; set; }
        public int rewardSiteHash { get; set; }
        public int requiredCooldownHash { get; set; }
        public bool deleteOnAction { get; set; }
        public bool consumeEntireStack { get; set; }
        public bool useOnAcquire { get; set; }
    }

    public class Inventory
    {
        public int maxStackSize { get; set; }
        public long bucketTypeHash { get; set; }
        public int recoveryBucketTypeHash { get; set; }
        public long tierTypeHash { get; set; }
        public bool isInstanceItem { get; set; }
        public bool nonTransferrableOriginal { get; set; }
        public string tierTypeName { get; set; }
        public int tierType { get; set; }
        public string expirationTooltip { get; set; }
        public string expiredInActivityMessage { get; set; }
        public string expiredInOrbitMessage { get; set; }
        public bool suppressExpirationWhenObjectivesComplete { get; set; }
    }

    public class Stats
    {
        public bool disablePrimaryStatDisplay { get; set; }
        public long statGroupHash { get; set; }
        public Stats1 stats { get; set; }
        public bool hasDisplayableStats { get; set; }
        public int primaryBaseStatHash { get; set; }
    }

    public class Stats1
    {
        public _1480404414 _1480404414 { get; set; }
        public _1935470627 _1935470627 { get; set; }
        public _1885944937 _1885944937 { get; set; }
        public _3555269338 _3555269338 { get; set; }
        public _2961396640 _2961396640 { get; set; }
        public _4043523819 _4043523819 { get; set; }
        public _1240592695 _1240592695 { get; set; }
        public _155624089 _155624089 { get; set; }
        public _3871231066 _3871231066 { get; set; }
        public _4188031367 _4188031367 { get; set; }
        public _1931675084 _1931675084 { get; set; }
        public _943549884 _943549884 { get; set; }
        public _1345609583 _1345609583 { get; set; }
        public _2715839340 _2715839340 { get; set; }
    }

    public class _1480404414
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _1935470627
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _1885944937
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _3555269338
    {
        public long statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _2961396640
    {
        public long statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _4043523819
    {
        public long statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _1240592695
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _155624089
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _3871231066
    {
        public long statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _4188031367
    {
        public long statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _1931675084
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _943549884
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _1345609583
    {
        public int statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class _2715839340
    {
        public long statHash { get; set; }
        public int value { get; set; }
        public int minimum { get; set; }
        public int maximum { get; set; }
        public int displayMaximum { get; set; }
    }

    public class Equippingblock
    {
        public string uniqueLabel { get; set; }

        [IgnoreDataMember]
        public long uniqueLabelHash { get; set; }
        [IgnoreDataMember]
        public long equipmentSlotTypeHash { get; set; }
        [IgnoreDataMember]
        public int attributes { get; set; }
        [IgnoreDataMember]
        public int equippingSoundHash { get; set; }
        [IgnoreDataMember]
        public int hornSoundHash { get; set; }
        [IgnoreDataMember]
        public int ammoType { get; set; }
        [IgnoreDataMember]
        public string[] displayStrings { get; set; }
    }

    public class Translationblock
    {
        public long weaponPatternHash { get; set; }
        public object[] defaultDyes { get; set; }
        public Lockeddye[] lockedDyes { get; set; }
        public object[] customDyes { get; set; }
        public Arrangement[] arrangements { get; set; }
        public bool hasGeometry { get; set; }
    }

    public class Lockeddye
    {
        public int channelHash { get; set; }
        public int dyeHash { get; set; }
    }

    public class Arrangement
    {
        public int classHash { get; set; }
        public long artArrangementHash { get; set; }
    }

    public class Preview
    {
        public string screenStyle { get; set; }
        public int previewVendorHash { get; set; }
        public string previewActionString { get; set; }
    }

    public class Quality
    {
        public object[] itemLevels { get; set; }
        public int qualityLevel { get; set; }
        public string infusionCategoryName { get; set; }
        public long infusionCategoryHash { get; set; }
        public long[] infusionCategoryHashes { get; set; }
        public int progressionLevelRequirementHash { get; set; }
        public int currentVersion { get; set; }
        public Version[] versions { get; set; }
        public string[] displayVersionWatermarkIcons { get; set; }
    }

    public class Version
    {
        public long powerCapHash { get; set; }
    }

    public class Objectives
    {
        public long[] objectiveHashes { get; set; }
        public long[] displayActivityHashes { get; set; }
        public bool requireFullObjectiveCompletion { get; set; }
        public int questlineItemHash { get; set; }
        public string narrative { get; set; }
        public string objectiveVerbName { get; set; }
        public string questTypeIdentifier { get; set; }
        public long questTypeHash { get; set; }
        public int completionRewardSiteHash { get; set; }
        public int nextQuestStepRewardSiteHash { get; set; }
        public int timestampUnlockValueHash { get; set; }
        public bool isGlobalObjectiveItem { get; set; }
        public bool useOnObjectiveCompletion { get; set; }
        public int inhibitCompletionUnlockValueHash { get; set; }
        public Perobjectivedisplayproperty[] perObjectiveDisplayProperties { get; set; }
        public bool displayAsStatTracker { get; set; }
    }

    public class Perobjectivedisplayproperty
    {
        public bool displayOnItemPreviewScreen { get; set; }
    }

    public class Sockets
    {
        public string detail { get; set; }
        public Socketentry[] socketEntries { get; set; }
        public Intrinsicsocket[] intrinsicSockets { get; set; }
        public Socketcategory[] socketCategories { get; set; }
    }

    public class Socketentry
    {
        public long socketTypeHash { get; set; }
        public long singleInitialItemHash { get; set; }
        public Reusableplugitem[] reusablePlugItems { get; set; }
        public bool preventInitializationOnVendorPurchase { get; set; }
        public bool preventInitializationWhenVersioning { get; set; }
        public bool hidePerksInItemTooltip { get; set; }
        public int plugSources { get; set; }
        public int reusablePlugSetHash { get; set; }
        public bool overridesUiAppearance { get; set; }
        public bool defaultVisible { get; set; }
    }

    public class Reusableplugitem
    {
        public int plugItemHash { get; set; }
    }

    public class Intrinsicsocket
    {
        public long plugItemHash { get; set; }
        public long socketTypeHash { get; set; }
        public bool defaultVisible { get; set; }
    }

    public class Socketcategory
    {
        public long socketCategoryHash { get; set; }
        public int[] socketIndexes { get; set; }
    }

    public class Talentgrid
    {
        public int talentGridHash { get; set; }
        public string itemDetailString { get; set; }
        public int hudDamageType { get; set; }
    }

    public class Investmentstat
    {
        public long statTypeHash { get; set; }
        public int value { get; set; }
        public bool isConditionallyActive { get; set; }
    }

    public class Perk
    {
        public string requirementDisplayString { get; set; }
        public int perkHash { get; set; }
        public int perkVisibility { get; set; }
    }

    public class Messagedata
    {
    }
}
