using BungieNetApi.Enums;
using static BungieNetApi.Enums.ActivityType;

namespace CommonData.Localization
{
    public static partial class Translation
    {
        public static ActivityType GetActivityType(string mode)
        {
            var pair = StatsActivityNames.FirstOrDefault(x => x.Value.Any(y => y.ToLower() == mode));

            if (pair.Value is null)
                return ActivityType.None;

            return pair.Key;
        }

        public static Dictionary<ActivityType, string[]> StatsActivityNames
        {
            get
            {
                return ActivityNames.Where(x => x.Key is
                Story or
                Strike or
                Raid or
                AllPvP or
                Patrol or
                AllPvE or
                AllMayhem or
                Supremacy or
                Survival or
                Countdown or
                TrialsOfTheNine or
                IronBannerControl or
                ScoredNightfall or
                Rumble or
                Showdown or
                Lockdown or
                ScorchedTeam or
                Gambit or
                Breakthrough or
                BlackArmoryRun or
                ClashQuickplay or
                ControlQuickplay or
                GambitPrime or
                Reckoning or
                Menagerie or
                VexOffensive or
                NightmareHunt or
                Elimination or
                Momentum or
                Dungeon or
                Sundial or
                TrialsOfOsiris or
                Dares
                ).ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public readonly static Dictionary<ActivityType, string[]> ActivityNames = new()
        {
            [None] = new[] { "Невизначено", "None" },
            [Story] = new[] { "Сюжет", "Story" },
            [Strike] = new[] { "Наліт", "Strike" },
            [Raid] = new[] { "Рейд", "Raid" },
            [AllPvP] = new[] { "ПвП", "PvP" },
            [Patrol] = new[] { "Фріплей", "Freeplay" },
            [AllPvE] = new[] { "ПвЕ", "PvE" },
            [Reserved9] = new[] { "Зарезервовано_9", "Reserved_9" },
            [Control] = new[] { "Контроль, легасі", "Control, legacy" },
            [Reserved11] = new[] { "Зарезервовано_11", "Reserved_11" },
            [Clash] = new[] { "Зіткнення, легасі", "Clash, legacy" },
            [Reserved13] = new[] { "Зарезервовано_13", "Reserved_13" },
            [CrimsonDoubles] = new[] { "Багряні дні, Напарники", "Crimson Doubles" },
            [Nightfall] = new[] { "Найтфол, легасі", "Nightfall, legacy" },
            [HeroicNightfall] = new[] { "Героїчний найтфол, легасі", "Heroic Nightfall, legacy" },
            [AllStrikes] = new[] { "Усі нальоти", "All Strikes" },
            [IronBanner] = new[] { "Залізний стяг, легасі", "Iron Banner, legacy" },
            [Reserved20] = new[] { "Зарезервовано_20", "Reserved_20" },
            [Reserved21] = new[] { "Зарезервовано_21", "Reserved_21" },
            [Reserved22] = new[] { "Зарезервовано_22", "Reserved_22" },
            [Reserved24] = new[] { "Зарезервовано_24", "Reserved_24" },
            [AllMayhem] = new[] { "Хаос", "Mayhem" },
            [Reserved26] = new[] { "Зарезервовано_26", "Reserved_26" },
            [Reserved27] = new[] { "Зарезервовано_27", "Reserved_27" },
            [Reserved28] = new[] { "Зарезервовано_28", "Reserved_28" },
            [Reserved29] = new[] { "Зарезервовано_29", "Reserved_29" },
            [Reserved30] = new[] { "Зарезервовано_30", "Reserved_30" },
            [Supremacy] = new[] { "Панування", "Supremacy" },
            [PrivateMatchesAll] = new[] { "Приватні матчі", "Private matches" },
            [Survival] = new[] { "Виживання", "Survival" },
            [Countdown] = new[] { "Зворотний відлік", "Countdown" },
            [TrialsOfTheNine] = new[] { "Випробування Дев'яти", "Trials Of The Nine" },
            [Social] = new[] { "Соціальні локації", "Social" },
            [TrialsCountdown] = new[] { "Випробування Дев'яти, Зворотний відлік", "Trials Of The Nine, Countdown" },
            [TrialsSurvival] = new[] { "Випробування Дев'яти, Виживання", "Trials Of The Nine, Survival" },
            [IronBannerControl] = new[] { "Залізний стяг", "Iron Banner" },
            [IronBannerClash] = new[] { "Залізний стяг, Зіткнення", "Iron Banner, Clash" },
            [IronBannerSupremacy] = new[] { "Залізний стяг, Панування", "Iron Banner, Supremacy" },
            [ScoredNightfall] = new[] { "Найтфол", "Nightfall" },
            [ScoredHeroicNightfall] = new[] { "Героїчний найтфол", "Heroic Nightfall" },
            [Rumble] = new[] { "Сутичка", "Rumble" },
            [AllDoubles] = new[] { "Усі напарники", "All Doubles" },
            [Doubles] = new[] { "Напарники", "Doubles" },
            [PrivateMatchesClash] = new[] { "Приватний матч, Зіткнення", "Private matches, Clash" },
            [PrivateMatchesControl] = new[] { "Приватний матч, Контроль", "Private matches, Control" },
            [PrivateMatchesSupremacy] = new[] { "Приватний матч, Панування", "Private matches, Supremacy" },
            [PrivateMatchesCountdown] = new[] { "Приватний матч, Зворотний відлік", "Private matches, Countdown" },
            [PrivateMatchesSurvival] = new[] { "Приватний матч, Виживання", "Private matches, Survival" },
            [PrivateMatchesMayhem] = new[] { "Приватний матч, Хаос", "Private matches, Mayhem" },
            [PrivateMatchesRumble] = new[] { "Приватний матч, Сутичка", "Private matches, Rumble" },
            [HeroicAdventure] = new[] { "Героїчна пригода", "Heroic Adventure" },
            [Showdown] = new[] { "Поєдинок", "Showdown" },
            [Lockdown] = new[] { "Ізоляція", "Lockdown" },
            [Scorched] = new[] { "Обпалення", "Scorched" },
            [ScorchedTeam] = new[] { "Командне обпалення", "Team Scorched" },
            [Gambit] = new[] { "Гамбіт", "Gambit" },
            [AllPvECompetitive] = new[] { "ПвЕ змагальне", "PvE Competitive" },
            [Breakthrough] = new[] { "Прорив", "Breakthrough" },
            [BlackArmoryRun] = new[] { "Активація кузні", "Black Armory" },
            [Salvage] = new[] { "Порятунок", "Salvage" },
            [IronBannerSalvage] = new[] { "Залізний стяг, Порятунок", "Iron Banner, Salvage" },
            [PvPCompetitive] = new[] { "ПвП змагальний", "PvP Competitive" },
            [PvPQuickplay] = new[] { "ПвП плейлист", "PvP Quickplay" },
            [ClashQuickplay] = new[] { "Зіткнення", "Clash" },
            [ClashCompetitive] = new[] { "Зіткнення змагальне", "Clash Competitive" },
            [ControlQuickplay] = new[] { "Контроль", "Control" },
            [ControlCompetitive] = new[] { "Контроль змагальний", "Control Competitive" },
            [GambitPrime] = new[] { "Гамбіт Прайм", "Gambit Prime" },
            [Reckoning] = new[] { "Суд", "Reckoning" },
            [Menagerie] = new[] { "Паноптикум", "Menagerie" },
            [VexOffensive] = new[] { "Наступ вексів", "Vex Offensive" },
            [NightmareHunt] = new[] { "Полювання на кошмарів", "Nightmare Hunt" },
            [Elimination] = new[] { "Ліквідація", "Elimination" },
            [Momentum] = new[] { "Управління інерцією", "Momentum Control" },
            [Dungeon] = new[] { "Підземелля", "Dungeon" },
            [Sundial] = new[] { "Сонячний годинник", "Sundial" },
            [TrialsOfOsiris] = new[] { "Випробування Осіріса", "Trials Of Osiris" },
            [Dares] = new[] { "Виклик вічності", "Dares Of Eternity" }
        };
    }
}
