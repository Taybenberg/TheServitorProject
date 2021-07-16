using BungieNetApi.Enums;
using System.Collections.Generic;
using static BungieNetApi.Enums.ActivityType;

namespace DataProcessor.DiscordEmoji
{ 
    public static partial class EmojiContainer
    {
        public static string GetActivityEmoji(ActivityType activityType) =>
            activityType switch
            {
                Patrol => "<:Patrol:865683931752038450>",
                Story => "<:Story:865685179531395102>",
                Dungeon => "<:Dungeon:865689171322601484>",
                Raid => "<:Raid:865691862891364382>",
                /*
                Strike => "НалітStrike",
                AllPvP => "ПвПPvP",
                AllPvE => "ПвЕPvE",
                Control => "Контроль, легасіControl, legacy",
                Clash => "Зіткнення, легасіClash, legacy",
                CrimsonDoubles => "Багряні дні, НапарникиCrimson Doubles",
                Nightfall => "Найтфол, легасіNightfall, legacy",
                HeroicNightfall => "Героїчний найтфол, легасіHeroic Nightfall, legacy",
                AllStrikes => "Усі нальотиAll Strikes",
                IronBanner => "Залізний стяг, легасіIron Banner, legacy",
                AllMayhem => "ХаосMayhem",
                Supremacy => "ПануванняSupremacy",
                PrivateMatchesAll => "Приватні матчіPrivate matches",
                Survival => "ВиживанняSurvival",
                Countdown => "Зворотний відлікCountdown",
                TrialsOfTheNine => "Випробування Дев'ятиTrials Of The Nine",
                Social => "Соціальні локаціїSocial",
                TrialsCountdown => "Випробування Дев'яти, Зворотний відлікTrials Of The Nine, Countdown",
                TrialsSurvival => "Випробування Дев'яти, ВиживанняTrials Of The Nine, Survival",
                IronBannerControl => "Залізний стягIron Banner",
                IronBannerClash => "Залізний стяг, ЗіткненняIron Banner, Clash",
                IronBannerSupremacy => "Залізний стяг, ПануванняIron Banner, Supremacy",
                ScoredNightfall => "НайтфолNightfall",
                ScoredHeroicNightfall => "Героїчний найтфолHeroic Nightfall",
                Rumble => "СутичкаRumble",
                AllDoubles => "Усі напарникиAll Doubles",
                Doubles => "НапарникиDoubles",
                PrivateMatchesClash => "Приватний матч, ЗіткненняPrivate matches, Clash",
                PrivateMatchesControl => "Приватний матч, КонтрольPrivate matches, Control",
                PrivateMatchesSupremacy => "Приватний матч, ПануванняPrivate matches, Supremacy",
                PrivateMatchesCountdown => "Приватний матч, Зворотний відлікPrivate matches, Countdown",
                PrivateMatchesSurvival => "Приватний матч, ВиживанняPrivate matches, Survival",
                PrivateMatchesMayhem => "Приватний матч, ХаосPrivate matches, Mayhem",
                PrivateMatchesRumble => "Приватний матч, СутичкаPrivate matches, Rumble",
                HeroicAdventure => "Героїчна пригодаHeroic Adventure",
                Showdown => "ПоєдинокShowdown",
                Lockdown => "ІзоляціяLockdown",
                Scorched => "ОбпаленняScorched",
                ScorchedTeam => "Командне обпаленняScorched Team",
                Gambit => "ГамбітGambit",
                AllPvECompetitive => "ПвЕ змагальнеPvE Competitive",
                Breakthrough => "ПроривBreakthrough",
                BlackArmoryRun => "Активація кузніBlack Armory",
                Salvage => "ПорятунокSalvage",
                IronBannerSalvage => "Залізний стяг, ПорятунокIron Banner, Salvage",
                PvPCompetitive => "ПвП змагальнийPvP Competitive",
                PvPQuickplay => "ПвП плейлистPvP Quickplay",
                ClashQuickplay => "ЗіткненняClash",
                ClashCompetitive => "Зіткнення змагальнеClash Competitive",
                ControlQuickplay => "КонтрольControl",
                ControlCompetitive => "Контроль змагальнийControl Competitive",
                GambitPrime => "Гамбіт ПраймGambit Prime",
                Reckoning => "СудReckoning",
                Menagerie => "ПаноптикумMenagerie",
                VexOffensive => "Наступ вексівVex Offensive",
                NightmareHunt => "Полювання на кошмарівNightmare Hunt",
                Elimination => "ЛіквідаціяElimination",
                Momentum => "Управління інерцієюMomentum Control",
                Sundial => "Сонячний годинникSundial",
                TrialsOfOsiris => "Випробування ОсірісаTrials Of Osiris",*/
                _ => NoData
            };
    }
}
