using static CommonData.Activities.ActivityRaidType;

namespace CommonData.Activities
{
    public class Activity
    {
        public static ActivityRaidType GetRaidType(string raidType) =>
            raidType.ToLower() switch
            {
                "lw" or "лв" or "об" => ActivityRaidType.LW,
                "gos" or "сп" or "сс" => ActivityRaidType.GOS,
                "dsc" or "сгк" => ActivityRaidType.DSC,
                "vog" or "вог" or "кс" => ActivityRaidType.VOG_L,
                "vogm" or "вогм" or "ксм" => ActivityRaidType.VOG_M,
                _ => ActivityRaidType.Undefined
            };

        public static ActivityRaidType GetRaidType(long hash) =>
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
