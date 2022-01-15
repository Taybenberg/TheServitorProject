using CommonData.RaidManager;
using static CommonData.RaidManager.RaidType;

namespace CommonData.Localization
{
    public static partial class Translation
    {
        public readonly static Dictionary<RaidType, string> RaidTypes = new()
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
