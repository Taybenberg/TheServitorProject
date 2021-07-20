namespace DataProcessor.RaidManager
{
    class Raid
    {
        public static RaidType GetRaidType(long hash) =>
            hash switch
            {
                1661734046 or 2122313384 or 2214608156 or 2214608157 => RaidType.LW,
                3458480158 or 2497200493 or 2659723068 or 3845997235 => RaidType.GOS,
                910380154 or 3976949817 => RaidType.DSC,
                3881495763 => RaidType.VOG_L,
                1681562271 => RaidType.VOG_M
            };
    }
}
