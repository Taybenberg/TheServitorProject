﻿using BungieNetApi;
using BungieNetApi.Enums;
using Database;
using DataProcessor.Localization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataProcessor.DatabaseWrapper
{
    public class SuspiciousActivities : IWrapper
    {
        public record User
        {
            public bool IsClanMember { get; internal set; }
            public string UserName { get; internal set; }
            public string ClanSign { get; internal set; }
            public string ClanName { get; internal set; }
        }

        public record Activity
        {
            public string Type { get; internal set; }
            public DateTime Period { get; internal set; }
            public float? Score { get; internal set; }
            public IEnumerable<User> Users { get; internal set; }
        }

        public IEnumerable<Activity> Activities { get; private set; }

        private readonly bool _isNightfallsOnly;

        private readonly IApiClient _apiClient;

        private readonly IClanDB _clanDB;

        internal SuspiciousActivities(IClanDB clanDB, IApiClient apiClient, bool isNightfallsOnly) =>
            (_clanDB, _apiClient, _isNightfallsOnly) = (clanDB, apiClient, isNightfallsOnly);

        public async Task InitAsync()
        {
            var activities = _isNightfallsOnly ? await _clanDB.GetSuspiciousNightfallsOnlyAsync() : await _clanDB.GetSuspiciousActivitiesWithoutNightfallsAsync();

            var userIDs = (await _clanDB.GetUsersAsync()).Select(x => x.UserID);

            ConcurrentBag<Activity> acts = new();

            Parallel.ForEach(activities, (activity) =>
            {
                var act = _apiClient.EntityFactory.GetActivity(activity.ActivityID);

                ConcurrentBag<User> users = new();

                Parallel.ForEach(act.UserStats.Select(x => Tuple.Create(x.MembershipID, x.MembershipType, x.DisplayName)).Distinct(), (user) =>
                {
                    if (userIDs.Any(x => x == user.Item1))
                    {
                        users.Add(new User
                        {
                            IsClanMember = true,
                            UserName = user.Item3,
                            ClanSign = "UA"
                        });
                    }
                    else
                    {
                        var clan = _apiClient.EntityFactory.GetUser(user.Item1, user.Item2).GetUserClanAsync().Result;

                        users.Add(new User
                        {
                            IsClanMember = false,
                            UserName = user.Item3,
                            ClanSign = clan?.ClanSign,
                            ClanName = clan?.ClanName
                        });
                    }
                });

                acts.Add(new Activity
                {
                    Type = TranslationDictionaries.ActivityNames[activity.ActivityType][0],
                    Period = activity.Period,
                    Score = activity.ActivityType == ActivityType.ScoredNightfall ? act.UserStats.FirstOrDefault()?.TeamScore : null,
                    Users = users
                });
            });

            Activities = acts.OrderByDescending(x => x.Period);
        }
    }
}
