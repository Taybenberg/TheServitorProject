using CommonData.Activities;
using static CommonData.Activities.Activity.ActivityRaidType;

namespace CommonData.Localization
{
    public static partial class Translation
    {
        public readonly static Dictionary<Activity.ActivityRaidType, string> ActivityRaidTypes = new()
        {
            [Undefined] = null,
            [LW] = "Last Wish",
            [GOS] = "Garden of Salvation",
            [DSC] = "Deep Stone Crypt",
            [VOGL] = "Vault of Glass: Legend",
            [VOGM] = "Vault of Glass: Master",
        };
    }
}
