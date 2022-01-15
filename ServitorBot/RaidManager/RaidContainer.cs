using CommonData.DiscordEmoji;
using CommonData.Localization;
using CommonData.Activities;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Timers;

namespace ServitorDiscordBot
{
    public class RaidContainer : IDisposable
    {
        public record Reservation
        {
            public int Position { get; set; }

            public ulong ID { get; set; }
        }

        const int notifyIntervalMin = -10;
        const int deleteIntervalMin = 60;

        private Timer _notifyTimer = new();
        private Timer _deleteTimer = new();

        public event Func<ulong, Task> Notify;
        public event Func<ulong, Task> Update;
        public event Func<ulong, Task> Delete;

        public ulong ID { get; set; }

        public DateTime PlannedDate { get; set; }

        public ActivityRaidType RaidType { get; set; }

        public string Description { get; set; } = string.Empty;

        public List<Reservation> Reservations { get; set; } = new();

        [JsonIgnore]
        public bool IsActual
        {
            get => PlannedDate.AddMinutes(deleteIntervalMin) > DateTime.Now;
        }

        [JsonIgnore]
        public string RaidIcon
        {
            get => Emote.Parse(CommonData.DiscordEmoji.Emoji.GetActivityRaidEmoji(RaidType)).Url;
        }

        [JsonIgnore]
        public string RaidName
        {
            get => Translation.ActivityRaidTypes[RaidType];
        }

        [JsonIgnore]
        public List<Reservation> ReservationsOrdered
        {
            get => Reservations.OrderBy(x => x.Position).ToList();
        }

        public void Dispose()
        {
            _notifyTimer.Stop();
            _notifyTimer.Dispose();

            _deleteTimer.Stop();
            _deleteTimer.Dispose();

            Notify = null;
            Update = null;
            Delete = null;
        }

        public void Start()
        {
            _deleteTimer.AutoReset = false;
            _notifyTimer.AutoReset = false;

            _deleteTimer.Elapsed += (_, _) =>
            {
                _deleteTimer.Stop();
                Delete?.Invoke(ID);
            };

            _notifyTimer.Elapsed += (_, _) =>
            {
                _notifyTimer.Stop();
                Notify?.Invoke(ID);

                _deleteTimer.Interval = (PlannedDate.AddMinutes(deleteIntervalMin) - DateTime.Now).TotalMilliseconds;
                _deleteTimer.Start();
            };

            var currDate = DateTime.Now;
            var notifyDate = PlannedDate.AddMinutes(notifyIntervalMin);
            var deleteDate = PlannedDate.AddMinutes(deleteIntervalMin);

            if (currDate < notifyDate)
            {
                _notifyTimer.Interval = (notifyDate - currDate).TotalMilliseconds;
                _notifyTimer.Start();
            }
            else if (currDate < deleteDate)
            {
                _deleteTimer.Interval = (deleteDate - currDate).TotalMilliseconds;
                _deleteTimer.Start();
            }
            else
            {
                Stop();
            }
        }

        public void Stop(bool forever = true)
        {
            _notifyTimer.Stop();
            _deleteTimer.Stop();

            if (forever)
            {
                Delete?.Invoke(ID);
            }
            else
            {
                _notifyTimer.Dispose();
                _deleteTimer.Dispose();

                _notifyTimer = new();
                _deleteTimer = new();
            }
        }

        public void TryStop(ulong id)
        {
            if (ReservationsOrdered.First().ID == id)
                Stop();
        }

        public void UpdateDate(ulong id, DateTime date)
        {
            if (ReservationsOrdered.First().ID == id)
            {
                Stop(false);

                PlannedDate = date;

                Start();

                Update?.Invoke(ID);
            }
        }

        public void TransferPlace(ulong senderID, ulong receiverID)
        {
            var sender = Reservations.Find(x => x.ID == senderID);
            var receiver = Reservations.Find(x => x.ID == receiverID);

            if (sender is not null)
            {
                if (receiver is not null)
                {
                    if (receiver.Position > sender.Position)
                    {
                        sender.ID = receiverID;
                        receiver.ID = senderID;
                    }
                }
                else
                    sender.ID = receiverID;

                Update?.Invoke(ID);
            }
        }

        public void AddUser(ulong id)
        {
            if (!Reservations.Any(x => x.ID == id))
            {
                Reservations.Add(new Reservation
                {
                    Position = (ReservationsOrdered.LastOrDefault()?.Position ?? -1) + 1,
                    ID = id
                });

                Update?.Invoke(ID);
            }
        }

        public void AddUsers(ulong id, IEnumerable<ulong> userIDs)
        {
            if (ReservationsOrdered.First().ID == id)
            {
                var pos = ReservationsOrdered.Last().Position;

                foreach (var userID in userIDs)
                {
                    if (!Reservations.Any(x => x.ID == userID))
                        Reservations.Add(new Reservation
                        {
                            Position = ++pos,
                            ID = userID
                        });
                }

                Update?.Invoke(ID);
            }
        }

        public void RemoveUser(ulong id)
        {
            var res = Reservations.Find(x => x.ID == id);

            if (res is not null)
            {
                Reservations.Remove(res);

                foreach (var r in Reservations.Where(x => x.Position > res.Position))
                    r.Position--;

                if (Reservations.Count > 0)
                    Update?.Invoke(ID);
                else
                    Stop();
            }
        }

        public void DecorateBuilder(EmbedBuilder builder)
        {
            builder.ThumbnailUrl = RaidIcon;

            builder.Description = Description;

            builder.Title = $"{RaidName} @ {PlannedDate.ToString("dd.MM.yyyy HH:mm")}";

            var res = ReservationsOrdered;

            builder.Fields = new List<EmbedFieldBuilder>()
            {
                new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Організатор збору",
                    Value = $"<@{res.First().ID}>"
                }
            };

            var fireteam = res.Skip(1).Take(5);

            if (fireteam.Count() > 0)
                builder.Fields.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Бойова група",
                    Value = string.Join("\n", fireteam
                            .Select(x => $"<@{x.ID}>"))
                });

            var reserve = res.Skip(6);

            if (reserve.Count() > 0)
                builder.Fields.Add(new EmbedFieldBuilder
                {
                    IsInline = false,
                    Name = "Лава запасних",
                    Value = string.Join("\n", reserve
                            .Select(x => $"<@{x.ID}>"))
                });
        }
    }
}
