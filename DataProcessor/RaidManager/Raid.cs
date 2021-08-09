using static DataProcessor.RaidManager.RaidType;

namespace DataProcessor.RaidManager
{
    class Raid
    {
        public static RaidType GetRaidType(long hash) =>
            hash switch
            {
                1661734046 or 2122313384 or 2214608156 or 2214608157 => LW,
                3458480158 or 2497200493 or 2659723068 or 3845997235 => GOS,
                910380154 or 3976949817 => DSC,
                3881495763 => VOG_L,
                1681562271 => VOG_M,
                _ => Undefined
            };
    }
}
