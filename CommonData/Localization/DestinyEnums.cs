using BungieSharper.Entities.Destiny;
using static BungieSharper.Entities.Destiny.DestinyClass;
using static BungieSharper.Entities.Destiny.DestinyGender;
using static BungieSharper.Entities.Destiny.DestinyRace;

namespace CommonData.Localization
{
    public static partial class Translation
    {
        public readonly static Dictionary<DestinyClass, string> ClassNames = new()
        {
            [Titan] = "Титан",
            [Hunter] = "Мисливець",
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
