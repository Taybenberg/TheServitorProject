using DataProcessor.RaidManager;
using System.Collections.Generic;
using static DataProcessor.RaidManager.RaidType;

namespace DataProcessor.Localization
{
    public static partial class TranslationDictionaries
    {
        public readonly static Dictionary<RaidType, string> RaidTypes = new()
        {
            [Undefined] = "Undefined",
            [LW] = "Last Wish",
            [GOS] = "Garden of Salvation",
            [DSC] = "Deep Stone Crypt",
            [VOG_L] = "Vault of Glass: Legend",
            [VOG_M] = "Vault of Glass: Master",
        };
    }
}
