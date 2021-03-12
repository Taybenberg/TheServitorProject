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
            [Strike] = "Страйк",
            [Raid] = "Рейд",
            [AllPvP] = "ПвП",
            [Patrol] = "Патруль",
            [AllPvE] = "ПвЕ",
            [Control] = "Контроль",
            [AllMayhem] = "Хаос",
            [Survival] = "Виживання",
            [IronBannerControl] = "Баннер",
            [ScoredNightfall] = "Наліт",
            [Gambit] = "Гамбіт",
            [BlackArmoryRun] = "Кузня",
            [GambitPrime] = "Гамбіт Прайм",
            [Reckoning] = "Суд",
            [Menagerie] = "Паноптикум",
            [NightmareHunt] = "Полювання на кошмари",
            [Dungeon] = "Підземелля",
            [TrialsOfOsiris] = "Випробування Осіріса"
        };

        public readonly static Dictionary<ActivityType, string> ActivityNames = new()
        {
            [None] = "0",
            [Story] = "2",
            [Strike] = "3",
            [Raid] = "4",
            [AllPvP] = "5",
            [Patrol] = "6",
            [AllPvE] = "7",
            [Reserved9] = "9",
            [Control] = "10",
            [Reserved11] = "11",
            [Clash] = "12",
            [Reserved13] = "13",
            [CrimsonDoubles] = "15",
            [Nightfall] = "16",
            [HeroicNightfall] = "17",
            [AllStrikes] = "18",
            [IronBanner] = "19",
            [Reserved20] = "20",
            [Reserved21] = "21",
            [Reserved22] = "22",
            [Reserved24] = "24",
            [AllMayhem] = "25",
            [Reserved26] = "26",
            [Reserved27] = "27",
            [Reserved28] = "28",
            [Reserved29] = "29",
            [Reserved30] = "30",
            [Supremacy] = "31",
            [PrivateMatchesAll] = "32",
            [Survival] = "37",
            [Countdown] = "38",
            [TrialsOfTheNine] = "39",
            [Social] = "40",
            [TrialsCountdown] = "41",
            [TrialsSurvival] = "42",
            [IronBannerControl] = "43",
            [IronBannerClash] = "44",
            [IronBannerSupremacy] = "45",
            [ScoredNightfall] = "46",
            [ScoredHeroicNightfall] = "47",
            [Rumble] = "48",
            [AllDoubles] = "49",
            [Doubles] = "50",
            [PrivateMatchesClash] = "51",
            [PrivateMatchesControl] = "52",
            [PrivateMatchesSupremacy] = "53",
            [PrivateMatchesCountdown] = "54",
            [PrivateMatchesSurvival] = "55",
            [PrivateMatchesMayhem] = "56",
            [PrivateMatchesRumble] = "57",
            [HeroicAdventure] = "58",
            [Showdown] = "59",
            [Lockdown] = "60",
            [Scorched] = "61",
            [ScorchedTeam] = "62",
            [Gambit] = "63",
            [AllPvECompetitive] = "64",
            [Breakthrough] = "65",
            [BlackArmoryRun] = "66",
            [Salvage] = "67",
            [IronBannerSalvage] = "68",
            [PvPCompetitive] = "69",
            [PvPQuickplay] = "70",
            [ClashQuickplay] = "71",
            [ClashCompetitive] = "72",
            [ControlQuickplay] = "73",
            [ControlCompetitive] = "74",
            [GambitPrime] = "75",
            [Reckoning] = "76",
            [Menagerie] = "77",
            [VexOffensive] = "78",
            [NightmareHunt] = "79",
            [Elimination] = "80",
            [Momentum] = "81",
            [Dungeon] = "82",
            [Sundial] = "83",
            [TrialsOfOsiris] = "84"
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
