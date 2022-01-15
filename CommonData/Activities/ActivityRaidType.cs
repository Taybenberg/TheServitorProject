namespace CommonData.Activities
{
    public partial class Activity
    {
        public enum ActivityRaidType
        {
            Undefined,
            LW,
            GOS,
            DSC,
            VOGL,
            VOGM
        }

        public static ActivityRaidType[] ActivityRaidTypes =>
            new ActivityRaidType[]
            {
                ActivityRaidType.LW,
                ActivityRaidType.GOS,
                ActivityRaidType.DSC,
                ActivityRaidType.VOGL,
                ActivityRaidType.VOGM
            };

        public static ActivityRaidType GetRaidType(string raidType) =>
                raidType.ToLower() switch
                {
                    "lw" or "лв" or "об" => ActivityRaidType.LW,
                    "gos" or "сп" or "сс" => ActivityRaidType.GOS,
                    "dsc" or "сгк" => ActivityRaidType.DSC,
                    "vog" or "vogl" or "вог" or "вогл" or "кс" => ActivityRaidType.VOGL,
                    "vogm" or "вогм" or "ксм" => ActivityRaidType.VOGM,
                    _ => ActivityRaidType.Undefined
                };

        public static ActivityRaidType GetRaidType(long hash) =>
            hash switch
            {
                1661734046 or 2122313384 or 2214608156 or 2214608157 => ActivityRaidType.LW,
                3458480158 or 2497200493 or 2659723068 or 3845997235 => ActivityRaidType.GOS,
                910380154 or 3976949817 => ActivityRaidType.DSC,
                3881495763 => ActivityRaidType.VOGL,
                1681562271 => ActivityRaidType.VOGM,
                _ => ActivityRaidType.Undefined
            };
    }
}
