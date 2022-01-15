using CommonData.Activities;
using static CommonData.Activities.ActivityRaidType;

namespace CommonData.Localization
{
    public static partial class Translation
    {
        public readonly static Dictionary<ActivityRaidType, string> ActivityRaidTypes = new()
        {
            [Undefined] = null,
            [LW] = "Last Wish",
            [GOS] = "Garden of Salvation",
            [DSC] = "Deep Stone Crypt",
            [VOG_L] = "Vault of Glass: Legend",
            [VOG_M] = "Vault of Glass: Master",
        };
    }
}
