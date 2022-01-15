using ClanActivitiesDatabase;
using CommonData.DiscordEmoji;
using CommonData.Localization;
using CommonData.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class MyRaids : IWrapper
    {
        public record RaidContainer
        {
            public string Emoji { get; internal set; }
            public string Name { get; internal set; }
        }

        public record ClassContainer
        {
            public string Emoji { get; internal set; }
            public string Class { get; internal set; }
            public IEnumerable<RaidContainer> Raids { get; internal set; }
        }

        public bool IsUserRegistered { get; private set; }

        public string UserName { get; private set; }

        public IEnumerable<ClassContainer> Classes { get; private set; }

        private readonly DateTime _seasonStart;

        private readonly int _weekNumber;

        private readonly ulong _userID;

        private readonly IClanActivitiesDB _clanDB;

        internal MyRaids(IClanActivitiesDB clanDB, ulong discordUserID, DateTime seasonStart, int weekNumber) =>
            (_clanDB, _userID, _seasonStart, _weekNumber) = (clanDB, discordUserID, seasonStart.ToUniversalTime(), weekNumber);

        public async Task InitAsync()
        {
            var user = await _clanDB.GetUserByDiscordIdAsync(_userID);

            if (!(IsUserRegistered = user is not null))
                return;

            UserName = user.UserName;

            var raids = await _clanDB.GetUserRaidsAsync(_userID, _seasonStart.AddDays((_weekNumber - 1) * 7));

            Classes = user.Characters.Select(c =>
            {
                var charRaids = raids.Where(x => x.ActivityUserStats.Any(y => y.Completed && y.CharacterID == c.CharacterID));

                var types = charRaids.OrderBy(x => x.Period).Select(x => Activity.GetRaidType(x.ReferenceHash)).Distinct();

                return new ClassContainer
                {
                    Emoji = Emoji.GetClassEmoji(c.Class),
                    Class = Translation.ClassNames[c.Class],
                    Raids = types.Select(t => new RaidContainer
                    {
                        Emoji = Emoji.GetActivityRaidEmoji(t),
                        Name = Translation.ActivityRaidTypes[t]
                    })
                };
            }).OrderByDescending(x => x.Raids.Count());
        }
    }
}
