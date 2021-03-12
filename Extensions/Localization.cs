using BungieNetApi;
using System.Collections.Generic;
using static BungieNetApi.ActivityType;

namespace Extensions
{
    public static class Localization
    {
        public readonly static Dictionary<ActivityType, string> ActivityNames = new()
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
    }
}
