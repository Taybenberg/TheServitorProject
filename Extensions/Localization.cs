using BungieNetApi;
using System.Collections.Generic;
using static BungieNetApi.ActivityType;
using static BungieNetApi.DestinyClass;
using static BungieNetApi.DestinyGender;
using static BungieNetApi.DestinyRace;

namespace Extensions
{
    public static class Localization
    {
        public readonly static Dictionary<string, string> StatNames = new()
        {
            ["lbSingleGameKills"] = "Вбивств за гру",
            ["lbPrecisionKills"] = "Прицільних вбивств",
            ["lbAssists"] = "Ассісти",
            ["lbDeaths"] = "Смертей",
            ["lbKills"] = "Вбивств",
            ["lbObjectivesCompleted"] = "Виконаних цілей",
            ["lbMostPrecisionKills"] = "Прицільних вбивств за гру",
            ["lbLongestKillSpree"] = "Найбільше вбивств за одне життя",
            ["lbLongestKillDistance"] = "Найбільша дистанція вбивства",
            ["lbFastestCompletionMs"] = "Найшвидше закриття",
            ["lbLongestSingleLife"] = "Найдовше життя",
            ["lbSingleGameScore"] = "Очок за гру"
        };

        public readonly static Dictionary<ActivityType, string> StatsActivityNames = new()
        {
            [Story] = "Сюжет",
            [Strike] = "Наліт",
            [Raid] = "Рейд",
            [AllPvP] = "ПвП",
            [Patrol] = "Патруль",
            [AllPvE] = "ПвЕ",
            [AllMayhem] = "Хаос",
            [IronBanner] = "Залізний стяг",
            [Supremacy] = "Панування",
            [Survival] = "Виживання",
            [TrialsOfTheNine] = "Випробування Дев'яти",
            [Countdown] = "Зворотний відлік",
            [ScoredNightfall] = "Найтфол",
            [Rumble] = "Сутичка",
            [Showdown] = "Поєдинок",
            [Lockdown] = "Ізоляція",
            [Gambit] = "Гамбіт",
            [Breakthrough] = "Прорив",
            [BlackArmoryRun] = "Активація кузні",
            [ClashQuickplay] = "Зіткнення",
            [ControlQuickplay] = "Контроль",
            [GambitPrime] = "Гамбіт Прайм",
            [Reckoning] = "Суд",
            [Menagerie] = "Паноптикум",
            [VexOffensive] = "Наступ вексів",
            [NightmareHunt] = "Полювання на кошмарів",
            [Elimination] = "Ліквідація",
            [Momentum] = "Управління інерцією",
            [Dungeon] = "Підземелля",
            [Sundial] = "Сонячний годинник",
            [TrialsOfOsiris] = "Випробування Осіріса",
        };

        public readonly static Dictionary<ActivityType, string> ActivityNames = new()
        {
            [None] = "Невідомо",
            [Story] = "Сюжет",
            [Strike] = "Наліт",
            [Raid] = "Рейд",
            [AllPvP] = "ПвП",
            [Patrol] = "Патруль",
            [AllPvE] = "ПвЕ",
            [Reserved9] = "Зарезервовано_9",
            [Control] = "Контроль, легасі",
            [Reserved11] = "Зарезервовано_11",
            [Clash] = "Зіткнення, легасі",
            [Reserved13] = "Зарезервовано_13",
            [CrimsonDoubles] = "Багряні дні, напарники",
            [Nightfall] = "Найтфол, легасі",
            [HeroicNightfall] = "Героїчний найтфол, легасі",
            [AllStrikes] = "Усі страйки",
            [IronBanner] = "Залізний стяг, легасі",
            [Reserved20] = "Зарезервовано_20",
            [Reserved21] = "Зарезервовано_21",
            [Reserved22] = "Зарезервовано_22",
            [Reserved24] = "Зарезервовано_24",
            [AllMayhem] = "Хаос",
            [Reserved26] = "Зарезервовано_26",
            [Reserved27] = "Зарезервовано_27",
            [Reserved28] = "Зарезервовано_28",
            [Reserved29] = "Зарезервовано_29",
            [Reserved30] = "Зарезервовано_30",
            [Supremacy] = "Панування",
            [PrivateMatchesAll] = "Приватні матчі",
            [Survival] = "Виживання",
            [Countdown] = "Зворотний відлік",
            [TrialsOfTheNine] = "Випробування Дев'яти",
            [Social] = "Соціальні локації",
            [TrialsCountdown] = "Випробування Дев'яти, зворотний відлік",
            [TrialsSurvival] = "Випробування Дев'яти, виживання",
            [IronBannerControl] = "Залізний стяг",
            [IronBannerClash] = "Залізний стяг, зіткнення",
            [IronBannerSupremacy] = "Залізний стяг, панування",
            [ScoredNightfall] = "Найтфол",
            [ScoredHeroicNightfall] = "Героїчний найтфол",
            [Rumble] = "Сутичка",
            [AllDoubles] = "Усі напарники",
            [Doubles] = "Напарники",
            [PrivateMatchesClash] = "Приватний матч, зіткнення",
            [PrivateMatchesControl] = "Приватний матч, контроль",
            [PrivateMatchesSupremacy] = "Приватний матч, панування",
            [PrivateMatchesCountdown] = "Приватний матч, зворотний відлік",
            [PrivateMatchesSurvival] = "Приватний матч, виживання",
            [PrivateMatchesMayhem] = "Приватний матч, хаос",
            [PrivateMatchesRumble] = "Приватний матч, сутичка",
            [HeroicAdventure] = "Героїчна пригода",
            [Showdown] = "Поєдинок",
            [Lockdown] = "Ізоляція",
            [Scorched] = "Обпалення",
            [ScorchedTeam] = "Командне обпалення",
            [Gambit] = "Гамбіт",
            [AllPvECompetitive] = "ПвЕ змагальне",
            [Breakthrough] = "Прорив",
            [BlackArmoryRun] = "Активація кузні",
            [Salvage] = "Порятунок",
            [IronBannerSalvage] = "Залізний стяг, порятунок",
            [PvPCompetitive] = "ПвП змагальний",
            [PvPQuickplay] = "ПвП плейлист",
            [ClashQuickplay] = "Зіткнення",
            [ClashCompetitive] = "Зіткнення змагальне",
            [ControlQuickplay] = "Контроль",
            [ControlCompetitive] = "Контроль змагальний",
            [GambitPrime] = "Гамбіт Прайм",
            [Reckoning] = "Суд",
            [Menagerie] = "Паноптикум",
            [VexOffensive] = "Наступ вексів",
            [NightmareHunt] = "Полювання на кошмарів",
            [Elimination] = "Ліквідація",
            [Momentum] = "Управління інерцією",
            [Dungeon] = "Підземелля",
            [Sundial] = "Сонячний годинник",
            [TrialsOfOsiris] = "Випробування Осіріса"
        };

        public readonly static Dictionary<DestinyClass, string> ClassNames = new()
        {
            [Titan] = "Титан",
            [Hunter] = "Хантер",
            [Warlock] = "Варлок",
            [DestinyClass.Unknown] = "Невідомо"
        };

        public readonly static Dictionary<DestinyGender, string> GenderNames = new()
        {
            [Male] = "Чоловік",
            [Female] = "Жінка",
            [DestinyGender.Unknown] = "Невідомо"
        };

        public readonly static Dictionary<DestinyRace, string> RaceNames = new()
        {
            [Human] = "Людина",
            [Awoken] = "Пробуджена",
            [Exo] = "Екзо",
            [DestinyRace.Unknown] = "Невідомо"
        };
    }
}
